<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>

<%@ Page Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="RMToolkitConfigPage.aspx.cs"
    Inherits="RMToolkitConfig.RMTookitConfigPage" 
    MasterPageFile="~/_admin/admin.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
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
                            id="btnSubmitTop" ToolTip="Save current settings"
                            accesskey="<%$Resources:wss,okbutton_accesskey%>"
                            Enabled="true"/>
                        <asp:Button UseSubmitBehavior="false" runat="server"
                            class="ms-ButtonHeightWidth"
                            Text="<%$Resources:wss,multipages_cancelbutton_text%>"
                            CausesValidation="False"
                            id="btnCancelTop"
                            CommandName="Cancel"
                            accesskey="<%$Resources:wss,multipages_cancelbutton_accesskey%>"
                            Enabled="true"/>
                    </Template_Buttons>
                </wssuc:buttonsection>
                  <!-- *************************************** -->
                    <!-- ***************************************
                    display an input form section -->
                    <wssuc:inputformsection runat="server" title="RMToolkit Site" description="RM Toolkit Site URL">
                    <Template_InputFormControls>
  			            <wssuc:InputFormControl runat="server"
				            LabelText="Enter List URL">
				            <Template_Control>
					            <wssawc:InputFormTextBox Title="Configuration Site URL" class="ms-input" ID="txtRMToolkitSiteURL" Columns="75" Runat="server" MaxLength=255 EnableViewState="true" />
					            <wssawc:InputFormRequiredFieldValidator
						            ID="InputFormRequiredFieldValidator1"
						            ControlToValidate="txtRMToolkitSiteURL"
						            ErrorMessage="You must enter a valid URL!"
						            Runat="server"/>
				            </Template_Control>
			            </wssuc:InputFormControl>
                    </Template_InputFormControls>
                </wssuc:inputformsection>
              
                  <!-- *************************************** -->
                    <!-- ***************************************
                    display an input form section -->
                    <wssuc:inputformsection runat="server" title="RMToolkit Archive Certificate Library Name" description="RM Toolkit Archive Certificate Library Name">
                    <Template_InputFormControls>
  			            <wssuc:InputFormControl runat="server"
				            LabelText="Enter Library Name">
				            <Template_Control>
					            <wssawc:InputFormTextBox Title="Library Name" class="ms-input" ID="txtRMToolkitArchiveCertificateLibraryName" Columns="75" Runat="server" MaxLength=255 EnableViewState="true" />
					            <wssawc:InputFormRequiredFieldValidator
						            ID="ReqValtxtRMToolkitArchiveCertificateLibraryName"
						            ControlToValidate="txtRMToolkitArchiveCertificateLibraryName"
						            ErrorMessage="You must enter a valid URL!"
						            Runat="server"/>
				            </Template_Control>
			            </wssuc:InputFormControl>
                    </Template_InputFormControls>
                </wssuc:inputformsection>
                
                  <!-- *************************************** -->
                    <!-- ***************************************
                    display an input form section -->
                    <wssuc:inputformsection runat="server" title="RMToolkit Deletion Certificate Library Name" description="RM Toolkit Deletion Certificate Library Name">
                    <Template_InputFormControls>
  			            <wssuc:InputFormControl runat="server"
				            LabelText="Enter Library Name">
				            <Template_Control>
					            <wssawc:InputFormTextBox Title="Library Name" class="ms-input" ID="txtRMToolkitDeleteCertificateLibraryName" Columns="75" Runat="server" MaxLength=255 EnableViewState="true" />
					            <wssawc:InputFormRequiredFieldValidator
						            ID="ReqVatxtRMToolkitDeleteCertificateLibraryName"
						            ControlToValidate="txtRMToolkitDeleteCertificateLibraryName"
						            ErrorMessage="You must enter a valid URL!"
						            Runat="server"/>
				            </Template_Control>
			            </wssuc:InputFormControl>
                    </Template_InputFormControls>
                </wssuc:inputformsection>

                 <!-- *************************************** -->
                    <!-- ***************************************
                    display an input form section -->
                    <wssuc:inputformsection runat="server" title="Global Metadata Column 1" description="Name of first global metadata column">
                    <Template_InputFormControls>
  			            <wssuc:InputFormControl runat="server"
				            LabelText="Enter Column Name">
				            <Template_Control>
					            <wssawc:InputFormTextBox Title="Column Name" class="ms-input" ID="txtRMToolkitGlbColumn1" Columns="75" Runat="server" MaxLength=255 EnableViewState="true" />
				            </Template_Control>
			            </wssuc:InputFormControl>
                    </Template_InputFormControls>
                </wssuc:inputformsection>

                
                 <!-- *************************************** -->
                    <!-- ***************************************
                    display an input form section -->
                    <wssuc:inputformsection runat="server" title="Global Metadata Column 2" description="Name of second global metadata column">
                    <Template_InputFormControls>
  			            <wssuc:InputFormControl runat="server"
				            LabelText="Enter Column Name">
				            <Template_Control>
					            <wssawc:InputFormTextBox Title="Column Name" class="ms-input" ID="txtRMToolkitGlbColumn2" Columns="75" Runat="server" MaxLength=255 EnableViewState="true" />
				            </Template_Control>
			            </wssuc:InputFormControl>
                    </Template_InputFormControls>
                </wssuc:inputformsection>

                
                 <!-- *************************************** -->
                    <!-- ***************************************
                    display an input form section -->
                    <wssuc:inputformsection runat="server" title="Global Metadata Column 3" description="Name of third global metadata column">
                    <Template_InputFormControls>
  			            <wssuc:InputFormControl runat="server"
				            LabelText="Enter Column Name">
				            <Template_Control>
					            <wssawc:InputFormTextBox Title="Column Name" class="ms-input" ID="txtRMToolkitGlbColumn3" Columns="75" Runat="server" MaxLength=255 EnableViewState="true" />
				            </Template_Control>
			            </wssuc:InputFormControl>
                    </Template_InputFormControls>
                </wssuc:inputformsection>

                    <!-- ************************************** -->


                </td>
            </tr>
        </table>
    </asp:Panel>
    <!-- Literal Control to display messages -->
    <div style="font-size: 12pt; color: red; font-weight: bold;">
        <asp:Literal ID="litMessages" runat="server" />
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    RM Toolkit Configuration Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    RM Toolkit Configuration Page Version 1.2.0.0
</asp:Content>

<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
    Manage configuration settings.
</asp:Content>

