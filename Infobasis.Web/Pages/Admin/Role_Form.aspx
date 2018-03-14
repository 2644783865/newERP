<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Role_Form.aspx.cs" Inherits="Infobasis.Web.Pages.Admin.Role_Form"
    MasterPageFile="~/PageMaster/Popup.master" %>


<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server"
        BodyPadding="10px" AutoScroll="true">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                        Text="关闭">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose"
                        OnClick="btnSaveClose_Click" runat="server" Text="保存后关闭">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server"
                Title="SimpleForm" EnableTableStyle="true" MessageTarget="Qtip">
                <Items>
                    <f:TextBox ID="tbxName" runat="server" Label="名称" Required="true" ShowRedStar="true">
                    </f:TextBox>
                    <f:CheckBox ID="tbxIsActive" runat="server" Label="启用" ShowRedStar="false" Checked="true">
                    </f:CheckBox>
                    <f:TextArea ID="tbxRemark" Label="备注" runat="server">
                    </f:TextArea>
                    <f:NumberBox ID="tbxDisplayOrder" Label="排序" runat="server"></f:NumberBox>
                </Items>
            </f:SimpleForm>
        </Items>
    </f:Panel>
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>
