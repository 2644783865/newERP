<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BudgetTemplateDesign.aspx.cs" Inherits="Infobasis.Web.Pages.Design.BudgetTemplateDesign" 
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
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" ShowBorder="false" ShowHeader="false" Layout="Region">
        <Items>
            <f:Panel runat="server" ID="panelTopRegion" RegionPosition="Top" RegionSplit="false" RegionSplitWidth="3px" EnableCollapse="false" Height="75px"
                Title="顶部面板" ShowBorder="true" ShowHeader="true" BodyPadding="5px">
                <Items>
                    <f:Button ID="btnIcon1" Text="添加空间" Icon="Star" runat="server" CssClass="marginr" />
                    <f:Button ID="Button1" Text="添加费率" Icon="CoinsAdd" runat="server" CssClass="marginr" />
                    <f:Button ID="Button2" Text="删除" Icon="Delete" runat="server" CssClass="marginr" />
                    <f:Button ID="Button3" Text="保存" Icon="PageSave" runat="server" CssClass="marginr" />
                </Items>
            </f:Panel>
            <f:Panel runat="server" ID="panelLeftRegion" RegionPosition="Left" RegionSplit="true" RegionSplitWidth="3px" EnableCollapse="true"
                Width="260px" Title="空间列表" ShowBorder="true" ShowHeader="true" BodyPadding="5px" Layout="VBox">
                <Items>
                    <f:Grid ID="Grid1" runat="server" ShowBorder="false" ShowHeader="false" EnableCheckBoxSelect="true"
                        DataKeyNames="ID" AllowSorting="true" OnSort="Grid1_Sort" SortField="Name" SortDirection="DESC"
                        AllowPaging="false" EnableMultiSelect="false" EnableHeaderMenu="false" EnableColumnResize="true" EnableColumnMove="false"
                        EnableSummary="false" SummaryPosition="Flow" AllowCellEditing="true" ClicksToEdit="1"
                        OnRowSelect="Grid1_RowSelect" EnableRowSelectEvent="true" MouseWheelSelection="true" AutoScroll="true">
                        <Columns>
                            <f:BoundField DataField="Name" Width="80px" ColumnID="Name" HeaderText="名称"></f:BoundField>
                            <f:BoundField DataField="Amount" Width="60px" ColumnID="Amount" HeaderText="金额"></f:BoundField>
                            <f:RenderField Width="60px" ColumnID="Size" DataField="Size" ExpandUnusedSpace="true"
                                HeaderText="面积">
                                <Editor>
                                    <f:TextBox ID="tbxEditorSize" Required="false" runat="server">
                                    </f:TextBox>
                                </Editor>
                            </f:RenderField>
                        </Columns>
                    </f:Grid>
                    <f:Panel runat="server" CssClass="totalpanel" ShowBorder="false" ShowHeader="false" Layout="VBox">
                        <Items>
                            <f:Panel ID="Panel4" runat="server" Layout="HBox" ShowHeader="false" ShowBorder="false" CssClass="totalpanelSumPanel">
                                <Items>
                                    <f:Label ID="Label4" runat="server" BoxFlex="1" Label="面积" Width="30px"></f:Label>
                                    <f:Label runat="server" BoxFlex="1" ID="labTotalSize" ShowLabel="false" Text="6000" CssClass="totalSummaryLabel"></f:Label>
                                </Items>
                            </f:Panel>
                            <f:Panel runat="server" Layout="HBox" ShowHeader="false" ShowBorder="false" CssClass="totalpanelSumPanel">
                                <Items>
                                    <f:Label runat="server" BoxFlex="1" Label="基装小计" Width="30px"></f:Label>
                                    <f:Label runat="server" BoxFlex="1" ID="labProcessSum" ShowLabel="false" Text="6000" CssClass="totalSummaryLabel"></f:Label>
                                </Items>
                            </f:Panel>
                            <f:Panel ID="Panel2" runat="server" Layout="HBox" ShowHeader="false" ShowBorder="false" CssClass="totalpanelSumPanel">
                                <Items>
                                    <f:Label ID="Label1" BoxFlex="1" runat="server" Label="材料小计" Width="30px"></f:Label>
                                    <f:Label runat="server" BoxFlex="1" ID="labMaterialSum" ShowLabel="false" Text="2000" CssClass="totalSummaryLabel"></f:Label>
                                </Items>
                            </f:Panel>
                            <f:Panel ID="Panel3" runat="server" Layout="HBox" ShowHeader="false" ShowBorder="false" CssClass="totalpanelSumPanel">
                                <Items>
                                    <f:Label ID="Label2" BoxFlex="1" runat="server" Label="费率小计" Width="30px"></f:Label>
                                    <f:Label runat="server" BoxFlex="1" ID="labOtherFeeSum" ShowLabel="false" Text="2000" CssClass="totalSummaryLabel"></f:Label>
                                </Items>
                            </f:Panel>

                            <f:Panel ID="Panel5" runat="server" Layout="HBox" ShowHeader="false" ShowBorder="true" CssStyle="padding-left:0;border-left:0;border-right:0;border-bottom:0;background-color:#eee">
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
                Title="清单" ShowBorder="true" ShowHeader="false" BodyPadding="5px">
                <Items>
                    <f:Grid ID="Grid2" runat="server" ShowBorder="false" ShowHeader="false" EnableCheckBoxSelect="true"
                        DataKeyNames="ID" AllowSorting="true" SortField="Name" SortDirection="DESC"
                        AllowPaging="false" EnableMultiSelect="false" EnableHeaderMenu="false" EnableColumnResize="true" EnableColumnMove="false"
                        EnableSummary="false" SummaryPosition="Flow" AllowCellEditing="true" ClicksToEdit="1"
                        EnableRowSelectEvent="true" MouseWheelSelection="true" AutoScroll="true">
                        <Columns>
                            <f:BoundField DataField="Name" Width="180px" ColumnID="Name" HeaderText="名称"></f:BoundField>
                            <f:BoundField DataField="UnitName" Width="80px" ColumnID="UnitName" HeaderText="单位"></f:BoundField>
                            <f:BoundField DataField="Qty" Width="80px" ColumnID="Qty" HeaderText="数量"></f:BoundField>
                            <f:BoundField DataField="Amount" Width="80px" ColumnID="Amount" HeaderText="金额"></f:BoundField>
                            <f:BoundField DataField="Spec" Width="80px" ColumnID="Spec" HeaderText="规格"></f:BoundField>
                            <f:BoundField DataField="Remark" Width="80px" ColumnID="Remark" HeaderText="备注"></f:BoundField>
                        </Columns>
                    </f:Grid>
                </Items>
            </f:Panel>
            <f:Panel runat="server" ID="panelBottomRegion" RegionPosition="Bottom" RegionSplit="true" RegionSplitWidth="3px" EnableCollapse="true" Height="80px"
                Title="费率" ShowBorder="true" ShowHeader="true" BodyPadding="5px">
                <Items>
                    <f:Label ID="Label5" runat="server" Text="底部费用内容">
                    </f:Label>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>