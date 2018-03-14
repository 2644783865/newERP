<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Job_Form.aspx.cs" Inherits="Infobasis.Web.Pages.HR.Job_Form"
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server"
        AutoScroll="true" BodyPadding="10px">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                        Text="关闭">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
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
                    <f:NumberBox ID="tbxLevel" runat="server" Label="级别"></f:NumberBox>
                    <f:NumberBox ID="tbxDisplayOrder" runat="server" Label="排序"></f:NumberBox>
                    <f:TextArea ID="tbxRemark" runat="server" Label="备注">
                    </f:TextArea>
                    <f:CheckBox ID="cbxEnabled" runat="server" Checked="true" Label="是否启用">
                    </f:CheckBox>
                </Items>
            </f:SimpleForm>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>