<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BudgetTemplateDesign.aspx.cs" Inherits="Infobasis.Web.Pages.Budget.BudgetTemplateDesign"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style>
        .f-grid-row-summary .f-grid-cell-inner {
            font-weight: bold;
            color: red;
        }
        .totalpanel {
            border-top-width: 0 !important;
            padding-left: 20px;
        }
        .totalpanelSumPanel {
            padding-left: 20px;
        }
        .totalSummaryLabel {
            color: #ff6a00; 
            font-weight: bold;
        }
        .totalSummaryAllLabel {
            color: red; 
            font-weight: bold;
            font-size: 1.2em;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="PanelMain" runat="server" />
        <f:Panel ID="PanelMain" Title="面板" runat="server" Height="300px" EnableCollapse="false"
            Width="850px" BodyPadding="5px" ShowBorder="false" ShowHeader="false" AutoScroll="true">
            <Items>
            <f:Panel ID="Panel1" runat="server" ShowBorder="false" ShowHeader="false" Layout="Region" AutoScroll="true"  Height="380px">
                <Items>
                    <f:Panel runat="server" ID="panelTopRegion" RegionPosition="Top" RegionSplit="false" RegionSplitWidth="3px" EnableCollapse="false" Height="35px"
                        Title="顶部面板" ShowBorder="true" ShowHeader="false" BodyPadding="5px">
                        <Items>
                            <f:Button ID="btnAddItem" Text="添加模版房间" Icon="Add" runat="server" CssClass="marginr" />
                            <f:Button ID="btnAddBasePrice" Text="添加套餐基价" Icon="Star" runat="server" CssClass="marginr" />
                            <f:Button ID="btnAddRate" Text="添加费率" Icon="CoinsAdd" runat="server" CssClass="marginr" />
                            <f:Button ID="Button2" Text="删除" Icon="Delete" runat="server" CssClass="marginr" />
                            <f:Button ID="Button3" Text="保存" Icon="PageSave" runat="server" CssClass="marginr" />
                        </Items>
                    </f:Panel>
                    <f:Panel runat="server" ID="panelLeftRegion" RegionPosition="Left" RegionSplit="true" RegionSplitWidth="3px" EnableCollapse="true"
                        Width="260px" Title="模版房间" ShowBorder="true" ShowHeader="true" BodyPadding="5px" Layout="VBox" AutoScroll="true">
                        <Items>
                            <f:Grid ID="Grid1" runat="server" ShowBorder="false" ShowHeader="false" EnableCheckBoxSelect="true"
                                DataKeyNames="ID" AllowSorting="true" OnSort="Grid1_Sort" SortField="Name" SortDirection="DESC"
                                AllowPaging="false" EnableMultiSelect="false" EnableHeaderMenu="false" EnableColumnResize="true" EnableColumnMove="false"
                                EnableSummary="false" SummaryPosition="Flow" AllowCellEditing="true" ClicksToEdit="1"
                                OnRowSelect="Grid1_RowSelect" EnableRowSelectEvent="true" MouseWheelSelection="false" AutoScroll="true">
                                <Columns>
                                    <f:BoundField DataField="PartTypeName" Width="80px" ColumnID="Name" HeaderText="名称"></f:BoundField>
                                    <f:BoundField DataField="BudgetTemplateItemMaterials.Count" Width="160px" ColumnID="BudgetTemplateItemMaterials.Count" HeaderText="定额项目" DataFormatString="已定义 {0} 个项目"></f:BoundField>
                                    <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="删除"
                                        ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px" />
                                                        
                                    <f:RenderField Width="60px" ColumnID="Size" DataField="Size" ExpandUnusedSpace="true"
                                        HeaderText="面积">
                                        <Editor>
                                            <f:TextBox ID="tbxEditorSize" Required="false" runat="server">
                                            </f:TextBox>
                                        </Editor>
                                    </f:RenderField>
                                </Columns>
                            </f:Grid>
                            <f:Panel ID="Panel2" runat="server" CssClass="totalpanel" ShowBorder="false" ShowHeader="false" Layout="VBox">
                                <Items>
                                    <f:Panel ID="Panel4" runat="server" Layout="HBox" ShowHeader="false" ShowBorder="false" CssClass="totalpanelSumPanel">
                                        <Items>
                                            <f:Label ID="Label4" runat="server" BoxFlex="1" Label="面积" Width="30px"></f:Label>
                                            <f:Label runat="server" BoxFlex="1" ID="labTotalSize" ShowLabel="false" Text="6000" CssClass="totalSummaryLabel"></f:Label>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel3" runat="server" Layout="HBox" ShowHeader="false" ShowBorder="false" CssClass="totalpanelSumPanel">
                                        <Items>
                                            <f:Label ID="Label1" runat="server" BoxFlex="1" Label="基装小计" Width="30px"></f:Label>
                                            <f:Label runat="server" BoxFlex="1" ID="labProcessSum" ShowLabel="false" Text="6000" CssClass="totalSummaryLabel"></f:Label>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel5" runat="server" Layout="HBox" ShowHeader="false" ShowBorder="false" CssClass="totalpanelSumPanel">
                                        <Items>
                                            <f:Label ID="Label2" BoxFlex="1" runat="server" Label="材料小计" Width="30px"></f:Label>
                                            <f:Label runat="server" BoxFlex="1" ID="labMaterialSum" ShowLabel="false" Text="2000" CssClass="totalSummaryLabel"></f:Label>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel6" runat="server" Layout="HBox" ShowHeader="false" ShowBorder="false" CssClass="totalpanelSumPanel">
                                        <Items>
                                            <f:Label ID="Label3" BoxFlex="1" runat="server" Label="费率小计" Width="30px"></f:Label>
                                            <f:Label runat="server" BoxFlex="1" ID="labOtherFeeSum" ShowLabel="false" Text="2000" CssClass="totalSummaryLabel"></f:Label>
                                        </Items>
                                    </f:Panel>

                                    <f:Panel ID="Panel7" runat="server" Layout="HBox" ShowHeader="false" ShowBorder="true" CssStyle="padding-left:0;border-left:0;border-right:0;border-bottom:0;background-color:#eee">
                                        <Items>
                                            <f:Label ID="Label6" BoxFlex="1" runat="server" Label="小计" Width="30px"></f:Label>
                                            <f:Label runat="server" BoxFlex="1" ID="labTotalFee" ShowLabel="false" Text="2000" CssClass="totalSummaryAllLabel"></f:Label>
                                        </Items>
                                    </f:Panel>

                                </Items>
                            </f:Panel>
                        </Items>
                    </f:Panel>
                    <f:Panel runat="server" ID="panelCenterRegion" RegionPosition="Center"
                        Title="模版房间和项目定义" ShowBorder="false" ShowHeader="true" BodyPadding="5px">
                        <Items>
                            <f:Grid ID="Grid2" runat="server" ShowBorder="false" ShowHeader="false" EnableCheckBoxSelect="true"
                                DataKeyNames="ID" AllowSorting="true" SortField="Material.Name" SortDirection="DESC"
                                AllowPaging="false" EnableMultiSelect="false" EnableHeaderMenu="false" EnableColumnResize="true" EnableColumnMove="false"
                                EnableSummary="false" SummaryPosition="Flow" AllowCellEditing="true" ClicksToEdit="1"
                                EnableRowSelectEvent="true" MouseWheelSelection="true" AutoScroll="true">
                                <Columns>
                                    <f:BoundField DataField="Material.Name" Width="120px" ColumnID="Material.Name" HeaderText="名称"></f:BoundField>
                                    <f:BoundField DataField="Material.BrandName" Width="80px" ColumnID="Material.BrandName" HeaderText="品牌"></f:BoundField>
                                    <f:BoundField DataField="Material.Spec" Width="80px" ColumnID="Spec" HeaderText="规格"></f:BoundField>
                                    <f:BoundField DataField="Material.UnitName" Width="30px" ColumnID="UnitName" HeaderText="单位"></f:BoundField>
                                    <f:BoundField DataField="Qty" Width="130px" ColumnID="Qty" HeaderText="数量"></f:BoundField>
                                    <f:BoundField DataField="DisplayOrder" Width="80px" ColumnID="DisplayOrder" HeaderText="排序"></f:BoundField>
                                    <f:BoundField DataField="Remark" Width="80px" ColumnID="Remark" HeaderText="备注"></f:BoundField>
                                </Columns>
                                <Toolbars>
                                    <f:Toolbar  runat="server" Position="Bottom">
                                        <Items>
                                            <f:Button ID="btnDeleteMaterialSelected" Icon="Delete" runat="server" Text="移除选中的项目">
                                            </f:Button>
                                            <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                            </f:ToolbarFill>
                                            <f:Button ID="btnAddNewItemMaterial" runat="server" Icon="Add" EnablePostBack="false" Text="添加">
                                            </f:Button>
                                        </Items>
                                    </f:Toolbar>
                                </Toolbars>
                            </f:Grid>
                        </Items>
                    </f:Panel>
                    <f:Panel runat="server" ID="panelBottomRegion" RegionPosition="Bottom" RegionSplit="false" RegionSplitWidth="3px" EnableCollapse="true" Height="80px"
                        Title="费率" ShowBorder="true" ShowHeader="true" BodyPadding="5px">
                        <Items>
                            <f:Label ID="Label5" runat="server" Text="底部费用内容">
                            </f:Label>
                        </Items>
                    </f:Panel>
                </Items>
            </f:Panel>
            <f:Panel ID="Panel9" runat="server" Width="260px" Title="套餐基价">
                <Items>
                    <f:Grid runat="server" ID="Grid3" ShowBorder="false" ShowHeader="false">
                        <Columns>
                            <f:BoundField DataField="Material.Name" Width="120px" ColumnID="Material.Name" HeaderText="项目"></f:BoundField>
                            <f:BoundField DataField="Material.Price" Width="30px" ColumnID="UnitName" HeaderText="单价"></f:BoundField>
                            <f:BoundField DataField="Qty" Width="130px" ColumnID="Qty" HeaderText="基数"></f:BoundField>
                            <f:BoundField DataField="DisplayOrder" Width="80px" ColumnID="DisplayOrder" HeaderText="排序"></f:BoundField>
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Panel>
    </Items>
</f:Panel>

    <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
        EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="500px"
        Height="360px" OnClose="Window1_Close">
    </f:Window>
    <f:Window ID="Window2" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
        EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="800px"
        Height="430px" OnClose="Window1_Close">
    </f:Window>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>