<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleRole.aspx.cs" Inherits="Infobasis.Web.Pages.Admin.ModuleRole"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
    <f:RegionPanel ID="RegionPanel1" ShowBorder="false" BodyPadding="5px" runat="server">
        <Regions>
            <f:Region ID="Region1" ShowBorder="true" ShowHeader="false" Width="250px" Position="Left" Layout="Fit"
                EnableCollapse="true" RegionSplit="true" RegionSplitHeaderClass="false" runat="server">
                <Items>
                    <f:Grid ID="Grid1" runat="server" ShowBorder="false" ShowHeader="false" EnableCheckBoxSelect="false"
                        DataKeyNames="ID" AllowSorting="true" OnSort="Grid1_Sort" SortField="Name" SortDirection="DESC"
                        AllowPaging="false" EnableMultiSelect="false" 
                        OnRowSelect="Grid1_RowSelect" EnableRowSelectEvent="true" MouseWheelSelection="true">
                        <Columns>
                            <f:RowNumberField></f:RowNumberField>
                            <f:BoundField DataField="Name" SortField="Name" ExpandUnusedSpace="true" HeaderText="角色名称"></f:BoundField>
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Region>
            <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="Fit" runat="server" AutoScroll="true">
                <Items>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnUpdate" Icon="GroupEdit" runat="server" Text="更新当前角色的权限" OnClick="btnUpdate_Click">
                            </f:Button>
                            <f:Label ID="Label1" runat="server" Text="<<--修改后请注意保存！！" CssStyle="color:red;font-weight:bold;"></f:Label>
                        </Items>
                    </f:Toolbar>
                    <f:Tree ID="TreeModule" Width="550px" EnableCollapse="true" ShowHeader="true" Title="菜单列表" runat="server" AutoScroll="true" Expanded="true">
                        <Nodes>
                            <f:TreeNode Text="中国" EnableCheckBox="true" Expanded="true">
                                <f:TreeNode Text="河南省" EnableCheckBox="true" Expanded="true">
                                    <f:TreeNode Text="驻马店市" EnableCheckBox="true" NodeID="zhumadian">
                                        <f:TreeNode Text="遂平县" EnableCheckBox="true" NodeID="Suiping">
                                        </f:TreeNode>
                                        <f:TreeNode Text="西平县" EnableCheckBox="true" NodeID="Xiping">
                                        </f:TreeNode>
                                    </f:TreeNode>
                                    <f:TreeNode Text="漯河市" EnableCheckBox="true" NodeID="luohe" />
                                </f:TreeNode>
                                <f:TreeNode EnableCheckBox="true" Text="安徽省" Expanded="true"
                                    NodeID="Anhui">
                                    <f:TreeNode EnableCheckBox="true" Text="合肥市" NodeID="Hefei">
                                    </f:TreeNode>
                                    <f:TreeNode EnableCheckBox="true" Text="黄山市" NodeID="Huangshan">
                                    </f:TreeNode>
                                </f:TreeNode>
                            </f:TreeNode>
                        </Nodes>
                        <Listeners>
                            <f:Listener Event="nodecheck" Handler="onTreeNodeCheck" />
                        </Listeners>
                    </f:Tree>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    <f:Menu ID="Menu1" runat="server">
        <f:MenuButton ID="menuSelectRows" EnablePostBack="false" runat="server" Text="全选行">
            <Listeners>
                <f:Listener Event="click" Handler="onSelectRowsClick" />
            </Listeners>
        </f:MenuButton>
        <f:MenuButton ID="menuUnselectRows" EnablePostBack="false" runat="server" Text="取消行">
            <Listeners>
                <f:Listener Event="click" Handler="onUnselectRowsClick" />
            </Listeners>
        </f:MenuButton>
    </f:Menu>
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script>

        var menuID = '<%= Menu1.ClientID %>';
        var menuSelectRows = '<%= menuSelectRows.ClientID %>';
        var menuUnselectRows = '<%= menuUnselectRows.ClientID %>';

        function onTreeNodeCheck(event, nodeId, checked) {
            if (checked) {
                // 第二个参数true：递归更新全部子节点
                this.checkNode(nodeId, true);
            } else {
                this.uncheckNode(nodeId, true);
            }
        }

        function selectCheckbox(checked) {

            selectedRows.find('.powers input[type=checkbox]').prop('checked', checked);
        }



        function onGridBeforeRowContextMenu(event) {
            F(menuID).show();
            return false; // 阻止弹出系统菜单
        }

        function onSelectRowsClick(event) {
            selectCheckbox(true);
        }

        function onUnselectRowsClick(event) {
            selectCheckbox(false);
        }

    </script>
</asp:Content>