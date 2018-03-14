<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Org.aspx.cs" Inherits="Infobasis.Web.Pages.HR.Org"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link type="text/css" rel="stylesheet" href="/res/css/jquery.orgchart.css" />
    <style>
        .hide {display:none;}

        div.orgChart {
            border: 0px solid #cccccc; 
            background-color: #fff;
            margin: 0px; 
            padding: 0px;
        }

        .long-name {
            font-size: 12px;
        }
        div.orgChart div.node {
            background-color: #d7e9f6;
            /* color: #fff; */
            border: 1px solid #e5e5e5;
            padding-top: 6px;
        }
        div.orgChart div.hasChildren {
            background-color: #6fb3e0;
        }

        div.orgChart h2 p {
            font-weight: normal;
            color: #555;
            font-size: 95%;
        }
    </style>
    <script src="../../res/js/jquery-1.11.1.min.js"></script>
    <script src="../../res/js/echarts.min.js"></script>
    <script src="../../res/js/axios.min.js"></script>
    <script src="../../res/js/api.js"></script>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
        ShowBorder="false" ShowHeader="false" Layout="Fit">
        <Items>
            <f:TabStrip runat="server" AutoScroll="true" ShowBorder="false" TabPosition="Top" TabBorderColor="true" TabPlain="true" BodyPadding="5px" MarginTop="5px">
                <Tabs>
                    <f:Tab Title="列表" runat="server" AutoScroll="true">
                        <Items>
                            <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false"
                                DataKeyNames="ID" OnPreDataBound="Grid1_PreDataBound"
                                OnRowCommand="Grid1_RowCommand" EnableMultiSelect="false"
                                EnableTree="true" TreeColumn="Name" DataIDField="ID" DataParentIDField="ParentID" ExpandAllTreeNodes="true"
                                MouseWheelSelection="true" >
                                <Toolbars>
                                    <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                                        <Items>
                                            <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                            </f:ToolbarFill>
                                            <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增部门">
                                            </f:Button>
                                        </Items>
                                    </f:Toolbar>
                                </Toolbars>
                                <Columns>
                                    <f:RowNumberField EnableTreeNumber="true" />
                                    <f:BoundField ColumnID="Name" DataField="Name" HeaderText="部门名称" ExpandUnusedSpace="true" />
                                    <f:BoundField ColumnID="DepartmentControlTypeName" DataField="DepartmentControlTypeName" HeaderText="部门属性" />
                                    <f:BoundField ColumnID="ProvinceName" DataField="ProvinceName" HeaderText="区域" />
                                    <f:BoundField DataField="Description" HeaderText="部门描述" Width="100px" />
                                    <f:BoundField DataField="LeaderName" HeaderText="负责人" Width="100px" />
                                    <f:BoundField DataField="DisplayOrder" HeaderText="排序" Width="60px" />
                                    <f:BoundField DataField="EECount" HeaderText="部门人数" Width="100px" />
                                    <f:CheckBoxField DataField="Enabled" SortField="Enabled" HeaderText="启用" RenderAsStaticField="true"
                                        Width="50px" />
                                    <f:WindowField ColumnID="assignField" TextAlign="Center" Icon="UserAdd" ToolTip="添加员工到此部门"
                                        WindowID="Window1" Title="添加员工" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/HR/Org_Form.aspx?id={0}"
                                        Width="50px" />
                                    <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                                        WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/HR/Org_Form.aspx?id={0}"
                                        Width="50px" />
                                    <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="删除"
                                        ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px" />
                                </Columns>
                            </f:Grid>
                        </Items>
                    </f:Tab>
                    <f:Tab runat="server" Title="架构图" AutoScroll="true">
                        <Content>
                            <div id="org-nodes-source-wrap"></div>                                                          
                            <div id="orgChart"></div>
                        </Content>
                    </f:Tab>
                </Tabs>
            </f:TabStrip>
        </Items>
    </f:Panel>
    <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
        Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
        Width="900px" Height="500px" OnClose="Window1_Close">
    </f:Window>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="../../res/js/jquery.orgchart.min.js"></script>
    <script>
        F.ready(function () {
            initChart();
            $("#org-nodes-source").orgChart({ container: $("#orgChart") });
        });

        function initChart() {
            infobasisService.getAjaxInstance.get('/department')
              .then(function (response) {
                  var data = response.data;
                  var rtn = processOrgChart(data);
                  console.log(rtn);
              })
              .catch(function (error) {
                  console.log(error);
              });
        };

        function processOrgChart(data) {
            var rtnHtml = '<ul id="org-nodes-source" class="hide"><li><span>总公司</span><ul>';
            $.each(data, function (key, val) {
                if (val.parentID == 0 || val.parentID == null)
                    rtnHtml += '<li><span>' + val.name + '</span><p>' + '</span><p>' + (val.leaderName ? '(' + val.leaderName +')' : '') + '</p>' + processSubData(val.id, data) + '</li>';
                else
                    rtnHtml += '';
            });
            rtnHtml += '</ul></li></ul>';
            $("#org-nodes-source-wrap").html(rtnHtml);
            $("#org-nodes-source").orgChart({ container: $("#orgChart") });

        };
        function processSubData(parentID, data) {
            var subhtml = '<ul>';

            var newData = [];
            $.each(data, function (key, val) {
                if (val.parentID == parentID)
                    newData.push(val);
            });

            if (newData.length == 0)
                subhtml = '';
            else {
                $.each(newData, function (key, val) {
                    subhtml += '<li><span>' + val.name + '</span><p>' + (val.leaderName ? '(' + val.leaderName + ')' : '') + '</p>' + processSubData(val.id, data) + '</li>';
                });
                subhtml += '</ul>';
            }

            return subhtml;
        };
    </script>
</asp:Content>