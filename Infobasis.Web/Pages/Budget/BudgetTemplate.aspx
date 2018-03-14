<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BudgetTemplate.aspx.cs" Inherits="Infobasis.Web.Pages.Budget.BudgetTemplate" 
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
        ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
        ShowHeader="false" Title="预算模板">
        <Items>
            <f:Form ID="Form2" runat="server" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                <Rows>
                    <f:FormRow ID="FormRow1" runat="server">
                        <Items>
                            <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在预算模板中搜索"
                                Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                            </f:TwinTriggerBox>
                            <f:RadioButtonList ID="rblEnableStatus" AutoPostBack="true" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged"
                                Label="模板状态" ColumnNumber="4" runat="server">
                                <f:RadioItem Text="全部" Selected="true" Value="all" />
                                <f:RadioItem Text="启用" Value="enabled" />
                                <f:RadioItem Text="禁用" Value="disabled" />
                            </f:RadioButtonList>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
            <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                EnableCheckBoxSelect="true"
                DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="Name"
                SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true" OnPreDataBound="Grid1_PreDataBound" OnPreRowDataBound="Grid1_PreRowDataBound"
                OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange"
                MouseWheelSelection="true" QuickPaging="true">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnDeleteSelected" Icon="Delete" runat="server" Text="删除选中记录" OnClick="btnDeleteSelected_Click" Hidden="true">
                            </f:Button>
                            <f:ToolbarFill ID="ToolbarFill1" runat="server">
                            </f:ToolbarFill>
                            <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增模板">
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
                    <f:TemplateField HeaderText="模板名称" Width="160px">
                        <ItemTemplate>
                            <a href="javascript:;" onclick="<%# GetEditUrl(Eval("Id"), Eval("Name")) %>"><%# Eval("Name") %></a>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:BoundField DataField="BudgetTypeName" HeaderText="预算类型" ExpandUnusedSpace="false" Width="120px" />
                    <f:RenderField Width="60px" ColumnID="Status"  SortField="BudgetTemplateStatus" DataField="BudgetTemplateStatus"
                        RendererFunction="renderStatus"  HeaderText="状态">
                    </f:RenderField>
                    <f:RenderField Width="100px" ColumnID="CreateDatetime" DataField="CreateDatetime" FieldType="Date"
                        Renderer="Date" RendererArgument="yyyy-MM-dd" HeaderText="创建日期" Hidden="false">
                    </f:RenderField>
                    <f:RenderField Width="100px" ColumnID="LastUpdateDatetime" DataField="LastUpdateDatetime" FieldType="Date"
                        Renderer="Date" RendererArgument="yyyy-MM-dd hh:mm:ss" HeaderText="最后修改日期" Hidden="false">
                    </f:RenderField>
                    <f:BoundField DataField="Remark" HeaderText="备注" ExpandUnusedSpace="true" />
                    <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                        WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/Budget/Budget_Form.aspx?id={0}"
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
        <f:MenuButton ID="mbEnableRows" runat="server" OnClick="btnEnable_Click" Text="启用模板">
        </f:MenuButton>
        <f:MenuButton ID="mbDisableRows" runat="server" OnClick="btnDisable_Click" Text="禁用模板">
        </f:MenuButton>
    </f:Menu>
    <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
        EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="500px"
        Height="360px" OnClose="Window1_Close">
    </f:Window>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script>
        var menuID = '<%= Menu1.ClientID %>';

        function onGridBeforeRowContextMenu(event) {
            F(menuID).show();
            return false; // 阻止弹出系统菜单
        }

        function renderStatus(value) {
            if (value == 'Enabled')
                return '正常';
            else if (value == 'Disabled')
                return '禁用';
            else
                return '正常';
        }

        function getNewWindowUrl() {
            return F.baseUrl + 'Pages/Budget/Budget_Form.aspx?parenttabid=' + parent.getActiveTabId();
        }


        function getEditWindowUrl(id, name) {
            return F.baseUrl + 'Pages/Budget/BudgetTemplateDesign.aspx?id=' + id + '&name=' + name + '&parenttabid=' + parent.getActiveTabId();
        }
    </script>
</asp:Content>