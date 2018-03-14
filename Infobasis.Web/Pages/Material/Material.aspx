<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Material.aspx.cs" Inherits="Infobasis.Web.Pages.Material.Material"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style>
        .traceWarning {
            color: red;
        }
        .traceNormal {
            color: #111;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" BodyPadding="5px" AutoScroll="true"
        ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
        ShowHeader="false" Title="材料管理">
        <Items>
            <f:Form ID="Form2" runat="server" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                <Rows>
                    <f:FormRow ID="FormRow1" runat="server">
                        <Items>
                            <f:SimpleForm ID="SimpleForm1" CssClass="mysimpleform" runat="server" ShowBorder="false" EnableCollapse="true"
                                Layout="VBox" ShowHeader="false">
                                <Items>
                                    <f:Panel ID="Panel2" runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                        <Items>
                                            <f:DropDownList runat="server" ID="DropDownProvince" Label="城市" BoxFlex="1">
                                                <f:ListItem Text="城市" Value="0" />
                                            </f:DropDownList>
                                            <f:DropDownList runat="server" ID="DropDownMainMaterialType" Label="主辅材" BoxFlex="1" AutoPostBack="true" OnSelectedIndexChanged="DropDownMainMaterialType_SelectedIndexChanged">
                                                <f:ListItem Text="主材" Value="0" />
                                            </f:DropDownList>
                                            <f:DropDownList runat="server" ID="DropDownMaterialType" Label="材料类别" BoxFlex="1">
                                                <f:ListItem Text="" Value="0" />
                                            </f:DropDownList>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel3" runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                        <Items>
                                            <f:TextBox runat="server" ID="tbxBrand" Label="品牌" BoxFlex="1"></f:TextBox>
                                            <f:TextBox runat="server" ID="tbxName" Label="材料名称" BoxFlex="1"></f:TextBox>
                                            <f:TextBox runat="server" ID="tbxCode" Label="材料编号" BoxFlex="1"></f:TextBox>
                                            <f:DropDownList runat="server" ID="DropDownCustomType" BoxFlex="2" Label="定制" EnableMultiSelect="true" AutoShowClearIcon="true">
                                                <f:ListItem Text="" Value="0" />
                                            </f:DropDownList>             
                                            <f:HiddenField ID="HiddenFieldInput" runat="server">
                                            </f:HiddenField>
                                            <f:LinkButton ID="btnReset"  runat="server" CssStyle="margin-left:6px;" Text="重置" EnablePostBack="false">
                                            </f:LinkButton>
                                            <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server" MarginLeft="6px"></f:ToolbarSeparator>
                                            <f:Button runat="server" ID="btnSearch" CssStyle="margin-left:6px;" Text="查询" ValidateForms="SimpleForm1" OnClick="btnSearch_Click" IconFont="Search"></f:Button>
                                            <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server" MarginLeft="6px"></f:ToolbarSeparator>                            
                                            <f:Button ID="btnNew" Icon="Add" runat="server" Text="新增材料" EnablePostBack="false">
                                            </f:Button>
                                        </Items>
                                    </f:Panel>

                                </Items>
                            </f:SimpleForm>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
            <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                EnableCheckBoxSelect="true" AllowColumnLocking="false" AutoScroll="true"
                DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="Name"
                SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true" OnPreDataBound="Grid1_PreDataBound"
                OnPageIndexChange="Grid1_PageIndexChange" OnRowDataBound="Grid1_RowDataBound"
                MouseWheelSelection="true" QuickPaging="true">
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
                    <f:RowNumberField Width="15px" EnablePagingNumber="true" EnableLock="false" Locked="false" />
                    <f:ImageField DataImageUrlField="PicPath" HeaderText="图片" ImageHeight="60px"></f:ImageField> 
                    <f:TemplateField HeaderText="材料编号" Width="100px">
                        <ItemTemplate>
                            <a href="javascript:;" onclick="<%# GetEditUrl(Eval("Id"), Eval("Name")) %>"><%# Eval("Code") %></a>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:TemplateField HeaderText="名称" Width="100px">
                        <ItemTemplate>
                            <a href="javascript:;" onclick="<%# GetEditUrl(Eval("Id"), Eval("Name")) %>"><%# Eval("Name") %></a>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:BoundField DataField="BrandName" SortField="BrandName" Width="80px" HeaderText="品牌" />
                    <f:BoundField DataField="Model" Width="80px"  HeaderText="型号" DataToolTipField="Model" />
                    <f:BoundField DataField="Spec" Width="80px"  HeaderText="规格" DataToolTipField="Spec" />
                    <f:BoundField DataField="SalePrice" Width="80px"  HeaderText="销售价格" DataToolTipField="SalePrice" />
                    <f:BoundField DataField="PurchasePrice" Width="80px"  HeaderText="采购价格" DataToolTipField="PurchasePrice" />
                    <f:BoundField DataField="UpgradePrice" Width="80px"  HeaderText="升级费用" DataToolTipField="UpgradePrice" />
                    <f:BoundField DataField="CustomizationTypeName" SortField="CustomizationTypeName" Width="80px"  HeaderText="定制配置" HeaderToolTip="定制配置" />
                    <f:BoundField DataField="BudgetTypeNames" SortField="BudgetTypeNames" Width="120px"  HeaderText="定制适用范围" HeaderToolTip="定制适用范围" />
                    <f:BoundField Width="90px" DataField="CreateDatetime" DataToolTipField="CreateDatetime" DataToolTipFormatString="{0:yyyy-MM-dd hh:MM:dd}" DataFormatString="{0:yyyy-MM-dd}" HeaderText="添加日期" />

                </Columns>
            </f:Grid>
        </Items>
    </f:Panel>
    <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
        EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="800px"
        Height="550px" OnClose="Window1_Close">
    </f:Window>
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">

    <script>

        function onClearDateClick(event) {
            // this -> 按钮实例；获取按钮所在的表单ID
            var formId = this.el.parents('.f-form').attr('id');

            F(formId).reset();
        }

        function getNewWindowUrl() {
            return F.baseUrl + 'Pages/Material/Material_Form.aspx?parenttabid=' + parent.getActiveTabId();
        }


        function getEditWindowUrl(id, name) {
            return F.baseUrl + 'Pages/Material/Material_Form.aspx?id=' + id + '&name=' + name + '&parenttabid=' + parent.getActiveTabId();
        }

    </script>
</asp:Content>