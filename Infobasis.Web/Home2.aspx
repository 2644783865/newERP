<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home2.aspx.cs" Inherits="Infobasis.Web.Home2"
    MasterPageFile="~/PageMaster/Page.master" %>
<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link type="text/css" rel="stylesheet" href="/res/css/clndr.css" />
    <script src="/res/js/echarts.min.js"></script>
    <script src="/res/js/axios.min.js"></script>
    <script src="/res/js/api.js"></script>
    <style>
        .Home-Middle-Head {
            font-size: 14px;
            font-weight: 600;
            padding: 5px;
            border-bottom: 1px solid #eee;
            color: #555758;
        }
        .Home-Message-Row {
            padding: 5px 3px;
            border-bottom: 1px dotted #eee;
            margin: 2px;
        }

        .badge {
            display: inline-block;
            padding: 0 4px;
            font-size: 12px;
            line-height: 15px;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #999;
            border-radius: 10px;
            margin-left: 5px;
        }

        .badge-danger {
            background-color: #d15b47 !important;
            position: absolute;
            right: -3px;
            top: 50px;
        }

    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" />
    <div data-ng-app="homeApp" data-ng-controller="homeCtrl">  
    <f:Panel ID="PanelMain" runat="server" Height="720px" ShowBorder="false" EnableCollapse="true"
        Layout="HBox" BodyPadding="5px" AutoScroll="true" MarginRight="10px"
        BoxConfigChildMargin="0 5 0 0" ShowHeader="false"
        Title="">
        <Items>
            <f:Panel ID="PanelLeft" Title="左边" Width="350px" runat="server"
                BodyPadding="5px" ShowBorder="false" ShowHeader="false" Layout="VBox" AutoScroll="true">
                <Items>
                    <f:Panel ID="Panel1" runat="server" ShowHeader="false">
                        <Content>
                            <div class="Home-Middle-Body" style="margin-top:6px; height:120px">
                                <div class="Home-Middle-Head">业务提醒</div>
                                <div class="Home-Message-Row">本月新增 <span style="color:red">288</span> 个客户,请及时跟踪.</div>
                                <div class="Home-Message-Row">本日新增 <span style="color:red">24</span> 个客户,请及时跟踪.</div>
                                <div class="Home-Message-Row">本月新增 <span style="color:red">288</span> 个客户,请及时跟踪.</div>                
                            </div>
                        </Content>
                    </f:Panel>
                    <f:Panel ID="Panel2" runat="server" ShowHeader="false" MarginTop="10px">
                        <Content>
                            <div class="Home-Middle-Body" style="margin-top:6px;height: 380px;">
                                <div class="Home-Middle-Head">设计部</div>
                                <f:TabStrip ID="TabStrip2" Height="350px" ShowBorder="false" TabPosition="Top" TabBorderColor="true" TabPlain="true" BodyPadding="5px" MarginTop="5px"
                                    EnableTabCloseMenu="false" ActiveTabIndex="0" runat="server" CssClass="f-tabstrip-theme-simple">
                                    <Tabs>
                                        <f:Tab ID="Tab3" Title="个人" BodyPadding="5px" Layout="Fit" runat="server">
                                            <Content>
                                                <div style="float:left;margin:10px;position:relative;">
                                                    <img class="f-btn-icon" src="/res/images/person/p1.jpeg" style="width: 60px;height: 60px;display: inline-block;">
                                                    <span class="badge badge-danger">1</span>
                                                </div>
                                                <div style="float:left;margin:10px;position:relative;">
                                                    <img class="f-btn-icon" src="/res/images/person/p2.jpeg" style="width: 60px;height: 60px;display: inline-block;">
                                                    <span class="badge badge-danger">2</span>
                                                </div>
                                                <div style="float:left;margin:10px;position:relative;">
                                                    <img class="f-btn-icon" src="/res/images/person/p3.jpeg" style="width: 60px;height: 60px;display: inline-block;">
                                                    <span class="badge badge-danger">3</span>
                                                </div>
                                                <div style="float:left;margin:10px;position:relative;">
                                                    <img class="f-btn-icon" src="/res/images/person/p1.jpeg" style="width: 60px;height: 60px;display: inline-block;">
                                                    <span class="badge badge-danger">4</span>
                                                </div>
                                            </Content>
                                        </f:Tab>
                                        <f:Tab ID="Tab4" Title="部门" BodyPadding="5px"
                                            runat="server">
                                            <Items>
                                                <f:Label ID="Label4" CssClass="highlight" Text="部门排名" runat="server" />
                                            </Items>
                                        </f:Tab>
                                    </Tabs>
                                </f:TabStrip>
                            </div>
                        </Content>
                    </f:Panel>
                </Items>
            </f:Panel>
            <f:Panel ID="PanelCenter" Title="中间" Width="350px"
                runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false" Layout="VBox" AutoScroll="true">
                <Items>
                    <f:Panel ID="Panel3" runat="server" ShowHeader="false">
                        <Content>
                            <div class="Home-Middle-Body" style="margin-top:6px; height:260px">
                                <div class="Home-Middle-Head">目标达成</div>
                                <f:TabStrip ID="TabStrip1" Height="280px" ShowBorder="false" TabPosition="Top" TabBorderColor="true" TabPlain="true" MarginTop="5px"
                                    EnableTabCloseMenu="false" ActiveTabIndex="0" runat="server" CssClass="f-tabstrip-theme-simple">
                                    <Tabs>
                                        <f:Tab Title="本月" runat="server">
                                            <Content>
                                                <div id="myGoalChart" style="width:350px;height:220px;"></div>
                                            </Content>
                                        </f:Tab>
                                        <f:Tab Title="本年" runat="server">
                                            <Content>
                                                <div id="myGoalChartYear" style="width:350px;height:220px;"></div>
                                            </Content>
                                        </f:Tab>
                                    </Tabs>
                                </f:TabStrip>
         
                            </div>
                        </Content>
                    </f:Panel>
                    <f:Panel ID="Panel4" runat="server" ShowHeader="false" MarginTop="10px">
                        <Content>
                            <div class="Home-Middle-Body" style="margin-top:6px;">
                                <div class="Home-Middle-Head">业务部</div>
                                <f:TabStrip ID="TabStrip1" Height="350px" ShowBorder="false" TabPosition="Top" TabBorderColor="true" TabPlain="true" BodyPadding="5px" MarginTop="5px"
                                    EnableTabCloseMenu="false" ActiveTabIndex="0" runat="server" CssClass="f-tabstrip-theme-simple">
                                    <Tabs>
                                        <f:Tab ID="Tab1" Title="个人" BodyPadding="5px" Layout="Fit" runat="server">
                                            <Content>     
                                                <div style="float:left;margin:10px;position:relative;">
                                                    <img class="f-btn-icon" src="/res/images/person/p1.jpeg" style="width: 60px;height: 60px;display: inline-block;">
                                                    <span class="badge badge-danger">1</span>
                                                </div>
                                                <div style="float:left;margin:10px;position:relative;">
                                                    <img class="f-btn-icon" src="/res/images/person/p2.jpeg" style="width: 60px;height: 60px;display: inline-block;">
                                                    <span class="badge badge-danger">2</span>
                                                </div>
                                                <div style="float:left;margin:10px;position:relative;">
                                                    <img class="f-btn-icon" src="/res/images/person/p3.jpeg" style="width: 60px;height: 60px;display: inline-block;">
                                                    <span class="badge badge-danger">3</span>
                                                </div>
                                                <div style="float:left;margin:10px;position:relative;">
                                                    <img class="f-btn-icon" src="/res/images/person/p1.jpeg" style="width: 60px;height: 60px;display: inline-block;">
                                                    <span class="badge badge-danger">4</span>
                                                </div>
                                            </Content>
                                        </f:Tab>
                                        <f:Tab ID="Tab2" Title="部门" BodyPadding="5px"
                                            runat="server">
                                            <Items>
                                                <f:Label ID="Label3" CssClass="highlight" Text="部门排名" runat="server" />
                                            </Items>
                                        </f:Tab>
                                    </Tabs>
                                </f:TabStrip>
                            </div>
                        </Content>
                    </f:Panel>
                </Items>
            </f:Panel>
            <f:Panel ID="PanelRight" Title="右边" BoxFlex="1" runat="server" MarginTop="5px"
                BodyPadding="5px" Margin="0" ShowBorder="true" ShowHeader="false" AutoScroll="true" Layout="VBox">
                <Content>
                    <div class="Home-Middle-Body" style="margin-top:6px;height: 160px;">
                        <div class="Home-Middle-Head">公告 (<span style="color:red;font-size:80%">2条未读</span>)</div>
                        <div class="Home-Message-Row" data-ng-repeat="item in announcementList">{{item.title}}</div>
                    </div>
                    <div class="Home-Middle-Body" style="margin-top:10px">
                        <div class="Home-Middle-Head">工作日程</div>
                        <div>
                            <div id="full-clndr" class="clearfix">
                                <script type="text/template" id="full-clndr-template">
                                <div class="clndr-controls">
                                    <div class="clndr-previous-button">&lt;</div>
                                    <div class="clndr-next-button">&gt;</div>
                                    <div class="current-month">{%= moment(year + '-' + month).format('YYYY年MM月') %}</div>

                                </div>
                                <div class="clndr-grid">
                                    <div class="days-of-the-week clearfix">
                                    {% _.each(daysOfTheWeek, function(day) { %}
                                        <div class="header-day">{%= day %}</div>
                                    {% }); %}
                                    </div>
                                    <div class="days">
                                    {% _.each(days, function(day) { %}
                                        <div class="{%= day.classes %}" id="{%= day.id %}"><span class="day-number">{%= day.day %}</span></div>
                                    {% }); %}
                                    </div>
                                </div>
                                <div class="event-listing">
                                    <div class="event-listing-title">日程</div>
                                    {% _.each(eventsThisMonth, function(event) { %}
                                        <div class="event-item">
                                        <div class="event-item-name">{%= event.title %}</div>
                                        <div class="event-item-location">{%= event.location %} {%= event.date %}</div>
                                        </div>
                                    {% }); %}
                                </div>
                                </script>
                            </div>
                        </div>
                    </div>
                </Content>
            </f:Panel>
        </Items>
    </f:Panel>
    </div>
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="../res/js/underscore.js"></script>
    <script src="../res/js/moment.js"></script>
    <script src="../res/js/clndr.min.js"></script>
    <script src="../res/js/angular.min.js"></script>
    <script src="../res/js/axios.min.js"></script>
    <script src="../res/js/api.js"></script>
    <script>
        var clndr = {};
        F.ready(function () {
            _.templateSettings = {
                interpolate: /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
                evaluate: /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
                escape: /\{%=([\s\S]+?)%\}/g
            }; // excape HTML: {%= <script> %} prints &lt;script&gt;

            // PARDON ME while I do a little magic to keep these events relevant for the rest of time...
            var currentMonth = moment().format('YYYY-MM');
            var nextMonth = moment().add('month', 1).format('YYYY-MM');

            var events = [
              { date: currentMonth + '-' + '10', title: '开会', location: '2楼会议室' },
              { date: currentMonth + '-' + '19', title: '营销活动', location: '光大会展中心' },
              { date: currentMonth + '-' + '23', title: '技术会议', location: '2楼会议室' },
              { date: nextMonth + '-' + '07', title: '营销活动', location: '会展中心' }
            ];

            var currentMonthMoment = moment();
            var currentMonth = currentMonthMoment.format('YYYY-MM');
            var currentYear = currentMonthMoment.format('YYYY');
            var currentMonthDay = currentMonth + "-01";

            clndr = $('#full-clndr').clndr({
                template: $('#full-clndr-template').html(),
                month: 'September',
                startWithMonth: currentMonthDay,
                weekOffset: 1,
                daysOfTheWeek: ['日', '一', '二', '三', '四', '五', '六'],
                events: events,
                forceSixRows: true
            });

            myGoaloption.series[0].data[0].value = 82.5;
            myGoaloptionYear.series[0].data[0].value = 10.5;
            myGoalChart.setOption(myGoaloption, true);
            myGoalChartYear.setOption(myGoaloptionYear, true);
        });

        var myGoalChart = echarts.init(document.getElementById('myGoalChart'));
        var myGoalChartYear = echarts.init(document.getElementById('myGoalChartYear'));

        window.onresize = myGoalChart.resize;
        $('#myGoalChart').resize(myGoalChart.resize);

        var myGoaloption = {
            title: {
                text: '我的目标',
                subtext: ''
            },
            tooltip: {
                formatter: "{a} <br/>{b} : {c}%"
            },
            toolbox: {
                feature: {
                    //restore: {},
                    //saveAsImage: {}
                }
            },
            series: [
                {
                    name: '业务目标',
                    type: 'gauge',
                    radius: '96%',
                    detail: { formatter: '{value}%' },
                    data: [{ value: 50, name: '完成率' }]
                }
            ]
        };

        var myGoaloptionYear = {
            title: {
                text: '我的目标',
                subtext: ''
            },
            tooltip: {
                formatter: "{a} <br/>{b} : {c}%"
            },
            toolbox: {
                feature: {
                    //restore: {},
                    //saveAsImage: {}
                }
            },
            series: [
                {
                    name: '业务目标',
                    type: 'gauge',
                    radius: '96%',
                    detail: { formatter: '{value}%' },
                    data: [{ value: 50, name: '完成率' }]
                }
            ]
        };

        var app = angular.module('homeApp', []);
        app.controller('homeCtrl', function ($scope) {
            $scope.announcementList = [{ title: '加载中...', publishDate: '', announceTypeName: '', isRead: true }];
            init();

            function init() {
                infobasisService.getAjaxInstance.get('/home/announcement?num=4')
                  .then(function (response) {
                      var data = response.data;
                      $scope.announcementList = data;
                      $scope.$apply();
                  })
                  .catch(function (error) {
                      console.log(error);
                  });
            }
        });

    </script>
</asp:Content>