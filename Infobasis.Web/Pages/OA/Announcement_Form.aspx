<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Announcement_Form.aspx.cs" Inherits="Infobasis.Web.Pages.OA.Announcement_Form"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script>
        window.UEDITOR_HOME_URL = './';
    </script>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" />
    <f:Toolbar ID="Toolbar2" runat="server" Layout="HBox">
        <Items>     
            <f:LinkButton ID="LinkButton1" runat="server" Text="返回" OnClientClick="window.open('/Pages/OA/Announcement2.aspx','_self');"></f:LinkButton>
            <f:Button ID="Button1" IconFont="Save" MarginLeft="20px" Text="保存信息" ValidateForms="SimpleForm1" ValidateMessageBox="false" OnClick="btnSave_Click" runat="server">
            </f:Button>
            <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" MarginLeft="20px" Icon="SystemSaveClose"
                OnClick="btnSaveClose_Click" runat="server" Text="保存后关闭">
            </f:Button>
            <f:HiddenField runat="server" ID="tbxAnnounceID"></f:HiddenField>
        </Items>                
    </f:Toolbar>
    <f:SimpleForm runat="server" ID="SimpleForm1" ShowBorder="false" ShowHeader="true">
        <Items>
            <f:Panel ID="Panel5" runat="server" Width="950px" ShowBorder="false" EnableCollapse="true"
                Layout="HBox" AutoScroll="true" BodyPadding="5"
                ShowHeader="false" Title="公告通知">
                <Items>
                    <f:Panel ID="Panel1" Title="面板1" BoxFlex="1" runat="server" MarginRight="5px"
                        BodyPadding="5px" ShowBorder="true" ShowHeader="false" Layout="VBox">
                        <Items>
                            <f:TextBox ID="tbxTitle" runat="server" Label="公告标题" ShowRedStar="true" Required="true" NextClickControl="tbxPublishDate"></f:TextBox>
                            <f:DatePicker ID="tbxPublishDate" runat="server" Label="发布时间" ShowTime="true" EnableEdit="false" NextClickControl="tbxEndDate"></f:DatePicker>
                            <f:DatePicker ID="tbxEndDate" runat="server" Label="结束时间" ShowTime="true" EnableEdit="false" NextClickControl="DropDownAnnounceType"></f:DatePicker>
                            <f:HtmlEditor runat="server" Label="内容" ID="tbxContentHtml" MarginTop="20px" Editor="UEditor" BoxFlex="1" BasePath="~/third-party/res/ueditor/" ToolbarSet="Full" Height="400px">
                            </f:HtmlEditor>                            
                            <f:Panel ID="Panel2" runat="server" Layout="Column" ShowHeader="false" ShowBorder="false">
                                <Items>
                                    <f:DropDownList ID="DropDownAnnounceType" Label="公告类型" ShowRedStar="true" Required="true" runat="server" NextClickControl="tbxContent"
                                            ColumnWidth="50%"  Margin="0 5 0 0">
                                        <f:ListItem Text="装修/保养常识" Value="006" />
                                        <f:ListItem Text="客户通知" Value="005" />
                                        <f:ListItem Text="知识共享" Value="004" />
                                        <f:ListItem Text="规章制度" Value="003" />
                                        <f:ListItem Text="公司通知" Value="002" Selected="true" />
                                        <f:ListItem Text="公司新闻" Value="001" />
                                        <f:ListItem Text="活动咨询" Value="007" />
                                        <f:ListItem Text="材料商活动" Value="008" />
                                    </f:DropDownList>
                                    <f:Label runat="server" ID="tbxPublisherDisplayName" ColumnWidth="50%" Label="主持人"></f:Label>
                                    </Items>
                            </f:Panel>
                        </Items>
                    </f:Panel>
                </Items>
            </f:Panel>
        </Items>
    </f:SimpleForm>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>