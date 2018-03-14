<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Client.aspx.cs" Inherits="Infobasis.Web.Pages.Admin.Client"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">

        .f-grid-cell[data-color=Expired-Company] {
            background-color: #b200ff;
            color: #fff;
        }

        .f-grid-cell[data-color=Disabled-Company] {
            background-color: #0026ff;
            color: #fff;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
        ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
        ShowHeader="false" Title="客户管理">
        <Items>
            <f:Form ID="Form2" runat="server" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                <Rows>
                    <f:FormRow ID="FormRow1" runat="server">
                        <Items>
                            <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在客户名称中搜索"
                                Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                            </f:TwinTriggerBox>
                            <f:RadioButtonList ID="rblEnableStatus" AutoPostBack="true" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged"
                                Label="客户状态" ColumnNumber="4" runat="server">
                                <f:RadioItem Text="全部" Selected="true" Value="all" />
                                <f:RadioItem Text="启用" Value="enabled" />
                                <f:RadioItem Text="禁用" Value="disabled" />
                                <f:RadioItem Text="到期" Value="expired" />
                            </f:RadioButtonList>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
            <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                EnableCheckBoxSelect="true"
                DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="Name"
                SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true" OnPreDataBound="Grid1_PreDataBound" 
                OnRowCommand="Grid1_RowCommand" OnRowDataBound="Grid1_RowDataBound" OnPageIndexChange="Grid1_PageIndexChange"
                MouseWheelSelection="true" QuickPaging="true">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnDeleteSelected" Icon="Delete" runat="server" Text="删除选中记录" OnClick="btnDeleteSelected_Click" Hidden="true">
                            </f:Button>
                            <f:ToolbarFill ID="ToolbarFill1" runat="server">
                            </f:ToolbarFill>
                            <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增客户">
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
                    <f:RowNumberField Width="35px" EnablePagingNumber="true" />
                    <f:BoundField DataField="Name" SortField="Name" Width="120px" HeaderText="客户简称" />
                    <f:BoundField DataField="CompanyCode" SortField="CompanyCode" Width="80px" HeaderText="客户代号" />
                    <f:RenderField Width="60px" ColumnID="CompanyStatus"  SortField="CompanyStatus" DataField="CompanyStatus"
                        RendererFunction="renderCompanyStatus"  HeaderText="状态">
                    </f:RenderField>
                    <f:RenderField Width="100px" ColumnID="CreateDatetime" DataField="CreateDatetime" FieldType="Date"
                        Renderer="Date" RendererArgument="yyyy-MM-dd" HeaderText="创建日期" Hidden="false">
                    </f:RenderField>
                    <f:RenderField Width="100px" ColumnID="ExpiredDatetime" DataField="ExpiredDatetime" FieldType="Date"
                        Renderer="Date" RendererArgument="yyyy-MM-dd" HeaderText="到期日期" Hidden="false">
                    </f:RenderField>
                    <f:RenderField Width="80px" ColumnID="MaxUsers" DataField="MaxUsers" FieldType="Int"
                        HeaderText="允许最大用户人数">
                    </f:RenderField>
                    <f:BoundField DataField="ClientAdminAccount" SortField="ClientAdminAccount" Width="80px" HeaderText="管理员" />
                    <f:RenderField Width="80px" ColumnID="Sheng" DataField="Sheng"
                        RendererFunction="renderSheng" HeaderText="省">
                    </f:RenderField>
                    <f:RenderField Width="80px" ColumnID="Shi" DataField="Shi"
                        RendererFunction="renderShi" HeaderText="市">
                    </f:RenderField>
                    <f:BoundField DataField="Notes" ExpandUnusedSpace="true" HeaderText="备注" />
                    <f:WindowField TextAlign="Center" Icon="Information" ToolTip="查看详细信息" Title="查看详细信息"
                        WindowID="Window1" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/Admin/Client_View.aspx?id={0}"
                        Width="50px" />
                    <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                        WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/Admin/Client_Edit.aspx?id={0}"
                        Width="50px" />
                    <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="删除"
                        ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px" />
                </Columns>
                <Listeners>
                    <f:Listener Event="beforerowcontextmenu" Handler="onGridBeforeRowContextMenu" />
                </Listeners>
            </f:Grid>
        </Items>
    </f:Panel>
    <f:Menu ID="Menu1" runat="server">
        <f:MenuButton ID="mbEnableRows" OnClick="btnEnableUsers_Click" runat="server" Text="启用客户">
        </f:MenuButton>
        <f:MenuButton ID="mbDisableRows" OnClick="btnDisableUsers_Click" runat="server" Text="禁用客户">
        </f:MenuButton>
    </f:Menu>
    <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
        EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="800px"
        Height="550px" OnClose="Window1_Close">
    </f:Window>
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script>

        var menuID = '<%= Menu1.ClientID %>';

        function onGridBeforeRowContextMenu(event) {
            F(menuID).show();
            return false; // 阻止弹出系统菜单
        }

        // 根据省代码获取省名称
        function getShengName(shengValue) {
            var shengName = '', sheng;
            if (shengValue) {
                for (var i = 0, count = window._SHENG.length; i < count; i++) {
                    sheng = window._SHENG[i];
                    if (shengValue == sheng[0]) {
                        shengName = sheng[1];
                        break;
                    }
                }
            }
            return shengName;
        }

    // 根据省代码和市代码获取市名称
    function getShiName(shengValue, shiValue) {
        var shiData = window._SHI[shengValue], shi, shiName = '';
        if (shiData) {
            for (var i = 0, count = shiData.length; i < count; i++) {
                shi = shiData[i];
                if (shiValue == shi[0]) {
                    shiName = shi[1];
                    break;
                }
            }
        }
        return shiName;
    }


    function renderGender(value) {
        return value == 1 ? '男' : '女';
    }

    function renderCompanyStatus(value) {
        if (value == 'Enabled')
            return '正常';
        else if (value == 'Disabled')
            return '禁用';
        else
            return '过期';
    }

    function renderSheng(value) {
        return getShengName(value);
    }

    function renderShi(value, params) {
        var shengValue = params.rowValue['Sheng']; // 'Sheng' - ColumnID
        return getShiName(shengValue, value);
    }

    var windowClientID = '<%= Window1.ClientID %>';
    var gridClientID = '<%= Grid1.ClientID %>';

    F.ready(function () {

    });

    </script>


</asp:Content>