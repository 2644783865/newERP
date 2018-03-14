<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientAdd.aspx.cs" Inherits="Infobasis.Web.Pages.Business.ClientAdd" 
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" href="../../res/third/jqueryuiautocomplete/jquery-ui.css" />
    <style>
        fieldset.ClientDetailsGroup .f-panel-body {
            background-color: #fff8f8;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="PanelMain" runat="server" />
    <f:Panel runat="server" ID="PanelMain" ShowHeader="false" ShowBorder="false" Width="850px" AutoScroll="true">
        <Items>
            <f:TabStrip ID="TabStrip1" BodyPadding="5px" MarginTop="5px" ShowBorder="true" TabBorderColor="true" TabPlain="true" ActiveTabIndex="0"
            runat="server" Height="530px" Width="820px" AutoScroll="true" CssClass="f-tabstrip-theme-simple">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" ToolbarAlign="Right" Position="Bottom">
                    <Items>
                        <f:Button ID="btnSave" MarginBottom="6px" IconFont="Save" Text="保存信息" ValidateForms="Form1" ValidateMessageBox="false" OnClick="btnSave_Click" runat="server">
                        </f:Button>
                    </Items>                
                </f:Toolbar>
            </Toolbars>
            <Tabs>
                <f:Tab ID="Tab1" Title="客户信息" runat="server">
                    <Items>
                        <f:Panel ID="Panel1" runat="server"
                            EnableCollapse="False" Title="" Layout="Fit" ShowBorder="false" ShowHeader="false" AutoScroll="true">
                            <Items>
                                <f:Form ID="Form1" LabelAlign="Right" MessageTarget="Qtip"
                                        BodyPadding="5px" ShowBorder="false" ShowHeader="false" runat="server" AutoScroll="false">
                                    <Items>
                                        <f:GroupPanel ID="GroupPanel1" Layout="Anchor" Title="基本信息" runat="server">
                                            <Items>
                                                <f:Panel ID="Panel2" Layout="HBox" ShowHeader="false" ShowBorder="false" runat="server">
                                                    <Items>
                                                        <f:TextBox ID="tbxProjectName" Label="客户姓名" ShowLabel="true" FocusOnPageLoad="true" CssClass="marginr" Required="true" ShowRedStar="true" BoxFlex="1" runat="server" NextFocusControl="tbxTel" NextClickControl="tbxTel">
                                                        </f:TextBox>
                                                        <f:TextBox ID="tbxTel" ShowLabel="true" Required="true" Label="联系电话" ShowRedStar="true" BoxFlex="1" Regex="^[1]+[358]+\d{9}$" runat="server" NextClickControl="tbxQQ" NextFocusControl="tbxQQ">
                                                        </f:TextBox>
                                                        <f:Label ID="labUserName" runat="server" Label="录入人" BoxFlex="1"></f:Label>
                                                    </Items>
                                                </f:Panel>
                                                <f:Panel ID="Panel7" Layout="HBox" ShowHeader="false" ShowBorder="false" runat="server">
                                                    <Items>
                                                        <f:TextBox ID="tbxQQ" Label="QQ" ShowLabel="true" CssClass="marginr" BoxFlex="1" runat="server" NextFocusControl="tbxWeChat" NextClickControl="tbxWeChat">
                                                        </f:TextBox>
                                                        <f:TextBox ID="tbxWeChat" ShowLabel="true" Label="微信" BoxFlex="1" runat="server" NextFocusControl="tbxEmail" NextClickControl="tbxEmail">
                                                        </f:TextBox>
                                                        <f:TextBox ID="tbxEmail" Required="false" Label="电子邮箱" RegexPattern="EMAIL" NextFocusControl="ddlGender" NextClickControl="ddlGender"
                                                            RegexMessage="请输入有效的邮箱地址" BoxFlex="1" runat="server">
                                                        </f:TextBox>
                                                    </Items>
                                                </f:Panel>
                                                <f:Panel ID="Panel4" Layout="HBox" BoxConfigAlign="Stretch" ShowHeader="false" ShowBorder="false" runat="server">
                                                    <Items>
                                                        <f:RadioButtonList ID="ddlGender" Label="性别" runat="server" BoxFlex="1">
                                                            <f:RadioItem Text="男" Value="男" />
                                                            <f:RadioItem Text="女" Value="女" />
                                                        </f:RadioButtonList>
                                                        <f:DropDownList ID="DropDownListClientAge" Label="年龄段" BoxFlex="1" CssClass="marginr" Required="false" ShowRedStar="false" runat="server">
                                                            <f:ListItem Text="20" Value="20" />
                                                            <f:ListItem Text="30" Value="30" />
                                                        </f:DropDownList>
                                                        <f:DropDownList ID="DropDownClientFrom" Label="客户来源" BoxFlex="1" CssClass="marginr" runat="server">
                                                            <f:ListItem Text="来源一" Value="1" />
                                                            <f:ListItem Text="来源二" Value="2" />
                                                        </f:DropDownList>
                                                    </Items>
                                                </f:Panel>
                                                <f:Panel ID="Panel3" Layout="HBox" BoxConfigAlign="Stretch" ShowHeader="false" ShowBorder="false" runat="server">
                                                    <Items>
                                                        <f:TextBox ID="tbxDecorationAddress" Required="true" ShowRedStar="true" Label="装修地址" BoxFlex="2" runat="server">
                                                        </f:TextBox>
                                                        <f:TextBox ID="tbxHousesName" Label="楼盘信息"  BoxFlex="1" Required="false" ShowRedStar="false" runat="server">
                                                        </f:TextBox>
                                                        <f:HiddenField ID="tbxHouseInfoID" runat="server"></f:HiddenField>
                                                    </Items>
                                                </f:Panel>
                                                <f:Panel Layout="HBox" BoxConfigAlign="Stretch" ShowHeader="false" ShowBorder="false" runat="server">
                                                    <Items>
                                                        <f:DatePicker ID="dpDeliverHouseDate" BoxFlex="1" Label="交房日期" EmptyText="请选择日期" runat="server" EnableEdit="false" ShowRedStar="false" Required="false"></f:DatePicker>
                                                        <f:NumberBox ID ="tbxBuiltupArea" runat="server" Label="面积" DecimalPrecision="1" NoNegative="true" BoxFlex="1" EnableSuffix="true" Suffix="平方" ></f:NumberBox>
                                                        <f:NumberBox ID ="tbxBudget" runat="server" Label="预算" DecimalPrecision="0" NoNegative="true" NoDecimal="true" BoxFlex="1" EnableSuffix="true" Suffix="万元" ></f:NumberBox>
                                                    </Items>
                                                </f:Panel>
                                                <f:Panel Layout="HBox" BoxConfigAlign="Stretch" ShowHeader="false" ShowBorder="false" runat="server">
                                                    <Items>
                                                        <f:DatePicker ID="dpPlanStartDate" BoxFlex="1" Label="开工日期" EmptyText="请选择日期" runat="server" EnableEdit="false" ShowRedStar="false" Required="false"></f:DatePicker>
                                                        <f:DatePicker ID="dpPlanEndDate" BoxFlex="1" Label="结束日期" EmptyText="请选择日期" runat="server" EnableEdit="false" ShowRedStar="false" Required="false"></f:DatePicker>
                                                        <f:Label runat="server" Label="工期" BoxFlex="1" Hidden="false" ></f:Label>
                                                    </Items>
                                                </f:Panel>
                                                <f:Panel Layout="HBox" BoxConfigAlign="Stretch" ShowHeader="false" ShowBorder="false" runat="server">
                                                    <Items>
                                                        <f:DropDownList ID="DropDownProvince" Label="区域" BoxFlex="1" Required="false" ShowRedStar="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownProvince_SelectedIndexChanged">
                                                            <f:ListItem Text="浦东" Value="浦东" />
                                                            <f:ListItem Text="青浦" Value="青浦" />
                                                        </f:DropDownList>
                                                        <f:DropDownBox runat="server" BoxFlex="1" ID="ddbDesignerDept" Label="设计部门" 
                                                            AutoShowClearIcon="true" OnTextChanged="ddbDesignerDept_TextChanged" AutoPostBack="true">
                                                            <PopPanel>
                                                                <f:Grid ID="gridDept" ShowBorder="true" ShowHeader="false" runat="server"
                                                                    DataKeyNames="ID" Hidden="true" Width="550px" ShowGridHeader="false"
                                                                    EnableTree="true" TreeColumn="Name" DataIDField="ID" DataParentIDField="ParentID" DataTextField="Name"
                                                                    ExpandAllTreeNodes="true"
                                                                    EnableRowLines="false" EnableAlternateRowColor="false">
                                                                    <Columns>
                                                                        <f:BoundField ColumnID="Name" DataField="Name" HeaderText="部门名称" ExpandUnusedSpace="true" />
                                                                    </Columns>
                                                                </f:Grid>
                                                            </PopPanel>
                                                        </f:DropDownBox>

                                                        <f:DropDownBox runat="server" BoxFlex="1" ID="ddbDesigner" Label="设计师" AutoShowClearIcon="true" Enabled="false">
                                                            <PopPanel>
                                                                <f:Grid ID="gridDesigner" ShowBorder="true" ShowHeader="false" runat="server"
                                                                    DataKeyNames="ID" Hidden="true" Width="550px" ShowGridHeader="false"
                                                                    DataIDField="ID" DataTextField="ChineseName"
                                                                    EnableRowLines="false" EnableAlternateRowColor="false">
                                                                    <Columns>
                                                                        <f:BoundField ColumnID="ChineseName" DataField="ChineseName" HeaderText="姓名" ExpandUnusedSpace="true" />
                                                                    </Columns>
                                                                </f:Grid>
                                                            </PopPanel>
                                                        </f:DropDownBox>

                                                    </Items>
                                                </f:Panel>
                                            </Items>
                                        </f:GroupPanel>
                                        <f:GroupPanel ID="GroupPanel2" Layout="Anchor" Title="详细信息" runat="server" CssClass="ClientDetailsGroup" CssStyle="margin-top:20px; background-color: #ddd">
                                            <Items>
                                                <f:Panel ID="Panel5" Layout="HBox" BoxConfigAlign="Stretch" ShowHeader="false" ShowBorder="false" runat="server">
                                                    <Items>
                                                        <f:DropDownList ID="DropDownHouseStructType" Label="户型" BoxFlex="1" CssClass="marginr" LabelWidth="80px" Required="false" ShowRedStar="false" runat="server">
                                                            <f:ListItem Text="一室一厅" Value="一室一厅" />
                                                        </f:DropDownList>

                                                        <f:DropDownList ID="DropDownDecorationStype" Label="装修风格" BoxFlex="1" CssClass="marginr" LabelWidth="80px" Required="false" ShowRedStar="false" runat="server">
                                                            <f:ListItem Text="现代简约" Value="1" />
                                                        </f:DropDownList>
                                                        <f:DropDownList ID="DropDownDecorationType" Label="装修类型" BoxFlex="1" CssClass="marginr" LabelWidth="80px" Required="false" ShowRedStar="false" runat="server">
                                                            <f:ListItem Text="精装" Value="精装" />
                                                        </f:DropDownList>
                                                    </Items>
                                                </f:Panel>
                                                <f:Panel Layout="HBox" BoxConfigAlign="Stretch" ShowHeader="false" ShowBorder="false" runat="server">
                                                    <Items>
                                                        <f:DropDownList ID="DropDownHouseType" Label="房屋类型" BoxFlex="1" CssClass="marginr" LabelWidth="80px" Required="false" ShowRedStar="false" runat="server">
                                                            <f:ListItem Text="别墅" Value="别墅" />
                                                            <f:ListItem Text="错层房" Value="错层房" />
                                                        </f:DropDownList>
                                                        <f:DropDownBox runat="server" ID="DropDownBoxClientNeed" BoxFlex="1" DataControlID="CheckBoxListClientNeed" EnableMultiSelect="true" Values="装修">
                                                        <PopPanel>
                                                        <f:SimpleForm ID="SimpleForm2" BodyPadding="10px" runat="server" AutoScroll="true"
                                                            ShowBorder="True" ShowHeader="false" Hidden="true">
                                                            <Items>
                                                                <f:Label ID="Label1" runat="server" Text="客户需求"></f:Label>
                                                                <f:CheckBoxList ID="CheckBoxListClientNeed" ColumnNumber="3" runat="server" BoxFlex="1">
                                                                    <f:CheckItem Text="设计" Value="设计" />
                                                                    <f:CheckItem Text="软装" Value="软装" />
                                                                    <f:CheckItem Text="园林" Value="园林" />
                                                                    <f:CheckItem Text="装修" Value="装修" Selected="true" />
                                                                    <f:CheckItem Text="风水" Value="风水" />
                                                                </f:CheckBoxList>
                                                            </Items>
                                                        </f:SimpleForm>
                                                        </PopPanel>
                                                        </f:DropDownBox>
                                                        <f:DropDownList ID="DropDownDecorationColor" Label="色彩偏向" BoxFlex="1" CssClass="marginr" Required="false" ShowRedStar="false" runat="server">
                                                            <f:ListItem Text="暖黄" Value="暖黄" />
                                                            <f:ListItem Text="暖黄2" Value="暖黄2" />
                                                        </f:DropDownList>
                                                    </Items>
                                                </f:Panel>
                                                <f:Panel ID="Panel6" Layout="HBox" BoxConfigAlign="Stretch" ShowHeader="false" ShowBorder="false" runat="server">
                                                    <Items>
                                                        <f:DropDownList ID="DropDownClientStatus" runat="server" Label="客户状态" BoxFlex="1">
                                                            <f:ListItem Text="潜在" Value="潜在" />
                                                        </f:DropDownList>
                                                        <f:DropDownList ID="DropDownPackageName" Label="套餐" BoxFlex="1" CssClass="marginr" Required="false" ShowRedStar="false" runat="server">
                                                            <f:ListItem Text="788" Value="788" />
                                                            <f:ListItem Text="988" Value="988" />
                                                        </f:DropDownList>
                                                    </Items>
                                                </f:Panel>
                                                <f:Panel Layout="HBox" BoxConfigAlign="Stretch" ShowHeader="false" ShowBorder="false" runat="server">
                                                    <Items>
                                                        <f:TextArea ID="tbxRemark" Label="备注" runat="server" BoxFlex="1" Height="30px" AutoGrowHeight="true" AutoGrowHeightMin="100" AutoGrowHeightMax="200"></f:TextArea>
                                                        <f:Label ID="labCreateDate" runat="server" Label="录入时间" BoxFlex="1"></f:Label>
                                                    </Items>
                                                </f:Panel>
                                            </Items>
                                        </f:GroupPanel>
                                    </Items>
                                </f:Form>
                            </Items>
                        </f:Panel>
                    </Items>
                </f:Tab>
                <f:Tab ID="Tab2" Title="回访计划" BodyPadding="5px" runat="server">
                    <Items>
                        <f:Form runat="server" Layout="VBox" ShowBorder="false" ShowHeader="false">
                            <Items>
                                <f:Panel runat="server" ShowBorder="false" ShowHeader="false">
                                    <Items>
                                        <f:DatePicker runat="server" Label="回访开始日期" EnableEdit="false" Width="220px"></f:DatePicker>
                                    </Items>
                                </f:Panel>
                                <f:Panel runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                    <Items>
                                        <f:Label runat="server" Label="客服几天回访一次"  Width="180px" LabelWidth="180px"></f:Label>
                                        <f:NumberBox runat="server" EnableSuffix="true" Suffix=" 天" Width="100px"></f:NumberBox>
                                        <f:Label runat="server" LabelWidth="200px" Text=" ,天数为0，则不提醒。"></f:Label>
                                    </Items>
                                </f:Panel>
                                <f:Panel ID="Panel8" runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                    <Items>
                                        <f:Label ID="Label2" runat="server" Label="设计师几天回访一次"  Width="180px" LabelWidth="180px"></f:Label>
                                        <f:NumberBox ID="NumberBox1" runat="server" EnableSuffix="true" Suffix=" 天" Width="100px"></f:NumberBox>
                                        <f:Label ID="Label4" runat="server" LabelWidth="200px" Text=" ,天数为0，则不提醒。"></f:Label>
                                    </Items>
                                </f:Panel>
                                <f:Panel ID="Panel9" runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                    <Items>
                                        <f:Label ID="Label3" runat="server" Label="售后几天回访一次"  Width="180px" LabelWidth="180px"></f:Label>
                                        <f:NumberBox ID="NumberBox2" runat="server" EnableSuffix="true" Suffix=" 天" Width="100px"></f:NumberBox>
                                        <f:Label ID="Label5" runat="server" LabelWidth="200px" Text=" ,天数为0，则不提醒。"></f:Label>
                                    </Items>
                                </f:Panel>
                            </Items>
                        </f:Form>
                    </Items>
                </f:Tab>
            </Tabs>
        </f:TabStrip>
    </Items>
</f:Panel>
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="../../res/third/jqueryuiautocomplete/jquery-ui.js" type="text/javascript"></script>
    <script src="../../res/js/axios.min.js"></script>
    <script src="../../res/js/api.js"></script>
    <script>

        var emailBoxID = '<%= tbxEmail.ClientID %>';
        var form1ClientID = '<%= Form1.ClientID %>';
        var tbxHousesName = '<%= tbxHousesName.ClientID %>';
        var tbxHouseInfoID = '<%= tbxHouseInfoID.ClientID %>';

        function onCloseClick() {
            window.location.reload();
        }

        F.ready(function () {
            var availableTags = [
                "qq.com",
                "163.com",
                "gmail.com",
                "outlook.com",
                "126.com",
                "sina.com",
                "yahoo.com",
                "sohu.com",
                "foxmail.com",
                "live.com",
                "mail.ustc.edu.cn"];


            function getFullEmails(name) {
                var emails = [];
                for (var i = 0, count = availableTags.length; i < count; i++) {
                    emails.push(name + "@" + availableTags[i]);
                }
                return emails;
            }

            $('#' + emailBoxID + ' input').autocomplete({
                source: function (request, response) {
                    if (request.term.indexOf('@') === -1) {
                        response(getFullEmails(request.term));
                    }
                }
            });

            // 将字符串 val 以逗号空格作为分隔符，分隔成数组
            function split(val) {
                return val.split(/,\s*/);
            }

            // 取得以逗号空格为分隔符的最后一个单词
            // 比如，输入为 "C++, C#, JavaScript" 则输入出 "JavaScript"
            function extractLast(term) {
                return split(term).pop();
            }

            $('#' + tbxHousesName + ' input').bind("keydown", function (event) {
                // 通过 Tab 选择一项时，不会使当前文本框失去焦点
                if (event.keyCode === $.ui.keyCode.TAB &&
                        $(this).data("autocomplete").menu.active) {
                    event.preventDefault();
                }
            }).autocomplete({
                minLength: 0,
                source: function (request, response) {
                    infobasisService.getAjaxInstance.get('/business/houseName?term=' + extractLast(request.term))
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

                    //$.getJSON("../../Handler/SearchHandler.ashx", {
                    //    term: extractLast(request.term)
                    //}, response);
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
                    var $tbxHousesName = $("#" + tbxHousesName);
                    var houseName = F(tbxHousesName);
                    var tbxHouseInfo = F(tbxHouseInfoID);
                    this.value = ui.item.label;
                    $tbxHousesName.val(ui.item.label);
                    houseName.setValue(ui.item.label);
                    tbxHouseInfo.setValue(ui.item.value);
                    return false;
                    //console.log("Selected: " + ui.item.value + " aka " + ui.item.label);
                    var terms = split(this.value);
                    //var ids = split($tbxHousesName.val());
                    // 移除用户正在输入项
                    //terms.pop();
                    // 添加用户选择的项
                    //terms.push(ui.item.label);
                    //ids.push(ui.item.value);
                    // 添加占位符，确保字符串的最后以逗号空格结束
                    //terms.push("");
                    //ids.push("");
                    //this.value = terms.join(", ");
                    //$tbxHousesName.val(ids.join(", "));
                    return false;
                }
            });

        });

/*
        F.beforeUnload(function () {
            var form = F(form1ClientID);
            // 表单数据改变了
            if (form.isDirty()) {
                return '当前表单数据已经被修改，确认放弃修改？';
            }
        });
*/

    </script>
</asp:Content>