var selectedDocItems;


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

   function loadJS()
   {
        ExecuteOrDelayUntilScriptLoaded(processSelectedListItemsViaCAML,"SP.js");
   }


   function processSelectedListItemsViaCAML() {
       /// <summary>
       /// This function runs a caml query on the selected items
       /// execution is then passed to onQuerySucceeded via the async call
       /// krd 25_Sept_2012
       /// </summary>
       var context = SP.ClientContext.get_current();
       var web = context.get_web();
       var oLists = web.get_lists();
       var currentListID = SP.ListOperation.Selection.getSelectedList();
       var oCurrentList = oLists.getById(currentListID);

       //this call generates the caml query
       //caml query returns metadata about selected list items
       var camlString = getSelectedItemsQuery();
       var query = new SP.CamlQuery();
       var queryString = '<View><Query>' + camlString + '</Query></View>';
       query.set_viewXml(queryString);
       //need to add something here if there is nothing selected call the whole show off... @TODO
       selectedDocItems = oCurrentList.getItems(query);
       context.load(selectedDocItems);
       context.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));
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
            var bApproved = oListItem.get_item('ApprovedforDeletion');
            //only process items that are approved
            if (bApproved==true) {
                //run "RMToolkitDeleteRecord" workflow on item (delete the record)
                bWorkflowRun = false;
                bWorkflowRun = runWorkflowOnSelectedItem(oListItem.get_item('DocumentURL').get_description(), 'RMToolkitDeleteRecord', oListItem.get_item('webURL').get_description());
                if (bWorkflowRun == true) {
                    listItemInfo += '\nID: ' + oListItem.get_id() + '\nTitle: ' + oListItem.get_item('Title') + '\nDocumentURL: ' + oListItem.get_item('DocumentURL').get_description();
                    //now update list item with date of deletion and the fact that the item was deleted
                    var updateListItem = oCurrentList.getItemById(oListItem.get_id());
                    contextUpdate.load(updateListItem);
                    updateListItem.set_item('Deleted', true);
                    var currDate = new Date();
                    updateListItem.set_item('DateOfDeletion', currDate);
                    updateListItem.update();
                }
                else  //couldn't find and subsequently run the workflow!
                {
                    listItemInfo += '\nID: ' + oListItem.get_id() + '\nTitle: ' + oListItem.get_item('Title') + '\nDocumentURL COULD NOT BE DELETED, need RMToolkit Deletion Workflow Feature to be enabled: ' + oListItem.get_item('DocumentURL').get_description();  
                }
            }
            else
            {
                listItemInfo += '\nID: ' + oListItem.get_id() + '\nTitle: ' + oListItem.get_item('Title') + '\nDocumentURL COULD NOT BE DELETED NOT APPROVED: ' + oListItem.get_item('DocumentURL').get_description();  
            }


        }

        alert(listItemInfo.toString());
       //must call async query again to update list items...
       contextUpdate.executeQueryAsync(Function.createDelegate(this, this.onUpdateQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));

   }

   function onUpdateQuerySucceeded(sender, args) {
       //do nothing the update is complete
       alert('list items have been updated');
   }


   function getWorkflowTemplateID(documentURL, wfName, webURLin) {
       // this function gets the ID of the workflow by name given the URL
       // uses web services call to get tempalate ID of workflow in given site     
       var workflowGUID = null;
       var itemURL = documentURL;
       var testVar;
       $().SPServices({
           operation: "GetTemplatesForItem",
           webURL: webURLin,
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

//   function getWorkflowTemplateIDAlt(documentURL, wfName) {
//       var workflowGUID = null;
//       var itemURL = documentURL;
//       var testVar;
//       $().SPServices({
//           operation: "GetTemplatesForItem",
//           webURL: "http://sptest/sites/rmtestwf/",
//           item: itemURL,
//           async: false,
//           completefunc: function (xData, Status) {
//               var out = $().SPServices.SPDebugXMLHttpResult({
//                   node: xData.responseXML
//               });
//               $("#WSOutput").html("").append("<b>This is the output from the workflow operation:</b>" + out);
//               workflowGUID = out;
//           }
//       });
//       return workflowGUID;

//   }

   function testSPServices() {
       $(document).ready(function () {
           $("#msgid").html("This is Hello World by JQuery");
       });

   waitMessage = "<table width='100%' align='center'><tr><td align='center'><img src='/_layouts/images/gears_an.gif'/></td></tr></table>";

$("#WSOutput").html(waitMessage).SPServices({
	operation: "GetUserInfo",
	userLoginName: "langford\\whitehart",
	completefunc: function (xData, Status) {
		$("#WSOutput").html("").append("<b>This is the output from the GetUserInfo operation:</b>");
		$(xData.responseXML).find("User").each(function() {
			$("#WSOutput").append("<li>ID: " + $(this).attr("ID") + "</li>");
			$("#WSOutput").append("<li>Sid: " + $(this).attr("Sid") + "</li>");
			$("#WSOutput").append("<li>Name: " + $(this).attr("Name") + "</li>");
			$("#WSOutput").append("<li>LoginName: " + $(this).attr("LoginName") + "</li>");
			$("#WSOutput").append("<li>Email: " + $(this).attr("Email") + "</li>");
			$("#WSOutput").append("<li>Notes: " + $(this).attr("Notes") + "</li>");
			$("#WSOutput").append("<li>IsSiteAdmin: " + $(this).attr("IsSiteAdmin") + "</li>");
			$("#WSOutput").append("<li>IsDomainGroup: " + $(this).attr("IsDomainGroup") + "</li>");
			$("#WSOutput").append("<hr/>");
		});
	}
});
   }



   function runWorkflowOnSelectedItem(documentURL,wfName,webURLin) {
       //  runs the workflow
       // would like to have error handling around runwf but haven't figured out how to do that yet..
       var templID = getWorkflowTemplateID(documentURL, wfName, webURLin);
       var bSuccess = false;
       
      
       //var templID=getWorkflowTemplateIDAlt(documentURL,wfName)
       if (templID != null) {
           {
               bSuccess = true;
               runWF(templID, documentURL, webURLin);
           }
       }
       return bSuccess;
   
   }

   //uses the web service function to start 
   function runWF(templID,documentURL, webURLin){
    $().SPServices({
    operation: "StartWorkflow",
    webURL: webURLin,
    item: documentURL,
    debug: true,
    templateId: templID,
    workflowParameters: "<root />",
    completefunc: function(xData, Status) {
      var out =  $().SPServices.SPDebugXMLHttpResult({node:  xData.responseXML});
    }
  });

   }

   function onQueryFailed(sender, args) {

       alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
   }

