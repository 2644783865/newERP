<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Module_Add.aspx.cs" Inherits="Infobasis.Web.Pages.Admin.Module_Add" 
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="SimpleForm1" runat="server" />
    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false"
        AutoScroll="true" BodyPadding="10px" runat="server">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server" Icon="SystemClose">
                    </f:Button>
                    <f:Button ID="btnSaveContinue" Text="回发父页面" runat="server" Icon="SystemSave"
                        OnClick="btnSaveContinue_Click">
                    </f:Button>
                    <f:Button ID="btnSaveRefresh" Text="刷新父页面" runat="server" Icon="SystemSaveNew"
                        OnClick="btnSaveRefresh_Click">
                    </f:Button>
                    <f:Button ID="Button1" Text="关闭当前选项卡" runat="server" Icon="SystemSaveClose"
                        OnClick="btnSaveClose_Click">
                    </f:Button>
                    <f:ToolbarFill ID="ToolbarFill1" runat="server">
                    </f:ToolbarFill>
                    <f:ToolbarText ID="ToolbarText1" Text="提示一" runat="server">
                    </f:ToolbarText>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:ToolbarText ID="ToolbarText2" Text="提示二" runat="server">
                    </f:ToolbarText>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Rows>
            <f:FormRow>
                <Items>
                    <f:Label ID="labUserName" Label="申请人" Text="三生石上" CssClass="highlight" runat="server" />
                    <f:Label ID="Label3" Label="电话" Text="0551-1234567" runat="server" />
                </Items>
            </f:FormRow>
            <f:FormRow>
                <Items>
                    <f:NumberBox ID="NumberBox1" NoDecimal="true" NoNegative="true" Label="数量" Required="true" ShowRedStar="true" runat="server" />
                    <f:DatePicker ID="DatePicker1" Required="True" ShowRedStar="true" runat="server"
                        SelectedDate="2008-05-09" Label="申请日期" Text="2008-05-09">
                    </f:DatePicker>
                </Items>
            </f:FormRow>
            <f:FormRow>
                <Items>
                    <f:TextArea ID="TextArea2" Height="80px" Label="描述" runat="server" Required="True" ShowRedStar="true" />
                </Items>
            </f:FormRow>
            <f:FormRow>
                <Items>
                    <f:HtmlEditor ID="HtmlEditor1" Editor="UMEditor" BasePath="~/third-party/res/umeditor/"
                        Label="详细描述" Height="200px" runat="server">
                    </f:HtmlEditor>
                </Items>
            </f:FormRow>
        </Rows>
    </f:Form>
</asp:Content>