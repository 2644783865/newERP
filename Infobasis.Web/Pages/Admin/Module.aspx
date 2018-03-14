<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Module.aspx.cs" Inherits="Infobasis.Web.Pages.Admin.Module" 
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <meta name="sourcefiles" content="~/Home.aspx" />
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="ModuleGrid" />
    <f:Grid ID="ModuleGrid" ShowBorder="true" ShowHeader="false" Title="模块列表" Width="800px" runat="server" EnableCollapse="true"
        DataKeyNames="ID" EnableTree="true" TreeColumn="Name" DataIDField="Id" DataParentIDField="ParentId">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                <Items>
                    <f:Button runat="server" ID="btnAdd" Text="新增" IconFont="Plus" OnClick="btnAdd_Click"></f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button runat="server" ID="btnEdit" Text="编辑" IconFont="Pencil" OnClick="btnEdit_Click"></f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button runat="server" ID="btnDel" Text="删除" IconFont="Remove" ConfirmText="确认删除？" ConfirmTarget="Top" OnClick="btnDel_Click"></f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>

        <Columns>
            <f:RowNumberField />
            <f:BoundField ColumnID="Name" ExpandUnusedSpace="true" DataField="Name" HeaderText="名称" />
            <f:BoundField Width="100px" DataField="Code" HeaderText="编号" />
            <f:BoundField Width="100px" DataField="Url" HeaderText="网页" />
            <f:BoundField Width="100px" DataField="Icon" HeaderText="图标" />
            <f:BoundField Width="100px" DataField="DisplayOrder" HeaderText="顺序" />
            <f:BoundField Width="150px" DataField="LastUpdateDatetime" DataFormatString="{0:yyyy-MM-dd}" HeaderText="修改日期" />
        </Columns>
    </f:Grid>

    <f:Window ID="Window1" IconUrl="~/res/images/16/10.png" runat="server" Hidden="true"
        WindowPosition="Center" IsModal="false" Title="Popup Window 1" EnableMaximize="true"
        EnableResize="true" Target="Self" EnableIFrame="true"
        Height="550px" Width="850px" OnClose="Window1_Close">
        <Items>

        </Items>
    </f:Window>

</asp:Content>
