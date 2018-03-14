<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="my97_format.aspx.cs" Inherits="FineUIPro.Examples.aspnet.my97_format" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:SimpleForm ID="SimpleForm1" runat="server" Title="表单" Width="500" BodyPadding="5px">
            <Items>
                <f:RadioButtonList runat="server" ColumnNumber="2" Width="250px" ID="rblType" Label="趋势类型">
                    <f:RadioItem Text="小时" Value="1" Selected="true" />
                    <f:RadioItem Text="日" Value="2" />
                </f:RadioButtonList>
                <f:TriggerBox ID="tbxStartDate" Required="true" ShowRedStar="True" Label="开始日期" EmptyText="请选择开始日期" EnableEdit="false" TriggerIcon="Date"
                    runat="server">
                </f:TriggerBox>
                <f:TriggerBox ID="tbxEndDate" Required="true" ShowRedStar="True" Label="结束日期" EmptyText="请选择结束日期" EnableEdit="false" TriggerIcon="Date"
                    runat="server">
                </f:TriggerBox>
                <f:Button ID="btnSubmit" runat="server" ValidateForms="SimpleForm1" Text="提交表单"
                    OnClick="btnSubmit_Click">
                </f:Button>
            </Items>
        </f:SimpleForm>
        <f:Label ID="labResult" ShowLabel="false" EncodeText="false" runat="server">
        </f:Label>
    </form>
    <script src="../res/my97/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">

        var tbxStartDateClientID = '<%= tbxStartDate.ClientID %>';
        var tbxEndDateClientID = '<%= tbxEndDate.ClientID %>';
        var rblTypeClientID = '<%= rblType.ClientID %>';

        F.ready(function () {

            var startDateCmp = F(tbxStartDateClientID);
            var endDateCmp = F(tbxEndDateClientID);
            var typeCmp = F(rblTypeClientID);

            function getDateFormat() {
                var format = 'yyyy-MM-dd';
                if (typeCmp.getValue() === '1') {
                    format = 'yyyy-MM-dd HH';
                }
                return format;
            }

            startDateCmp.onTriggerClick = function () {
                WdatePicker({
                    el: tbxStartDateClientID + '-inputEl',
                    dateFmt: getDateFormat(),
                    isShowWeek: true,
                    maxDate: endDateCmp.getValue(),
                    onpicked: function () {
                        // 确认选择后，执行触发器输入框的客户端验证
                        startDateCmp.validate();
                    }
                });
            };

            endDateCmp.onTriggerClick = function () {
                WdatePicker({
                    el: tbxEndDateClientID + '-inputEl',
                    dateFmt: getDateFormat(),
                    isShowWeek: true,
                    minDate: startDateCmp.getValue(),
                    onpicked: function () {
                        // 确认选择后，执行触发器输入框的客户端验证
                        endDateCmp.validate();
                    }
                });
            };

            typeCmp.on('change', function () {
                var type = this.getValue();

                // 由于没有从字符串到日期类型的转换函数（'2015-10-14 12' -> Date），所以这里暂时用空格分割日期和时间
                var startDate = startDateCmp.getValue();
                var endDate = endDateCmp.getValue();
                var startDateParts = startDate.split(' ');
                var endDateParts = endDate.split(' ');

                if (type === '1') {
                    startDateCmp.setValue(startDateParts[0] + ' 00');
                    endDateCmp.setValue(endDateParts[0] + ' 23');
                } else {
                    startDateCmp.setValue(startDateParts[0]);
                    endDateCmp.setValue(endDateParts[0]);
                }
            });
        });
    </script>
</body>
</html>
