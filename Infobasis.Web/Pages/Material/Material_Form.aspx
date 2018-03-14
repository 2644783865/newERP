<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Material_Form.aspx.cs" Inherits="Infobasis.Web.Pages.Material.Material_Form" 
    MasterPageFile="~/PageMaster/Page.master"%>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" href="../../res/third/jqueryuiautocomplete/jquery-ui.css" />
    <style>
        .materialImg {padding-left: 20px;}
    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" />
        <f:Panel ID="Panel2" runat="server" Height="500px" ShowBorder="False" EnableCollapse="False"
            Layout="HBox" BodyPadding="5px"
            BoxConfigChildMargin="0 5 0 0" ShowHeader="False"
            Title="">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnClose" EnablePostBack="false" Text="关闭" runat="server">
                        </f:Button>
                        <f:Button ID="btnCloseRefresh" ValidateForms="SimpleForm1" EnablePostBack="true" Text="保存并刷新材料页面" runat="server" OnClick="btnCloseRefresh_Click">
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
                <f:Panel ID="Panel1" Title="面板1" Width="360px" runat="server"
                    BodyPadding="5px" ShowBorder="true" ShowHeader="false">
                    <Items>
                        <f:FileUpload ID="materialImgUpload" CssClass="uploadbutton" runat="server" ButtonText="上传图片" ButtonOnly="true" AutoPostBack="true" OnFileSelected="materialImgUpload_FileSelected"></f:FileUpload>
                        <f:Image ID="materialImg" CssClass="materialImg" ImageUrl="~/res/images/blank_gray.jpg" runat="server" BoxFlex="1">
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
                                    <f:TextBox ID="tbxCode" Label="材料编号" Required="true" runat="server" />
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:TextBox ID="tbxName" Label="材料名称" Required="true" CssClass="highlight" runat="server" />
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:TextBox ID="tbxBrand" Label="品牌" Required="false" runat="server" />
                                    <f:HiddenField ID="tbxBrandHidden" runat="server" />
                                    <f:TextBox ID="tbxModel" Label="型号" Required="false" runat="server" />
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:TextBox ID="tbxSpec" Label="规格" Required="false" runat="server" />
                                    <f:DropDownList ID="DropDownUnit" Required="true" Label="单位" EnableEdit="false"
                                        runat="server">
                                        <f:ListItem Text="" Value="0" />
                                    </f:DropDownList>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:NumberBox ID="tbxPurchasePrice" Label="采购价" Required="false" runat="server"></f:NumberBox>
                                    <f:NumberBox ID="tbxEarningFactor" Label="利润系数" DecimalPrecision="4" Required="false" runat="server"></f:NumberBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow CssStyle="">
                                <Items>
                                    <f:NumberBox ID="tbxSalePrice" Label="销售价" Required="false" runat="server"></f:NumberBox>
                                    <f:CheckBox ID="tbxNoSalePrice" Label="无销售价格" runat="server">
                                        <Listeners>
                                            <f:Listener Event="change" Handler="onNoSalePriceChange" />
                                        </Listeners>
                                    </f:CheckBox>
                                    <f:NumberBox ID="tbxReturnFactor" Label="退货系数" DecimalPrecision="4" Required="false" runat="server"></f:NumberBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:NumberBox ID="tbxUpgradePrice" Label="升级费用" Required="false" runat="server" Width="80px"></f:NumberBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow CssStyle="background-color: #6fb3e0">
                                <Items>
                                    <f:DropDownList ID="DropDownCustomizationType" Required="true" Label="定制配置"
                                        runat="server">
                                        <f:ListItem Text="" Value="0" />
                                        <f:ListItem Text="标配" Value="1" />
                                        <f:ListItem Text="增配" Value="2" />
                                    </f:DropDownList>
                                    <f:DropDownBox runat="server" ID="ddbVendor" Label="材料商" AutoShowClearIcon="true"
                                        EmptyText="请从下拉表格中选择" DataControlID="Grid2" 
                                                EnableMultiSelect="false" MatchFieldWidth="false" Width="550px">
                                        <PopPanel>
                                            <f:Panel ID="Panel7" runat="server" BodyPadding="5px" Width="550px" Height="300px" Hidden="true"
                                                ShowBorder="true" ShowHeader="false" Layout="VBox" BoxConfigAlign="Stretch">
                                                <Items>
                                                    <f:Form ID="Form5" ShowBorder="False" ShowHeader="False" runat="server">
                                                        <Rows>
                                                            <f:FormRow>
                                                                <Items>
                                                                    <f:TwinTriggerBox Width="300px" runat="server" EmptyText="在名称中查找" ShowLabel="false" ID="ttbSearch"
                                                                        ShowTrigger1="false" OnTrigger1Click="ttbSearch_Trigger1Click" OnTrigger2Click="ttbSearch_Trigger2Click"
                                                                        Trigger1Icon="Clear" Trigger2Icon="Search">
                                                                    </f:TwinTriggerBox>
                                                                    <f:RadioButtonList ID="rblEnableStatus" AutoPostBack="true" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged"
                                                                        Label="启用状态" ColumnNumber="3" runat="server">
                                                                        <f:RadioItem Text="全部" Value="all" />
                                                                        <f:RadioItem Text="启用" Value="enabled" Selected="true" />
                                                                        <f:RadioItem Text="禁用" Value="disabled" />
                                                                    </f:RadioButtonList>
                                                                </Items>
                                                            </f:FormRow>
                                                        </Rows>
                                                    </f:Form>
                                                    <f:Grid ID="Grid2"
                                                        DataIDField="ID" DataTextField="Name" EnableMultiSelect="false"
                                                        PageSize="10" ShowBorder="true" ShowHeader="false"
                                                        AllowPaging="true" IsDatabasePaging="true" runat="server" EnableCheckBoxSelect="True"
                                                        DataKeyNames="ID" OnPageIndexChange="Grid2_PageIndexChange"
                                                        AllowSorting="true" SortField="Name" SortDirection="ASC"
                                                        OnSort="Grid2_Sort">
                                                        <Columns>
                                                            <f:RowNumberField />
                                                            <f:BoundField Width="100px" DataField="Name" SortField="Name" DataFormatString="{0}"
                                                                HeaderText="名称" />
                                                            <f:BoundField ID="BoundField1" runat="server" DataField="Code" SortField="Code" HeaderText="编号"></f:BoundField>
                                                            <f:BoundField ExpandUnusedSpace="True" DataField="Location" HeaderText="地址" />
                                                        </Columns>
                                                    </f:Grid>
                                                </Items>
                                            </f:Panel>
                                        </PopPanel>
                                    </f:DropDownBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow CssStyle="background-color: #b0c4de">
                                <Items>
                                    <f:DropDownList ID="DropDownMainMaterialType" Required="true" Label="主辅材" AutoPostBack="true" OnSelectedIndexChanged="DropDownMainMaterialType_SelectedIndexChanged"
                                        runat="server">
                                        <f:ListItem Text="" Value="0" />
                                        <f:ListItem Text="主材" Value="1" />
   
                                    </f:DropDownList>
                                    <f:DropDownList ID="DropDownMaterialType" Required="true" Label="材料分类"
                                        runat="server">
                                        <f:ListItem Text="" Value="0" />
                                    </f:DropDownList>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                <f:DropDownBox runat="server" Label="适用范围" ID="DropDownBoxBudgetType" DataControlID="CheckBoxListBudgetType" EnableMultiSelect="true" Values="0">
                                <PopPanel>
                                <f:SimpleForm ID="SimpleForm2" BodyPadding="10px" runat="server" AutoScroll="true"
                                    ShowBorder="True" ShowHeader="false" Hidden="true">
                                    <Items>
                                        <f:Label ID="Label2" runat="server" Text="客户需求"></f:Label>
                                        <f:CheckBoxList ID="CheckBoxListBudgetType" ColumnNumber="3" runat="server" BoxFlex="1">
                                            <f:CheckItem Text="清单报价" Value="0" />
                                        </f:CheckBoxList>
                                    </Items>
                                </f:SimpleForm>
                                </PopPanel>
                                </f:DropDownBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                <f:DropDownBox runat="server" Label="适用房间" ID="DropDownBoxRoomType" DataControlID="CheckBoxListRoomType" EnableMultiSelect="true" Values="0">
                                <PopPanel>
                                <f:SimpleForm ID="SimpleForm3" BodyPadding="10px" runat="server" AutoScroll="true"
                                    ShowBorder="True" ShowHeader="false" Hidden="true">
                                    <Items>
                                        <f:Label ID="Label3" runat="server" Text="客户需求"></f:Label>
                                        <f:CheckBoxList ID="CheckBoxListRoomType" ColumnNumber="3" runat="server" BoxFlex="1">
                                            <f:CheckItem Text="客厅" Value="0" />
                                        </f:CheckBoxList>
                                    </Items>
                                </f:SimpleForm>
                                </PopPanel>
                                </f:DropDownBox>
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:TextArea ID="tbxRemark" Height="80px" Label="说明" runat="server" Required="False" ShowRedStar="false" />
                                </Items>
                            </f:FormRow>
                            <f:FormRow>
                                <Items>
                                    <f:CheckBox ID="tbxIsActive" runat="server" Label="启用" />
                                </Items>
                            </f:FormRow>
                        </Rows>
                    </f:Form>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="../../res/third/jqueryuiautocomplete/jquery-ui.js" type="text/javascript"></script>
    <script src="../../res/js/axios.min.js"></script>
    <script src="../../res/js/api.js"></script>
    <script>
        var tbxSalePriceClientID = '<%= tbxSalePrice.ClientID %>';
        var tbxBrand = '<%= tbxBrand.ClientID %>';
        var tbxBrandHidden = '<%= tbxBrandHidden.ClientID %>';

        // 将字符串 val 以逗号空格作为分隔符，分隔成数组
        function split(val) {
            return val.split(/,\s*/);
        }

        // 取得以逗号空格为分隔符的最后一个单词
        // 比如，输入为 "C++, C#, JavaScript" 则输入出 "JavaScript"
        function extractLast(term) {
            return split(term).pop();
        }

        F.ready(function () {
            $('#' + tbxBrand + ' input').bind("keydown", function (event) {
                // 通过 Tab 选择一项时，不会使当前文本框失去焦点
                if (event.keyCode === $.ui.keyCode.TAB &&
                        $(this).data("autocomplete").menu.active) {
                    event.preventDefault();
                }
            }).autocomplete({
                minLength: 0,
                source: function (request, response) {
                    infobasisService.getAjaxInstance.get('/business/brand?term=' + extractLast(request.term))
                      .then(function (res) {
                          response($.map(res.data, function (item) {
                              return {
                                  label: item.label,
                                  value: item.value
                              }
                          }));
                      })
                      .catch(function (error) {
                          console.log(error);
                      });
                },
                search: function () {
                    // 自定义的minLength（如果要限制两个字符才提示，把下面的1改为2即可）
                    var term = extractLast(this.value);
                    if (term.length < 1) {
                        return false;
                    }
                },
                focus: function () {
                    // 阻止某一项获得焦点时，更新文本框的值
                    return false;
                },
                select: function (event, ui) {
                    var $tbxBrand = $("#" + tbxBrand);
                    var tbxBrandF = F(tbxBrand);
                    var tbxBrandHiddenF = F(tbxBrandHidden);
                    this.value = ui.item.label;
                    $tbxBrand.val(ui.item.label);
                    tbxBrandF.setValue(ui.item.label);
                    tbxBrandHiddenF.setValue(ui.item.value);
                    return false;
                    //console.log("Selected: " + ui.item.value + " aka " + ui.item.label);
                    var terms = split(this.value);
                    return false;
                }
            });
        });

        function onNoSalePriceChange(event) {
            var checked = this.isChecked();
            var tbxSalePrice = F(tbxSalePriceClientID);
            tbxSalePrice.setReadonly(!checked);
            if (checked) {
                tbxSalePrice.setValue(0);
            }
        }
    </script>
</asp:Content>