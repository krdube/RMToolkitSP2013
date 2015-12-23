<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="MetadataColumns.aspx.cs" 
    Inherits="RMToolkitSiteMetadata.Layouts.RMToolkitSiteMetadata.MetadataColumns" 
    DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<style type="text/css">
body #s4-leftpanel {
    display:none;
}
.s4-ca {
    margin-left:0px;
}
</style>

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<asp:Panel ID="inputControls" runat="server">
        <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <tr>
                <td width="50%">
                    <!-- **************************************
                     use sharepoint buttonsection control
                     to display the "ok" and "cancel" buttons -->
                    <wssuc:buttonsection runat="server" topbuttons="true" bottomspacing="5" showsectionline="false"
                        showstandardcancelbutton="false">
                    <Template_Buttons>
                        <asp:Button UseSubmitBehavior="false" runat="server"
                            class="ms-ButtonHeightWidth"
                            Text="<%$Resources:wss,multipages_okbutton_text%>"
                            id="btnOK" OnClick="Save" ToolTip="Save current settings"
                            accesskey="<%$Resources:wss,okbutton_accesskey%>"
                            Enabled="true"/>
                        <asp:Button UseSubmitBehavior="false" runat="server"
                            class="ms-ButtonHeightWidth"
                            Text="<%$Resources:wss,multipages_cancelbutton_text%>"
                            CausesValidation="False"
                            id="btnCancel" OnClick="Cancel"
                            CommandName="Cancel"
                            accesskey="<%$Resources:wss,multipages_cancelbutton_accesskey%>"
                            Enabled="true"/>
                    </Template_Buttons>
                </wssuc:buttonsection>
                  <!-- *************************************** -->
                    <!-- ***************************************
                    display an input form section -->
                    <wssuc:inputformsection runat="server" title="Site Specific Metadata" description="Column 1 Name">
                    <Template_InputFormControls>
  			            <wssuc:InputFormControl runat="server"
				            LabelText="Enter Column 1 Name">
				            <Template_Control>
					            <wssawc:InputFormTextBox Title="Column1" class="ms-input" ID="txtColumn1" Columns="75" Runat="server" MaxLength=255 EnableViewState="true" />
				            </Template_Control>
			            </wssuc:InputFormControl>
                    </Template_InputFormControls>
                </wssuc:inputformsection>
              
                  <!-- *************************************** -->
                    <!-- ***************************************
                    display an input form section -->
                    <wssuc:inputformsection runat="server" title="Site Specific Metadata" description="Column 2 Name">
                    <Template_InputFormControls>
  			            <wssuc:InputFormControl runat="server"
				            LabelText="Enter Column 2 Name">
				            <Template_Control>
					            <wssawc:InputFormTextBox Title="Column2" class="ms-input" ID="txtColumn2" Columns="75" Runat="server" MaxLength=255 EnableViewState="true" />
				            </Template_Control>
			            </wssuc:InputFormControl>
                    </Template_InputFormControls>
                </wssuc:inputformsection>
                
                   <!-- *************************************** -->
                    <!-- ***************************************
                    display an input form section -->
                    <wssuc:inputformsection runat="server" title="Site Specific Metadata" description="Column 3 Name">
                    <Template_InputFormControls>
  			            <wssuc:InputFormControl runat="server"
				            LabelText="Enter Column 3 Name">
				            <Template_Control>
					            <wssawc:InputFormTextBox Title="Column3" class="ms-input" ID="txtColumn3" Columns="75" Runat="server" MaxLength=255 EnableViewState="true" />
				            </Template_Control>
			            </wssuc:InputFormControl>
                    </Template_InputFormControls>
                </wssuc:inputformsection>

                


                </td>
            </tr>
        </table>
    </asp:Panel>
    <!-- Literal Control to display messages -->
    <div style="font-size: 12pt; color: red; font-weight: bold;">
        <asp:Literal ID="litMessages" runat="server" />
    </div>




    <%-- <br /><br />
    <h2>RMToolkit Site Specific Metadata Columns Configuration</h2>
    <table>
    <tr>
        <td>Site Column 1</td>
        <td><asp:TextBox ID="txtColumn1" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Site Column 2</td>
        <td><asp:TextBox ID="txtColumn2" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td>Site Column 3</td>
        <td><asp:TextBox ID="txtColumn3" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Button ID="btnOK" runat="server" Text="OK" OnClick="Save" />  <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="Cancel" /></td>
        <td></td>
    </tr>
    </table>--%>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
RMToolkit
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
RMToolkit Site Metadata Columns
</asp:Content>
