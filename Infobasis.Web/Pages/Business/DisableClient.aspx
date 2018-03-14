<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisableClient.aspx.cs" Inherits="Infobasis.Web.Pages.Business.DisableClient"
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" />
    <f:Panel ID="PanelInfo1" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox">
        <Items>
            <f:Label runat="server" ID="labNo" Label="客户编号" BoxFlex="1" LabelAlign="Right"></f:Label>
            <f:Label runat="server" ID="labName" Label="客户名称" BoxFlex="1" LabelAlign="Right"></f:Label>
            <f:Label runat="server" ID="labTel" Label="联系电话" BoxFlex="1" LabelAlign="Right"></f:Label>
        </Items>
    </f:Panel>
    <f:Panel ID="PanelInfo2" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox">
        <items>
            <f:Label runat="server" ID="labHousesName" Label="楼盘" BoxFlex="1" LabelAlign="Right"></f:Label>
            <f:Label runat="server" ID="labAddress" Label="地址" BoxFlex="1" LabelAlign="Right"></f:Label>
        </items>
    </f:Panel>

    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server"
        BodyPadding="10px" AutoScroll="true" MarginTop="10px">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                        Text="关闭">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                        runat="server" Text="保存后关闭">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server"
                Title="SimpleForm" EnableTableStyle="true" MessageTarget="Qtip">
                <Rows>
                    <f:FormRow ID="FormRow7" runat="server">
                        <Items>
                            <f:DropDownList runat="server" Label="作废原因" ID="disableReason">
                                <f:ListItem Text="已选择其他公司" Value="已选择其他公司" />
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow10" runat="server">
                        <Items>
                            <f:TextArea ID="tbxRemark" runat="server" Label="备注">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>