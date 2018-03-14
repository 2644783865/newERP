<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vendor_Form.aspx.cs" Inherits="Infobasis.Web.Pages.Material.Vendor_Form"
     MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" href="../../res/third/jqueryuiautocomplete/jquery-ui.css" />
    <style>
        .logoImg {padding-left: 10px;}
        .logoImg img {width: 60px;height:60px}
    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" />
        <f:Panel ID="Panel2" runat="server" Height="380px" ShowBorder="False" EnableCollapse="False"
            Layout="HBox" BodyPadding="5px"
            BoxConfigChildMargin="0 5 0 0" ShowHeader="False"
            Title="">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server">
                        </f:Button>
                        <f:Button ID="btnCloseRefresh" ValidateForms="SimpleForm1" EnablePostBack="true" Text="保存并刷新供应商页面" runat="server" OnClick="btnCloseRefresh_Click">
                        </f:Button>
                        <f:Button ID="btnContinueToAdd" ValidateForms="SimpleForm1" EnablePostBack="true" Text="保存并继续添加" runat="server" OnClick="btnContinueToAdd_Click"></f:Button>
                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                        </f:ToolbarFill>
                        <f:ToolbarText ID="ToolbarText1" Text="" runat="server">
                        </f:ToolbarText>
                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                        </f:ToolbarSeparator>
                        <f:ToolbarText ID="ToolbarText2" Text="&nbsp;&nbsp;" runat="server">
                        </f:ToolbarText>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Panel ID="Panel1" Title="面板1" Width="100px" runat="server"
                    BodyPadding="5px" ShowBorder="true" ShowHeader="false">
                    <Items>
                        <f:FileUpload ID="logoImgUpload" CssClass="uploadbutton" runat="server" ButtonText="上传Logo" ButtonOnly="true" AutoPostBack="true" OnFileSelected="logoImgUpload_FileSelected"></f:FileUpload>
                        <f:Image ID="logoImg" CssClass="logoImg" ImageUrl="~/res/images/blank_gray.jpg" runat="server" BoxFlex="1">
                        </f:Image>
                    </Items>
                </f:Panel>
                <f:Panel ID="Panel3" Title="面板2" BoxFlex="2"
                    runat="server" BodyPadding="5px" ShowBorder="true" ShowHeader="false">
                    <Items>
                    <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false"
                        AutoScroll="true" BodyPadding="10px" runat="server">
                        <Rows>
                            <f:FormRow>
                                <Items>
                                    <f:DropDownList ID="DropDownProvince" Label="区域" Required="false" ShowRedStar="false" runat="server">
                                        <f:ListItem Text="浦东" Value="浦东" />
                                        <f:ListItem Text="青浦" Value="青浦" />
                                    </f:DropDownList>
                                    <f:TextBox ID="tbxCode" Label="材料商编号" Required="true" runat="server" />
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:TextBox ID="tbxName" Label="简称" Required="true" CssClass="highlight" runat="server" />
                                    <f:TextBox ID="tbxFullName" Label="材料商名称" Required="true" CssClass="highlight" runat="server" />
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:DropDownList ID="DropDownMainMaterialType" Required="true" Label="供货类型" EnableEdit="false" 
                                        AutoPostBack="true" OnSelectedIndexChanged="DropDownMainMaterialType_SelectedIndexChanged" runat="server">
                                        <f:ListItem Text="" Value="0" />
                                    </f:DropDownList>
                                    <f:DropDownList ID="DropDownMaterialType" Required="true" Label="材料类别" EnableEdit="false"
                                        runat="server">
                                        <f:ListItem Text="" Value="0" />
                                    </f:DropDownList>
                                    <f:DropDownList ID="DropDownVendorStatus" Required="true" Label="准入状态" EnableEdit="false"
                                        runat="server">
                                        <f:ListItem Text="合格" Value="1" />
                                        <f:ListItem Text="临时" Value="2" />
                                        <f:ListItem Text="停用" Value="3" />
                                    </f:DropDownList>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:TextBox ID="tbxBandAccount" Label="银行账号" Required="false" runat="server" />
                                    <f:TextBox ID="tbxBandAccountName" Label="银行名称" Required="false" runat="server" />
                                    <f:NumberBox ID="tbxPaymentNum" Label="账期" Required="false" runat="server"></f:NumberBox>              
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:TextBox ID="tbxLocation" Label="公司地址" Required="false" runat="server" /> 
                                    <f:TextBox ID="tbxBrand" Label="品牌" Required="false" runat="server" /> 
                                    <f:DropDownList ID="DropDownCompanySize" Required="false" Label="公司大小" EnableEdit="false"
                                        runat="server">
                                        <f:ListItem Text="10人以下" Value="10人以下" />
                                        <f:ListItem Text="10人至100人" Value="10人至100人" />
                                        <f:ListItem Text="100人至1000人" Value="100人至1000人" />
                                        <f:ListItem Text="1000人以上" Value="1000人以上" />
                                    </f:DropDownList>                                                           
                                </Items>
                            </f:FormRow>
                            <f:FormRow CssStyle="">
                                <Items>
                                    <f:TextBox ID="tbxERPAccount" Label="ERP账号" Required="false" runat="server" /> 
                                    <f:TextBox ID="tbxERPAccountPwd" Label="ERP密码" InputType="Password" Required="false" runat="server" />
                                    <f:CheckBox ID="tbxOpenERPAccount" Label="开通" runat="server">
                                        <Listeners>
                                            <f:Listener Event="change" Handler="onOpenERPAccountChange" />
                                        </Listeners>
                                    </f:CheckBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:TextArea ID="tbxRemark" Height="80px" Label="描述" runat="server" Required="False" ShowRedStar="false" />
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:NumberBox ID="tbxDisplayOrder" Label="排序" Required="false" runat="server"></f:NumberBox>
                                </Items>
                            </f:FormRow>
                        </Rows>
                    </f:Form>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
        <f:Panel runat="server" ShowBorder="true" ShowHeader="true" AutoScroll="true" Title="供应商员工">
            <Items>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="false" ShowHeader="false" EmptyText="还没有添加员工" Title="供应商员工"
                    EnableCheckBoxSelect="true" AllowColumnLocking="false" AutoScroll="true" ShowGridHeader="true"
                    DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="Name"
                    SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true"
                    OnPageIndexChange="Grid1_PageIndexChange"
                    MouseWheelSelection="false" QuickPaging="true">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:Button ID="btnNew" Icon="ArrowSwitch" runat="server" Text="新增员工" EnablePostBack="false">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill2" runat="server">
                                </f:ToolbarFill>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <PageItems>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:ToolbarText ID="ToolbarText3" runat="server" Text="每页记录数：">
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
                        <f:BoundField DataField="Name" SortField="Name" Width="80px" HeaderText="姓名" />
                        <f:BoundField DataField="Tel" Width="80px"  HeaderText="联系电话" DataToolTipField="Tel" />
                        <f:BoundField DataField="CellPhone" Width="80px"  HeaderText="移动电话" DataToolTipField="CellPhone" />
                        <f:BoundField DataField="Fax" Width="80px"  HeaderText="传真" DataToolTipField="Fax" />
                        <f:BoundField DataField="QQ" Width="80px"  HeaderText="QQ" DataToolTipField="Fax" />
                        <f:BoundField DataField="WeChat" Width="80px"  HeaderText="微信" DataToolTipField="Fax" />
                        <f:BoundField DataField="ERPAccount" Width="80px"  HeaderText="ERP账号" DataToolTipField="ERPAccount" />
                        <f:CheckBoxField DataField="OpenERPAccount" HeaderText="启用ERP" RenderAsStaticField="true"
                            AutoPostBack="true" CommandName="OpenERPAccount" Width="100px" />
                        <f:TemplateField HeaderText="状态" Width="60px">
                            <ItemTemplate>
                                <span><%# GetEmployeeStatus(Eval("EmployeeStatus")) %></span>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:CheckBoxField DataField="IsMainContact" HeaderText="主要对接人" RenderAsStaticField="true"
                            AutoPostBack="true" CommandName="IsMainContact" Width="100px" />
                        <f:BoundField Width="90px" DataField="CreateDatetime" DataToolTipField="CreateDatetime" DataToolTipFormatString="{0:yyyy-MM-dd hh:MM:dd}" DataFormatString="{0:yyyy-MM-dd}" HeaderText="添加日期" />

                    </Columns>
                </f:Grid>                
            </Items>
        </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="../../res/third/jqueryuiautocomplete/jquery-ui.js" type="text/javascript"></script>
    <script src="../../res/js/axios.min.js"></script>
    <script src="../../res/js/api.js"></script>
    <script>
        var tbxERPAccountClientID = '<%= tbxERPAccount.ClientID %>';
        var tbxERPAccountPwd = '<%= tbxERPAccountPwd.ClientID %>';

        // 将字符串 val 以逗号空格作为分隔符，分隔成数组
        function split(val) {
            return val.split(/,\s*/);
        }

        // 取得以逗号空格为分隔符的最后一个单词
        // 比如，输入为 "C++, C#, JavaScript" 则输入出 "JavaScript"
        function extractLast(term) {
            return split(term).pop();
        }

        function onOpenERPAccountChange(event) {
            var checked = this.isChecked();
            var tbxERPAccount = F(tbxERPAccountClientID);
            tbxERPAccount.setReadonly(!checked);
        }
    </script>
</asp:Content>