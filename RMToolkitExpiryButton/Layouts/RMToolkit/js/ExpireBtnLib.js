var selectedDocItems;
var oSite;
var expiryDate;


function getSelectedItemsQuery() {
    /// <summary>
    /// This function returns string which contains CAML query for retrieving all selected list items
    /// </summary>
    var sb = new Sys.StringBuilder();
    var writer = new SP.XmlWriter.create(sb);
    writer.writeStartElement("Where");
    writer.writeStartElement("In");

    // FieldRef element
    writer.writeStartElement("FieldRef");
    writer.writeAttributeString("Name", "ID");
    writer.writeEndElement();

    // Values element
    var items = SP.ListOperation.Selection.getSelectedItems();
    var itemsCount = items.length;
    writer.writeStartElement("Values");
    while (items.length >= 1) {
        writer.writeStartElement("Value");
        writer.writeAttributeString("Type", "Integer");
        writer.writeString(items.pop().id);
        writer.writeEndElement();
    }
    writer.writeEndElement(); // Values
    writer.writeEndElement(); // In
    writer.writeEndElement(); // Where
    writer.close();
    return sb.toString();
}

function loadJS() {
    ExecuteOrDelayUntilScriptLoaded(processSelectedListItemsViaCAML, "SP.js");
}

function showExpiryModal() {
    
    var options = {
        url: "/_layouts/15/RMToolkitExpiryButton/ExpiryDate.aspx",
        args: null,
        title: "Select Value",
        width: 300,
        height: 250,
        dialogReturnValueCallback: Function.createDelegate(null, processSelectedListItemsViaCAML)
    };
    SP.UI.ModalDialog.showModalDialog(options);

}

function processSelectedListItemsViaCAML(dialogResult, returnValue) {
    /// <summary>
    /// This function runs a caml query on the selected items
    /// execution is then passed to onQuerySucceeded via the async call
    /// krd 25_Sept_2012
    /// </summary>
    if (dialogResult == SP.UI.DialogResult.OK) {
        var context = SP.ClientContext.get_current();
        var web = context.get_web();
        var oLists = web.get_lists();
        var currentListID = SP.ListOperation.Selection.getSelectedList();
        var oCurrentList = oLists.getById(currentListID);
        expiryDate = returnValue;
        oSite = context.get_site();
        //this call generates the caml query
        //caml query returns metadata about selected list items
        var camlString = getSelectedItemsQuery();
        var query = new SP.CamlQuery();
        var queryString = '<View><Query>' + camlString + '</Query></View>';
        query.set_viewXml(queryString);
        //need to add something here if there is nothing selected call the whole show off... @TODO
        selectedDocItems = oCurrentList.getItems(query);
        context.load(selectedDocItems, 'Include(File)');
        context.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));
    }
}

function onQuerySucceeded(sender, args) {
    //we retreived information about the list items
    var listItemInfo = '';
    var listItemEnumerator = selectedDocItems.getEnumerator();
    var contextUpdate = SP.ClientContext.get_current();
    var web = contextUpdate.get_web();
    var oLists = web.get_lists();
    var currentListID = SP.ListOperation.Selection.getSelectedList();
    var oCurrentList = oLists.getById(currentListID);
    var bWorkflowRun;

    //iterate through selected list items
    while (listItemEnumerator.moveNext()) {
        var oListItem = listItemEnumerator.get_current();
        //run "RMToolkitExpireRecord" workflow on item (expire the record)
        bWorkflowRun = false;
        var oFile = oListItem.get_file();
        var path = oFile.get_serverRelativeUrl();
        var fullitemurl = window.location.protocol + '//' + window.location.host + path;

        listItemInfo += '\nPath: ' + fullitemurl;
        //bWorkflowRun = runWorkflowOnSelectedItem(path, 'ExpireWF');
        bWorkflowRun = runWorkflowOnSelectedItem(fullitemurl, 'ExpireWF');

    }
    // alert(listItemInfo.toString());
}

function onUpdateQuerySucceeded(sender, args) {
    //do nothing the update is complete
    alert('list items have been updated');
}


function getWorkflowTemplateID(documentURL, wfName) {
    // this function gets the ID of the workflow by name given the URL
    // uses web services call to get tempalate ID of workflow in given site     
    var workflowGUID = null;


    var itemURL = documentURL;
    var testVar;
    $().SPServices({
        operation: "GetTemplatesForItem",
        item: itemURL,
        async: false,
        completefunc: function (xData, Status) {
            $(xData.responseXML).find("WorkflowTemplates > WorkflowTemplate").each(function (i, e) {
                testVar = $(xData.responseXML);
                // hard coded workflow name
                if ($(this).attr("Name") == wfName) {
                    var guid = $(this).find("WorkflowTemplateIdSet").attr("TemplateId");
                    if (guid != null) {
                        workflowGUID = "{" + guid + "}";
                    }
                }
            });
        }
    });
    return workflowGUID;

}


function runWorkflowOnSelectedItem(documentURL, wfName) {
    //  runs the workflow
    // would like to have error handling around runwf but haven't figured out how to do that yet..
    var templID = getWorkflowTemplateID(documentURL, wfName);
    var bSuccess = false;

    if (templID != null) {
        {
            bSuccess = true;
            runWF(templID, documentURL);
        }
    }
    return bSuccess;

}

//uses the web service function to start 
function runWF(templID, documentURL) {
    $().SPServices({
        operation: "StartWorkflow",
        item: documentURL,
        debug: true,
        templateId: templID,
        workflowParameters: "<Data><expiryDate>" + expiryDate + "</expiryDate></Data>",
        completefunc: function (xData, Status) {
            var out = $().SPServices.SPDebugXMLHttpResult({ node: xData.responseXML });
        }
    });

}

function onQueryFailed(sender, args) {

    alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
}

