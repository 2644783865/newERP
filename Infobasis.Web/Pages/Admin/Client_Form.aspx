<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Client_Form.aspx.cs" Inherits="Infobasis.Web.Pages.Admin.Client_Form"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" />
    <f:Panel ID="Panel3" ShowBorder="false" ShowHeader="false" runat="server"
        BodyPadding="10px" AutoScroll="true">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                        Text="关闭">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnSaveClose" ValidateForms="Form1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                        runat="server" Text="保存后关闭">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
        <f:Form ID="Form1" LabelAlign="Right" MessageTarget="Qtip" RedStarPosition="BeforeText" LabelWidth="90px"
                    BodyPadding="5px" ShowBorder="true" ShowHeader="false" runat="server" AutoScroll="true">
            <Items>
                <f:Panel ID="FormPanel1" runat="server" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column">
                    <Items>
                        <f:TextBox ID="tbxName" ShowLabel="true" Label="公司名称" Required="true" ShowRedStar="True" Width="260px" CssClass="marginr" runat="server">
                        </f:TextBox>
                        <f:TextBox ID="tbxCompanyCode" ShowLabel="true" Label="公司代号" Required="true" ShowRedStar="True"  Width="200px" CssClass="marginr" runat="server">
                        </f:TextBox>
                        <f:Button ID="btnCheckDuplicate" Text="检查是否可用" CssClass="marginr" runat="server" OnClick="btnCheckDuplicate_Click">
                        </f:Button>
                    </Items>
                </f:Panel>
                <f:Panel ID="FormPanel2" runat="server" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column">
                    <Items>
                        <f:TextBox ID="tbxClientAdminAccount" ShowLabel="true" Label="管理员账号" Required="true" Width="260px" ShowRedStar="True" CssClass="marginr" runat="server">
                        </f:TextBox>
                        <f:TextBox ID="tbxClientAdminAccountPwd" ShowLabel="true" Label="管理员密码" Required="true" Width="260px" ShowRedStar="True" CssClass="marginr" runat="server" TextMode="Password">
                        </f:TextBox>
                    </Items>
                </f:Panel>
                <f:Panel ID="FormPanel3" runat="server" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column">
                    <Items>
                        <f:NumberBox Label="允许最大用户数" ID="tbxMaxUsers" runat="server" MaxValue="900" MinValue="0" Width="260px"
                            NoDecimal="true" NoNegative="True" Required="True" EmptyText="比如 200" ShowRedStar="True" Text="200" />
                        <f:DatePicker ID="tbxExpiredDatetime" ShowLabel="true" Label="到期日期" Required="false" CssClass="marginr" EnableEdit="false" Width="260px" runat="server">
                        </f:DatePicker>
                    </Items>
                </f:Panel>
                <f:Panel ID="FormPanel4" runat="server" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column">
                    <Items>
                        <f:TextArea ID="tbxRemark" runat="server" Label="备注">
                        </f:TextArea>
                    </Items>
                </f:Panel>
            </Items>
        </f:Form>
        </Items>
</f:Panel>
<f:Window ID="Window1" Title="编辑" Hidden="true" EnableIFrame="true" runat="server"
    EnableMaximize="true" EnableResize="true" Target="Top" IsModal="True" Width="550px"
    Height="350px">
</f:Window>

</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>