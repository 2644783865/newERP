<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Announcement2.aspx.cs" Inherits="Infobasis.Web.Pages.OA.Announcement2"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" Title="公告通知" runat="server" 
        EnableCollapse="true" DataKeyNames="Id" AllowSorting="true" OnSort="Grid1_Sort"
        OnPageIndexChange="Grid1_PageIndexChange">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnNew" OnClientClick="window.open('/Pages/OA/Announcement_Form.aspx','_self');" runat="server" Icon="Add" EnablePostBack="false" Text="新增公告通知">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <PageItems>
            <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
            </f:ToolbarSeparator>
            <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
            </f:ToolbarText>
            <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                runat="server">
                <f:ListItem Text="10" Value="10" />
                <f:ListItem Text="20" Value="20" />
                <f:ListItem Text="50" Value="50" />
                <f:ListItem Text="100" Value="100" />
            </f:DropDownList>
        </PageItems>
        <Columns>
            <f:RowNumberField />
            <f:BoundField Width="100px" DataField="AnnounceTypeName" DataFormatString="{0}" HeaderText="栏目" />
            <f:WindowField DataTextField="Title" SortField="Title" Width="260px" HeaderText="标题" EnableLock="false" Locked="false"
                WindowID="Window1" DataIFrameUrlFields="ID" DataWindowTitleField="Title" DataIFrameUrlFormatString="~/Pages/OA/AnnouncementView.aspx?id={0}" />
            <f:BoundField Width="120px" DataField="PublishDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="发布时间" />
            <f:BoundField Width="120px" DataField="EndDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="结束时间" />
            <f:BoundField Width="120px" DataField="Publisher" HeaderText="发布人" />
            <f:BoundField Width="100px" DataField="ReadNum" HeaderText="阅览人数" />
        </Columns>
    </f:Grid>
    <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
        EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="800px"
        Height="550px" OnClose="Window1_Close">
    </f:Window>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>
