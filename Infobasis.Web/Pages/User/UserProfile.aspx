<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="Infobasis.Web.Pages.User.UserProfile"
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">

    <f:PageManager ID="PageManager1" runat="server" />
    <f:Panel ID="PanelMain" ShowBorder="false" ShowHeader="false" runat="server"
        BodyPadding="10px" AutoScroll="true">
        <Items>
            <f:Form ID="Form1" LabelAlign="Right" MessageTarget="Qtip" RedStarPosition="BeforeText" LabelWidth="90px"
                BodyPadding="5px" ShowBorder="false" ShowHeader="false" runat="server" AutoScroll="true">
                <Items>
                    <f:Panel ID="Panel2" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox" BoxConfigAlign="StretchMax">
                        <Items>
                            <f:Panel ID="Panel1" Title="面板1" BoxFlex="5" MarginRight="5px" runat="server" ShowBorder="false" ShowHeader="false">
                                <Items>
                                    <f:Label ID="tbxName" Label="姓名" runat="server" LabelAlign="Right"></f:Label>
                                    <f:Label ID="ddlSex" Label="性别" runat="server">
                                    </f:Label>
                                    <f:Label ID="dpBirthDay" Label="出生日期" runat="server"></f:Label>
                                    <f:Label ID="tbxAge" runat="server" Label="年龄"></f:Label>
                                    <f:Label ID="ddlMarriage" Label="婚姻状况" runat="server">
                                    </f:Label>
                                    <f:Label ID="ddlNation" Label="民族" runat="server">
                                    </f:Label>
                                </Items>
                            </f:Panel>
                            <f:Panel ID="Panel4" runat="server" BoxFlex="3" ShowBorder="false" ShowHeader="false" MarginRight="5px" Layout="VBox">
                                <Items>
                                    <f:Label ID="tbxArchivesNum" runat="server" Label="档案号"></f:Label>
                                    <f:Label ID="tbxUserName" runat="server" Label="用户名"></f:Label>
                                    <f:Label ID="tbxRemark" Label="备注" runat="server" BoxFlex="1"></f:Label>
                                </Items>
                            </f:Panel>
                            <f:Panel ID="Panel5" Title="面板1" BoxFlex="2" runat="server" ShowBorder="false" ShowHeader="false" Layout="VBox">
                                <Items>
                                    <f:FileUpload ID="userPortraitUpload" CssClass="uploadbutton" runat="server" ButtonText="上传头像" ButtonOnly="true" AutoPostBack="true" OnFileSelected="userPortraitUpload_FileSelected"></f:FileUpload>
                                    <f:Image ID="userPortrait" CssClass="userphoto" ImageUrl="~/res/images/blank_180.png" runat="server" BoxFlex="1">
                                    </f:Image>
                                </Items>
                            </f:Panel>
                        </Items>
                    </f:Panel>
                    <f:Form ID="Form7" ShowBorder="false" ShowHeader="false" runat="server">
                        <Rows>
                            <f:FormRow ColumnWidths="30% 70%">
                                <Items>
                                    <f:Label runat="server" Text=""></f:Label>
                                    <f:Button ID="btnChangePassword" Text="修改密码" runat="server"></f:Button>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:Label ID="ddlCertificates" Label="证件类别" runat="server">
                                    </f:Label>
                                    <f:Label ID="tbxIDCardEdit" runat="server" Label="证件号"></f:Label>
                                </Items>
                            </f:FormRow>
                            <f:FormRow ColumnWidths="50% 50%">
                                <Items>
                                    <f:Label ID="tbxCustomerEmail" runat="server" Label="邮箱"></f:Label>
                                    <f:Label ID="tbxCustomerTel" runat="server"></f:Label>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:Label ID="ddlEducation" Label="文化程度" runat="server">
                                    </f:Label>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:Label ID="tbxNativePlace" runat="server" Label="籍贯"></f:Label>
                                    <f:Label ID="ddlUserState" Label="状态" runat="server">
                                    </f:Label>
                                </Items>
                            </f:FormRow>
                        </Rows>
                    </f:Form>
                </Items>
            </f:Form>
        </Items>
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server" ToolbarAlign="Right" Position="Bottom">
                <Items>
                    <f:Button ID="Button1" IconFont="Save" Text="保存信息" ValidateForms="Form1" ValidateMessageBox="false" runat="server">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
    </f:Panel>

</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>