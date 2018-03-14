<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Budget_AddMaterial.aspx.cs" Inherits="Infobasis.Web.Pages.Budget.Budget_AddMaterial"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
        Layout="VBox">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                        Text="关闭">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                        runat="server" Text="选择后关闭">
                    </f:Button>
                    <f:ToolbarFill ID="ToolbarFill1" runat="server">
                    </f:ToolbarFill>
                    <f:TwinTriggerBox ID="ttbSearchMessage" Width="160px" runat="server" ShowLabel="false"
                        EmptyText="在名称中搜索" Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false"
                        OnTrigger2Click="ttbSearchMessage_Trigger2Click" OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                    </f:TwinTriggerBox>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:DropDownBox runat="server" ID="DropDownBox1" EmptyText="请从下面表格中选择"
                DataControlID="Grid1" PopPanelID="Grid1" AlwaysDisplayPopPanel="true"
                EnableMultiSelect="true" MatchFieldWidth="false" AutoShowClearIcon="true" Margin="0 0 5">
            </f:DropDownBox>
            <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true"
                DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort"
                SortField="Name" SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true"
                OnPageIndexChange="Grid1_PageIndexChange"
                KeepCurrentSelection="true" DataIDField="ID" DataTextField="Name"
                MouseWheelSelection="true" QuickPaging="true">
                <Columns>
                    <f:RowNumberField />
                    <f:BoundField DataField="Name" SortField="Name" Width="100px" HeaderText="项目名称" />
                    <f:BoundField DataField="BrandName" SortField="BrandName" Width="100px" HeaderText="品牌" />
                    <f:BoundField DataField="Model" SortField="Model" Width="100px" HeaderText="型号" />
                    <f:BoundField DataField="Spec" SortField="Spec" Width="100px" HeaderText="规格" />
                    <f:BoundField DataField="SalePrice" Width="80px"  HeaderText="单价" DataToolTipField="SalePrice" />
                    <f:BoundField DataField="CustomizationTypeName" SortField="CustomizationTypeName" Width="80px"  HeaderText="套餐配置" HeaderToolTip="套餐配置" />
                    <f:BoundField DataField="BudgetTypeNames" SortField="BudgetTypeNames" Width="120px"  HeaderText="定制适用范围" HeaderToolTip="定制适用范围" />
                    <f:BoundField DataField="Remark" ExpandUnusedSpace="true" HeaderText="备注" />
                </Columns>
                <PageItems>
                    <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
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
            </f:Grid>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>