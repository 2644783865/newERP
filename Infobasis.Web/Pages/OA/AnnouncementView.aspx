<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnnouncementView.aspx.cs" Inherits="Infobasis.Web.Pages.OA.AnnouncementView"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" />
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
                            <f:Label ID="labTitle" runat="server" Label="公告主题">
                            </f:Label>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow2" runat="server" >                
                        <Items>                                            
                            <f:Label ID="labNote" runat="server" Label="内容" EncodeText="false" Height="500px"></f:Label>
                       
                        </Items>
                    </f:FormRow>
                    <f:FormRow runat="server">
                        <Items>
                            <f:Label ID="labPublisher" runat="server" Label="发布人"></f:Label>
                            <f:Label ID="labPublishDate" runat="server" Label="发布时间"></f:Label>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>

</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>