<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DesignerTarget.aspx.cs" Inherits="Infobasis.Web.Pages.Design.DesignerTarget" 
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="../../res/js/echarts.min.js"></script>
    <script src="../../res/js/axios.min.js"></script>
    <script src="../../res/js/api.js"></script>

    <style>
        .activeGroupPanel {
            background-color: #f5f5f5 !important;
        }
        .activeGroupPanel .f-panel-title-text {
            color:#b74635;
            font-weight:bold !important;
        }
        .activeInputBox input {
            background-color: #ffd800 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="PanelMain" runat="server" />
    <f:Panel ID="PanelMain" runat="server" ShowBorder="false" ShowHeader="false" MarginLeft="6px">
        <Items>
            <f:Label runat="server" ID="labTitle" CssStyle="font-weight:bold" EncodeText="false" Text=""></f:Label>
            <f:TabStrip runat="server" BodyPadding="5px" MarginTop="5px" ShowBorder="false" TabBorderColor="true" TabPlain="true" ActiveTabIndex="0" Height="600px" AutoScroll="true">
                <Tabs>
                    <f:Tab runat="server" Title="目标分析" AutoScroll="true">
                        <Items>
                            <f:Panel runat="server" ShowBorder="false" ShowHeader="false" AutoScroll="true">
                                <Content>
                                    <div id="chart1" style="width: 600px;height:400px;"></div>
                                </Content>
                            </f:Panel>
                        </Items>
                    </f:Tab>
                    <f:Tab runat="server" Title="目标上报" AutoScroll="true">
                        <Items>
                            <f:Panel runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false" Width="300px">
                                <Items>
                                    <f:Label runat="server" CssStyle="font-weight:bold;color:red" Text="" ID="labAll" Label="总目标" LabelWidth="150px" BoxFlex="1"></f:Label>
                                    <f:Label ID="labYuan1" runat="server" CssStyle="font-weight:bold;color:#2b7dbc" Text="万元" BoxFlex="1"></f:Label>
                                </Items>
                            </f:Panel>

                            <f:Panel runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                <Items>
                                    <f:GroupPanel runat="server" ID="GroupPanel1" EnableCollapse="false" Title="第一季度" MarginRight="5px"
                                        BodyPadding="5px" BoxFlex="1">
                                        <Items>
                                            <f:NumberBox ID="tbxInput1" runat="server" Label="2017-01" NextClickControl="tbxInput2" NextFocusControl="tbxInput2" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:NumberBox ID="tbxInput2" runat="server" Label="2017-02" NextClickControl="tbxInput3" NextFocusControl="tbxInput3" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:NumberBox ID="tbxInput3" runat="server" Label="2017-03" NextClickControl="tbxInput4" NextFocusControl="tbxInput4" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:Panel ID="Panel1" runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                            <Items>
                                                <f:Label runat="server" CssStyle="font-weight:bold;color:red" Text="" ID="labGroup1" Label="第一季度目标" LabelWidth="150px" BoxFlex="1"></f:Label>
                                                <f:Label ID="labYuan2" runat="server" CssStyle="font-weight:bold;color:#2b7dbc" Text="万元" BoxFlex="1"></f:Label>
                                            </Items>
                                            </f:Panel>
                                        </Items>
                                    </f:GroupPanel>
                                    <f:GroupPanel ID="GroupPanel2" runat="server" EnableCollapse="false" Title="第二季度" 
                                        MarginRight="5px" BodyPadding="5px" BoxFlex="1">
                                        <Items>
                                            <f:NumberBox ID="tbxInput4" runat="server" Label="2017-01" NextClickControl="tbxInput5" NextFocusControl="tbxInput5" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange2" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:NumberBox ID="tbxInput5" runat="server" Label="2017-02" NextClickControl="tbxInput6" NextFocusControl="tbxInput6" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange2" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:NumberBox ID="tbxInput6" runat="server" Label="2017-03" NextClickControl="tbxInput7" NextFocusControl="tbxInput7" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange2" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:Panel ID="Panel2" runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                            <Items>
                                                <f:Label runat="server" CssStyle="font-weight:bold;color:red" Text="" ID="labGroup2" Label="第二季度目标" LabelWidth="150px" BoxFlex="1"></f:Label>
                                                <f:Label ID="labYuan3" runat="server" CssStyle="font-weight:bold;color:#2b7dbc" Text="万元" BoxFlex="1"></f:Label>
                                            </Items>
                                            </f:Panel>                                        
                                         </Items>
                                    </f:GroupPanel>
                                    <f:GroupPanel ID="GroupPanel3" runat="server" EnableCollapse="false" Title="第三季度" 
                                        MarginRight="5px" BodyPadding="5px" BoxFlex="1">
                                        <Items>
                                            <f:NumberBox ID="tbxInput7" runat="server" Label="2017-01" NextClickControl="tbxInput8" NextFocusControl="tbxInput8" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange3" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:NumberBox ID="tbxInput8" runat="server" Label="2017-02" NextClickControl="tbxInput9" NextFocusControl="tbxInput9" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange3" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:NumberBox ID="tbxInput9" runat="server" Label="2017-03" NextClickControl="tbxInput10" NextFocusControl="tbxInput10" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange3" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:Panel ID="Panel3" runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                            <Items>
                                                <f:Label runat="server" CssStyle="font-weight:bold;color:red" Text="" ID="labGroup3" Label="第三季度目标" LabelWidth="150px" BoxFlex="1">
                                                </f:Label>
                                                <f:Label ID="labYuan4" runat="server" CssStyle="font-weight:bold;color:#2b7dbc" Text="万元" BoxFlex="1"></f:Label>
                                            </Items>
                                            </f:Panel> 
                                        </Items>
                                    </f:GroupPanel>
                                    <f:GroupPanel ID="GroupPanel4" runat="server" EnableCollapse="false" Title="第四季度" 
                                        MarginRight="5px" BodyPadding="5px" BoxFlex="1">
                                        <Items>
                                            <f:NumberBox ID="tbxInput10" runat="server" Label="2017-01" NextClickControl="tbxInput11" NextFocusControl="tbxInput11" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange4" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:NumberBox ID="tbxInput11" runat="server" Label="2017-02" NextClickControl="tbxInput12" NextFocusControl="tbxInput12" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange4" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:NumberBox ID="tbxInput12" runat="server" Label="2017-03" NextClickControl="btnSave" NextFocusControl="btnSave" EnableSuffix="true" Suffix="万元" EnableRound="true" DecimalPrecision="2">
                                                <Listeners>
                                                    <f:Listener Event="change" Handler="ontextBoxChange4" />
                                                </Listeners>
                                            </f:NumberBox>
                                            <f:Panel ID="Panel4" runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                            <Items>
                                                <f:Label runat="server" CssStyle="font-weight:bold;color:red" Text="" ID="labGroup4" Label="第四季度目标" LabelWidth="150px" BoxFlex="1">
                                                </f:Label>
                                                <f:Label ID="labYuan5" runat="server" CssStyle="font-weight:bold;color:#2b7dbc" Text="万元" BoxFlex="1"></f:Label>
                                            </Items>
                                            </f:Panel> 
                                        </Items>
                                    </f:GroupPanel>
                                </Items>
                            </f:Panel>
                            <f:TextArea runat="server" Label="目标备注" ID="tbxRemark" Height="60px" MarginTop="10px"></f:TextArea>
                            <f:Label runat="server" ID="labLastUpdate" Label="最后修改时间" Text=""></f:Label>
                            <f:Button runat="server" ID="btnSave" Text="保存" Icon="PageSave" OnClick="btnSave_Click" MarginTop="10px"></f:Button>
                        </Items>
                    </f:Tab>
                </Tabs>
            </f:TabStrip>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script>
        var labAllClientID = '<%= labAll.ClientID %>';
        var labGroup1ClientID = '<%= labGroup1.ClientID %>';
        var labGroup2ClientID = '<%= labGroup2.ClientID %>';
        var labGroup3ClientID = '<%= labGroup3.ClientID %>';
        var labGroup4ClientID = '<%= labGroup4.ClientID %>';

        var tbxInput1ClientID = '<%= tbxInput1.ClientID %>';
        var tbxInput2ClientID = '<%= tbxInput2.ClientID %>';
        var tbxInput3ClientID = '<%= tbxInput3.ClientID %>';

        var tbxInput4ClientID = '<%= tbxInput4.ClientID %>';
        var tbxInput5ClientID = '<%= tbxInput5.ClientID %>';
        var tbxInput6ClientID = '<%= tbxInput6.ClientID %>';

        var tbxInput7ClientID = '<%= tbxInput7.ClientID %>';
        var tbxInput8ClientID = '<%= tbxInput8.ClientID %>';
        var tbxInput9ClientID = '<%= tbxInput9.ClientID %>';

        var tbxInput10ClientID = '<%= tbxInput10.ClientID %>';
        var tbxInput11ClientID = '<%= tbxInput11.ClientID %>';
        var tbxInput12ClientID = '<%= tbxInput12.ClientID %>';

        var allValues = {};
        var groupValue1 = {};
        var groupValue2 = {};
        var groupValue3 = {};
        var groupValue4 = {};
        F.ready(function () {
            init();
            initChart();
        });

        function init() {
            var tbxInput1 = F(tbxInput1ClientID);
            var tbxInput2 = F(tbxInput2ClientID);
            var tbxInput3 = F(tbxInput3ClientID);

            var tbxInput4 = F(tbxInput4ClientID);
            var tbxInput5 = F(tbxInput5ClientID);
            var tbxInput6 = F(tbxInput6ClientID);

            var tbxInput7 = F(tbxInput7ClientID);
            var tbxInput8 = F(tbxInput8ClientID);
            var tbxInput9 = F(tbxInput9ClientID);

            var tbxInput10 = F(tbxInput10ClientID);
            var tbxInput11 = F(tbxInput11ClientID);
            var tbxInput12 = F(tbxInput12ClientID);

            groupValue1[tbxInput1ClientID] = parseFloat(tbxInput1.getText());
            groupValue1[tbxInput2ClientID] = parseFloat(tbxInput2.getText());
            groupValue1[tbxInput3ClientID] = parseFloat(tbxInput3.getText());

            groupValue2[tbxInput4ClientID] = parseFloat(tbxInput4.getText());
            groupValue2[tbxInput5ClientID] = parseFloat(tbxInput5.getText());
            groupValue2[tbxInput6ClientID] = parseFloat(tbxInput6.getText());

            groupValue3[tbxInput7ClientID] = parseFloat(tbxInput7.getText());
            groupValue3[tbxInput8ClientID] = parseFloat(tbxInput8.getText());
            groupValue3[tbxInput9ClientID] = parseFloat(tbxInput9.getText());

            groupValue4[tbxInput10ClientID] = parseFloat(tbxInput10.getText());
            groupValue4[tbxInput11ClientID] = parseFloat(tbxInput11.getText());
            groupValue4[tbxInput12ClientID] = parseFloat(tbxInput12.getText());

            allValues = $.extend({}, allValues, groupValue1);
            allValues = $.extend({}, allValues, groupValue2);
            allValues = $.extend({}, allValues, groupValue3);
            allValues = $.extend({}, allValues, groupValue4);
        }

        function ontextBoxChange(event) {
            var val = this.getValue();
            var key = this.getAttr("id");
            if (!isNumber(val))
                val = 0;

            allValues[key] = val;
            groupValue1[key] = val;

            var sum = 0;
            for (var k in groupValue1) {
                sum += groupValue1[k];
            };

            countAllValues();
            var labGroup1 = F(labGroup1ClientID);
            labGroup1.setText(sum);
        }

        function ontextBoxChange2(event) {
            var val = this.getValue();
            var key = this.getAttr("id");
            if (!isNumber(val))
                val = 0;

            allValues[key] = val;
            groupValue2[key] = val;

            var sum = 0;
            for (var k in groupValue2) {
                sum += groupValue2[k];
            };

            countAllValues();

            var labGroup2 = F(labGroup2ClientID);
            labGroup2.setText(sum);
        }

        function ontextBoxChange3(event) {
            var val = this.getValue();
            var key = this.getAttr("id");
            if (!isNumber(val))
                val = 0;

            allValues[key] = val;
            groupValue3[key] = val;

            var sum = 0;
            for (var k in groupValue3) {
                sum += groupValue3[k];
            };

            countAllValues();

            var labGroup3 = F(labGroup3ClientID);
            labGroup3.setText(sum);
        }

        function ontextBoxChange4(event) {
            var val = this.getValue();
            var key = this.getAttr("id");
            if (!isNumber(val))
                val = 0;

            allValues[key] = val;
            groupValue4[key] = val;

            var sum = 0;
            for (var k in groupValue4) {
                sum += groupValue4[k];
            };

            countAllValues();

            var labGroup4 = F(labGroup4ClientID);
            labGroup4.setText(sum);
        }

        function countAllValues() {
            var sum = 0;
            for (var k in allValues) {
                var val = allValues[k];

                sum += val;
            };
            var labAll = F(labAllClientID);
            labAll.setText(sum);
        }

        function isNumber(obj) {
            return typeof obj === 'number' && !isNaN(obj)
        }

        function initChart() {
            var currentYear = moment().format('YYYY');
            chart1.showLoading();
            infobasisService.getAjaxInstance.get('/business/' + currentYear + '/myGoals')
              .then(function (response) {
                  var data = response.data;
                  chart1.hideLoading();
                  chart1.setOption({
                      series: [
                          {
                              name: '目标(万元)',
                              type: 'bar',
                              data: data.goalValues,
                              markPoint: {
                                  data: [
                                      { type: 'max', name: '最大值' },
                                      { type: 'min', name: '最小值' }
                                  ]
                              },
                              markLine: {
                                  data: [
                                      { type: 'average', name: '平均值' }
                                  ]
                              }
                          },
                          {
                              name: '业绩(万元)',
                              type: 'bar',
                              data: data.doneValues,
                              markPoint: {
                                  data: [
                                      { type: 'max', name: '最大值' },
                                      { type: 'min', name: '最小值' }
                                  ]
                              },
                              markLine: {
                                  data: [
                                      { type: 'average', name: '平均值' }
                                  ]
                              }
                          }
                      ]
                  });
              })
              .catch(function (error) {
                  console.log(error);
              });
        };

        var chart1 = echarts.init(document.getElementById('chart1'));
        var option = {
            title: {
                text: '我的本年目标与统计',
                subtext: ''
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: ['目标(万元)', '业绩(万元)']
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: { show: true, readOnly: false },
                    magicType: { show: true, type: ['line', 'bar'] },
                    restore: { show: true },
                    saveAsImage: { show: true }
                }
            },
            calculable: true,
            xAxis: [
                {
                    type: 'category',
                    data: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
                }
            ],
            yAxis: [
                {
                    type: 'value'
                }
            ],
            series: [
                {
                    name: '目标',
                    type: 'bar',
                    itemStyle: {
                        normal: {
                            color: 'rgba(51, 152, 219, 0.9)'
                        }
                    },
                    data: [],
                    markPoint: {
                        data: [
                            { type: 'max', name: '最大值' },
                            { type: 'min', name: '最小值' }
                        ]
                    },
                    markLine: {
                        data: [
                            { type: 'average', name: '平均值' }
                        ]
                    }
                },
                {
                    name: '业绩',
                    type: 'bar',
                    itemStyle: {
                        normal: {
                            color: 'rgba(194, 53, 49, 0.9)'
                        }
                    },
                    data: [],
                    markLine: {
                        data: [
                            { type: 'average', name: '平均值' }
                        ]
                    }
                }
            ]
        };
        chart1.setOption(option);

    </script>
</asp:Content>