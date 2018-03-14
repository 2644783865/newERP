<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Changepassword.aspx.cs" Inherits="Infobasis.Web.Pages.Admin.User_Changepassword"
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server" AutoScroll="true" BodyPadding="10px">
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
                    <f:Label ID="labUserName" Label="用户名" runat="server">
                    </f:Label>
                    <f:Label ID="labUserRealName" Label="中文名" runat="server">
                    </f:Label>
                    <f:TextBox ID="tbxPassword" runat="server" Label="新密码" Required="true" ShowRedStar="true"
                        TextMode="Password">
                    </f:TextBox>
                    <f:TextBox ID="tbxConfirmPassword" runat="server" Label="确认密码" Required="true"
                        ShowRedStar="true" TextMode="Password" CompareControl="tbxPassword" CompareOperator="Equal">
                    </f:TextBox>
                </Items>
            </f:SimpleForm>
        </Items>
    </f:Panel>
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>
