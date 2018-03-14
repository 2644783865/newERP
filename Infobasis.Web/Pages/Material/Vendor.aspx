<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vendor.aspx.cs" Inherits="Infobasis.Web.Pages.Material.Vendor"
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
        ShowHeader="false" Title="供应商管理">
        <Items>
            <f:Form ID="FormSearch" runat="server" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
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
                                            <f:DropDownList runat="server" ID="DropDownMainMaterialType" Label="主辅材" BoxFlex="1" OnSelectedIndexChanged="DropDownMainMaterialType_SelectedIndexChanged">
                                                <f:ListItem Text="主材" Value="0" />
                                            </f:DropDownList>
                                            <f:DropDownList runat="server" ID="DropDownMaterialType" Label="材料类别" BoxFlex="1">
                                                <f:ListItem Text="" Value="0" />
                                            </f:DropDownList>
                                            <f:TextBox runat="server" ID="tbxName" Label="材料商简称" BoxFlex="1"></f:TextBox>
                                            <f:TextBox runat="server" ID="tbxCode" Label="材料商编号" BoxFlex="1"></f:TextBox>            
                                            <f:HiddenField ID="HiddenFieldInput" runat="server">
                                            </f:HiddenField>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel ID="Panel3" runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                        <Items>

                                            <f:TextBox runat="server" ID="tbxFullName" Label="材料商名称" BoxFlex="1"></f:TextBox>
                                            <f:TextBox runat="server" ID="tbxBrand" Label="品牌" BoxFlex="1"></f:TextBox>
                                            <f:TextBox runat="server" ID="tbxBankAccount" Label="往来银行账号" BoxFlex="1"></f:TextBox>
                                            <f:TextBox runat="server" ID="tbxBankAccountName" Label="往来银行名称" BoxFlex="1"></f:TextBox>
                                            <f:LinkButton ID="btnReset" runat="server" CssStyle="margin-left:6px;" Text="重置" EnablePostBack="false">
                                            </f:LinkButton>
                                            <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server" MarginLeft="6px"></f:ToolbarSeparator>
                                            <f:Button runat="server" ID="btnSearch" CssStyle="margin-left:6px;" Text="查询" ValidateForms="SimpleForm1" OnClick="btnSearch_Click" IconFont="Search"></f:Button>                                        
                                            <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server" MarginLeft="6px"></f:ToolbarSeparator>
                                            <f:Button ID="btnNew" Icon="Add" runat="server" Text="新增供应商" EnablePostBack="false">
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
                    <f:ImageField DataImageUrlField="LogoPicPath" HeaderText="图片" ImageHeight="30px"></f:ImageField> 
                    <f:TemplateField HeaderText="供应商编号" Width="100px">
                        <ItemTemplate>
                            <a href="javascript:;" onclick="<%# GetEditUrl(Eval("Id"), Eval("Name")) %>"><%# Eval("Code") %></a>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:TemplateField HeaderText="简称" Width="120px">
                        <ItemTemplate>
                            <a href="javascript:;" onclick="<%# GetEditUrl(Eval("Id"), Eval("Name")) %>"><%# Eval("Name") %></a>
                        </ItemTemplate>
                    </f:TemplateField>
                    <f:BoundField DataField="ContactName" SortField="ContactName" Width="80px" HeaderText="对接人" />
                    <f:BoundField DataField="ContactTel" Width="80px"  HeaderText="联系电话" DataToolTipField="ContactTel" />
                    <f:BoundField DataField="ContactCellPhone" Width="80px"  HeaderText="移动电话" DataToolTipField="ContactCellPhone" />
                    <f:BoundField DataField="Fax" Width="80px"  HeaderText="传真" DataToolTipField="Fax" />
                    <f:BoundField DataField="ERPAccount" Width="80px"  HeaderText="ERP账号" DataToolTipField="ERPAccount" />
                    <f:CheckBoxField DataField="OpenERPAccount" HeaderText="启用ERP" RenderAsStaticField="true"
                        AutoPostBack="true" CommandName="OpenERPAccount" Width="100px" />
                    <f:BoundField DataField="DisplayOrder" Width="80px"  HeaderText="排序" DataToolTipField="DisplayOrder" />
                    <f:TemplateField HeaderText="材料商状态" Width="120px">
                        <ItemTemplate>
                            <span><%# GetVendorStatus(Eval("VendorStatus")) %></span>
                        </ItemTemplate>
                    </f:TemplateField>
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
            var formId = '<%# SimpleForm1.ClientID %>';

            F(formId).reset();
        }

        function getNewWindowUrl() {
            return F.baseUrl + 'Pages/Material/Vendor_Form.aspx?parenttabid=' + parent.getActiveTabId();
        }


        function getEditWindowUrl(id, name) {
            return F.baseUrl + 'Pages/Material/Vendor_Form.aspx?id=' + id + '&name=' + name + '&parenttabid=' + parent.getActiveTabId();
        }

    </script>
</asp:Content>
