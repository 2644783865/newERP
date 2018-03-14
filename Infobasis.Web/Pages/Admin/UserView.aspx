<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserView.aspx.cs" Inherits="Infobasis.Web.Pages.Admin.UserView"
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
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server"
                Title="SimpleForm" LabelAlign="Left" EnableTableStyle="true">
                <Rows>
                    <f:FormRow ID="FormRow1" runat="server">
                        <Items>
                            <f:Label ID="labName" runat="server" Label="用户名">
                            </f:Label>
                            <f:Label ID="labRealName" runat="server" Label="中文名">
                            </f:Label>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow2" runat="server">
                        <Items>
                            <f:Label ID="labEmail" runat="server" Label="个人邮箱">
                            </f:Label>
                            <f:Label ID="labEnabled" runat="server" Label="是否启用">
                            </f:Label>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow6" runat="server">
                        <Items>
                            <f:Label ID="labRole" runat="server" Label="所属角色">
                            </f:Label>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow9" runat="server">
                        <Items>
                            <f:Label ID="labRemark" runat="server" Label="备注">
                            </f:Label>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>

</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>