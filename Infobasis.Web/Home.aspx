<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Infobasis.Web.Home"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link type="text/css" rel="stylesheet" href="/res/css/clndr.css" />
    <style>
        body.x-body {
            background-color: #f6f6f6;
        }
        .Home-Top {
            background-color: #f6f6f6;
        }
        .Home-Top-Panel {
            width: 24.5%;
            margin-left: 2px;
            display: inline-block;
            height: 90px;
            border: 1px solid #ccc;
            background-color: #eee;
            color:#fff;
            box-sizing: border-box;
        }
        .Home-Top-Panel-Body{
            height: 68px;
            font-weight: 600;
            line-height: 68px;
            text-align: right;
            font-size: 18px;
        }
        .Home-Top-Panel-Bottom {
            font-weight: bold;
            background-color: #8dbd50;
            padding: 1px;
            height: 20px;
            line-height: 20px;
            overflow: hidden;
            text-align: right;
        }
        .Home-Middle {
            overflow: hidden;
            background-color: #f6f6f6;
        }
        .Home-Middle-Panel {
            background-color: #f6f6f6;
            margin:3px;
        }
        .Home-Middle-Left {
            float: left;
            width: 300px;
            height:450px;
            margin-right: 6px;
        }
        .Home-Middle-Right {
            float: right;
            width: 300px;
            background-color:#fff;
            margin-top:9px;
            margin-left: 6px;
        }
        .Home-Middle-Center {
            margin-left: 306px;
            margin-right: 306px;
            /* height: auto; */
            overflow: hidden;
            height:450px;
        }
        .Home-Middle-Body {
            background-color:#fff;
        }
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
    </style>
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" />
    <div class="Home-Top" style="display:none">
        <div class="Home-Top-Panel"><div class="Home-Top-Panel-Body" style="background-color:#9cd159">￥200,000 第三季度销售额</div><div class="Home-Top-Panel-Bottom">查看详情</div></div>
        <div class="Home-Top-Panel"><div class="Home-Top-Panel-Body" style="background-color:#65a6ff">￥800,000</div><div class="Home-Top-Panel-Bottom" style="background-color:#5b96e6">查看详情</div></div>
        <div class="Home-Top-Panel"><div class="Home-Top-Panel-Body" style="background-color:#41e5c0">￥600,000</div><div class="Home-Top-Panel-Bottom" style="background-color:#3bcfad">查看详情</div></div>
        <div class="Home-Top-Panel"><div class="Home-Top-Panel-Body" style="background-color:#ff5757">￥200,000</div><div class="Home-Top-Panel-Bottom" style="background-color:#e64f4f">查看详情</div></div>
    </div>
    <div class="Home-Middle">
        <div class="Home-Middle-Panel Home-Middle-Left">
            <div class="Home-Middle-Body" style="margin-top:6px;height: 221px;">
                <div class="Home-Middle-Head">公告 (<span style="color:red;font-size:80%">2条未读</span>)</div>
                <div class="Home-Message-Row">公司端午节放假通知</div>
                <div class="Home-Message-Row">公司端午节放假通知</div>
                <div class="Home-Message-Row">公司端午节放假通知</div>
                <div class="Home-Message-Row">公司端午节放假通知</div>
                <div class="Home-Message-Row">公司端午节放假通知</div>
                <div class="Home-Message-Row">公司端午节放假通知</div>
            </div>
            <div class="Home-Middle-Body" style="margin-top:6px;height: 221px;">
                <div class="Home-Middle-Head" style="border:0">设计部</div>
                <f:TabStrip ID="TabStrip2" Height="350px" ShowBorder="true" TabPosition="Top"
                    EnableTabCloseMenu="false" ActiveTabIndex="0" runat="server">
                    <Tabs>
                        <f:Tab ID="Tab3" Title="个人" BodyPadding="5px" Layout="Fit" runat="server">
                            <Items>
                                <f:Label ID="Label2" CssClass="highlight" Text="个人排名" runat="server" />
                            </Items>
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
        </div>
        <div class="Home-Middle-Panel Home-Middle-Right">
            <div class="Home-Middle-Body">
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
        </div> 
        <div class="Home-Middle-Panel Home-Middle-Center">
            <div class="Home-Middle-Body" style="margin-top:6px; height:221px">
                <div class="Home-Middle-Head">会议记录</div>
                <div class="Home-Message-Row">会议记录一</div>
                <div class="Home-Message-Row">会议记录二</div>
                <div class="Home-Message-Row">会议记录三</div>                
            </div>
            <div class="Home-Middle-Body" style="margin-top:6px;">
                <div class="Home-Middle-Head" style="border:0">业务部</div>
                <f:TabStrip ID="TabStrip1" Height="350px" ShowBorder="true" TabPosition="Top"
                    EnableTabCloseMenu="false" ActiveTabIndex="0" runat="server">
                    <Tabs>
                        <f:Tab ID="Tab1" Title="个人" BodyPadding="5px" Layout="Fit" runat="server">
                            <Items>
                                <f:Label ID="Label1" CssClass="highlight" Text="个人排名" runat="server" />
                            </Items>
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

        </div>
    </div>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="../res/js/underscore.js"></script>
    <script src="../res/js/moment.js"></script>
    <script src="../res/js/clndr.min.js"></script>
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
        })

    </script>
</asp:Content>