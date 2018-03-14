<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Meeting_Form.aspx.cs" Inherits="Infobasis.Web.Pages.OA.Meeting_Form"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <meta name="sourcefiles" content="~/Handler/autocomplete/search.ashx" />
    <link rel="stylesheet" href="../../res/third/jqueryuiautocomplete/jquery-ui.css" />
    <style>
        .ui-autocomplete {
            border-width: 1px;
            border-style: solid;
        }
        .ui-menu-item.ui-state-focus {
            border-width: 1px;
            border-style: solid;
        }
        .ui-autocomplete-loading {
            background: white url('../../res/images/ui-anim_basic_16x16.gif') right center no-repeat;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" />
    <f:Toolbar ID="Toolbar2" runat="server" Layout="HBox">
        <Items>     
            <f:LinkButton runat="server" Text="返回" OnClientClick="window.open('/Pages/OA/Meeting.aspx','_self');"></f:LinkButton>
            <f:Button ID="Button1" IconFont="Save" MarginLeft="20px" Text="保存信息" ValidateForms="SimpleForm1" ValidateMessageBox="false" OnClick="btnSave_Click" runat="server">
            </f:Button>
            <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" MarginLeft="20px" Icon="SystemSaveClose"
                OnClick="btnSaveClose_Click" runat="server" Text="保存后关闭">
            </f:Button>
            <f:HiddenField runat="server" ID="tbxMeetingID"></f:HiddenField>
        </Items>                
    </f:Toolbar>
    <f:SimpleForm runat="server" ID="SimpleForm1" ShowBorder="false" ShowHeader="true">
        <Items>
            <f:Panel ID="Panel5" runat="server" Width="950px" ShowBorder="false" EnableCollapse="true"
                Layout="HBox" AutoScroll="true" BodyPadding="5"
                ShowHeader="false" Title="会议记录">
                <Items>
                    <f:Panel ID="Panel1" Title="面板1" BoxFlex="1" runat="server" MarginRight="5px"
                        BodyPadding="5px" ShowBorder="true" ShowHeader="false" Layout="VBox">
                        <Items>
                            <f:TextBox ID="tbxTopic" runat="server" Label="会议标题" ShowRedStar="true" Required="true" NextClickControl="DropDownMeetingType"></f:TextBox>
                            <f:DatePicker ID="tbxMeetingDate" runat="server" Label="会议日期" EnableEdit="false"></f:DatePicker>
                            <f:Panel runat="server" Layout="Column" ShowHeader="false" ShowBorder="false">
                                <Items>
                                    <f:DropDownList ID="DropDownMeetingType" Label="会议类型" ShowRedStar="true" Required="true" runat="server" NextClickControl="tbxContent"
                                            ColumnWidth="50%"  Margin="0 5 0 0">
                                        <f:ListItem Text="部门周晨会" Value="部门周晨会" Selected="true" />
                                        <f:ListItem Text="部门周会" Value="部门周会" />
                                        <f:ListItem Text="公司月会" Value="公司月会" />
                                        <f:ListItem Text="后勤月会" Value="后勤月会" />
                                        <f:ListItem Text="技术升级" Value="技术升级" />
                                        <f:ListItem Text="企划会议" Value="企划会议" />
                                        <f:ListItem Text="主管会议" Value="主管会议" />
                                    </f:DropDownList>
                                    <f:TextBox runat="server" ID="tbxHostUserDisplayName" ColumnWidth="50%" Label="主持人" ShowRedStar="true" Required="true" EmptyText="输入姓名直接查找"></f:TextBox>
                                    </Items>
                            </f:Panel>
                            <f:Panel runat="server">
                                <Items>
                                    <f:TextBox runat="server" ID="tbxAttendanceIDs" Label="参会人员" ShowRedStar="true" Required="true" EmptyText="输入姓名直接查找"></f:TextBox>                  
                                </Items>
                            </f:Panel>

                            <f:HtmlEditor runat="server" Label="会议主题" ID="tbxContentHtml" MarginTop="20px" Editor="UMEditor" BoxFlex="1" BasePath="~/third-party/res/umeditor/" Height="200px">
                            </f:HtmlEditor>
                            <f:Label runat="server" EncodeText="false" Label="<span style='font-weight:bold;font-size:18px;'>会议机制</span>"></f:Label>
                            <f:Label runat="server" EncodeText="false" LabelSeparator="" LabelWidth="500px" Label="<ul style='font-size:16px;'><li>一、每周日上午10点技术升级会议；</li><li>二、迟到者中午饭包干；</li><li>三、有事参加不了需提前一天向主持人请假；</li></ul>"></f:Label>
                        </Items>
                    </f:Panel>
                    <f:Panel ID="Panel2" Title="面板2" BoxFlex="1"
                        runat="server" BodyPadding="5px" ShowBorder="true" ShowHeader="false">
                        <Items>
                            <f:Label runat="server" Label="本周工作安排"></f:Label>
                            <f:Form ID="Form1" Layout="VBox"
                                ShowBorder="false" ShowHeader="false" runat="server" AutoScroll="true"
                                EnableTableStyle="true" MessageTarget="Qtip">
                                <Items>
                                    <f:TextArea ID="tbxTaskContent" BoxFlex="1" MinHeight="100px" Required="false" ShowRedStar="true" Label="工作内容" runat="server">
                                    </f:TextArea>
                                        <f:Panel ID="Panel3" Layout="Column" ShowHeader="false" ShowBorder="false" runat="server">
                                            <Items>
                                                <f:TextBox ID="TextBox2" Label="完成人" Margin="0 5 0 0" Required="false" ShowRedStar="true" ColumnWidth="50%" runat="server">
                                                </f:TextBox>
                                                <f:DatePicker runat="server" Label="完成日期" Required="false" ShowRedStar="true" ColumnWidth="50%" EnableEdit="false"></f:DatePicker>
                                            </Items>
                                        </f:Panel>
                                </Items>
                            </f:Form>
                        </Items>
                    </f:Panel>
                </Items>
            </f:Panel>
        </Items>
    </f:SimpleForm>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="../../res/third/jqueryuiautocomplete/jquery-ui.js" type="text/javascript"></script>
    <script type="text/javascript">
        var textbox1ID = '<%= tbxAttendanceIDs.ClientID %>';

        F.ready(function () {

            // 将字符串 val 以逗号空格作为分隔符，分隔成数组
            function split(val) {
                return val.split(/,\s*/);
            }

            // 取得以逗号空格为分隔符的最后一个单词
            // 比如，输入为 "C++, C#, JavaScript" 则输入出 "JavaScript"
            function extractLast(term) {
                return split(term).pop();
            }

            $('#' + textbox1ID + ' input').bind("keydown", function (event) {
                // 通过 Tab 选择一项时，不会使当前文本框失去焦点
                if (event.keyCode === $.ui.keyCode.TAB &&
                        $(this).data("autocomplete").menu.active) {
                    event.preventDefault();
                }
            }).autocomplete({
                minLength: 0,
                source: function (request, response) {
                    $.getJSON("../../Handler/SearchHandler.ashx", {
                        term: extractLast(request.term)
                    }, response);
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
                    var terms = split(this.value);
                    // 移除用户正在输入项
                    terms.pop();
                    // 添加用户选择的项
                    terms.push(ui.item.value);
                    // 添加占位符，确保字符串的最后以逗号空格结束
                    terms.push("");
                    this.value = terms.join(", ");
                    return false;
                }
            });

        });

    </script>
</asp:Content>
