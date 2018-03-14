<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientList.aspx.cs" Inherits="Infobasis.Web.Pages.Business.ClientList"
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
        ShowHeader="false" Title="客户管理">
        <Items>
            <f:Form ID="Form2" runat="server" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                <Rows>
                    <f:FormRow ID="FormRow1" runat="server">
                        <Items>
                            <f:SimpleForm ID="SimpleForm1" CssClass="mysimpleform" runat="server" ShowBorder="false" EnableCollapse="true"
                                Layout="VBox" ShowHeader="false">
                                <Items>
                                    <f:Panel runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                        <Items>
                                            <f:TextBox runat="server" ID="tbxName" EmptyText="客户名称/手机/地址等"></f:TextBox>
                                            <f:DatePicker runat="server" Width="102px" CssClass="marginr" Required="false" DateFormatString="yyyy-MM-dd" EmptyText="开始日期"
                                                ShowLabel="false" Label="销毁统计开始时间"
                                                ID="dpStartDate" EnableEdit="false">
                                            </f:DatePicker>
                                            <f:DatePicker ID="dpEndDate" Width="102px" CssClass="marginr" Required="false" Readonly="false"
                                                CompareControl="dpStartDate" DateFormatString="yyyy-MM-dd" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期" EmptyText="结束日期"
                                                ShowLabel="false" Label="销毁统计结束时间"
                                                runat="server" EnableEdit="false">
                                            </f:DatePicker>
                                            <f:DropDownList runat="server" ID="DropDownListTraceStatus" Width="100px">
                                                <f:ListItem Text="跟进状态" Value="0" />
                                            </f:DropDownList>
                                            <f:DropDownList runat="server" ID="DropDownListClientFrom" Width="100px">
                                                <f:ListItem Text="客户来源" Value="0" />
                                            </f:DropDownList>
                                            <f:DropDownList runat="server" ID="DropDownListFee" Width="100px">
                                                <f:ListItem Text="收费状态" Value="0" />
                                            </f:DropDownList>
                                            <f:TriggerBox ID="TriggerBoxInput" EnableEdit="false" Text="" EmptyText="录入人员" EnablePostBack="false" Width="100px"
                                                TriggerIcon="Search" ShowLabel="false" runat="server">
                                            </f:TriggerBox>
                                            <f:HiddenField ID="HiddenFieldInput" runat="server">
                                            </f:HiddenField>
                                            <f:LinkButton  runat="server" CssStyle="margin-left:6px;" Text="重置">
                                                <Listeners>
                                                    <f:Listener Event="click" Handler="onClearDateClick" />
                                                </Listeners>
                                            </f:LinkButton>
                                            <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server" MarginLeft="6px"></f:ToolbarSeparator>
                                            <f:Button runat="server" ID="btnSearch" CssStyle="margin-left:6px;" Text="查询" ValidateForms="SimpleForm1" OnClick="btnSearch_Click" IconFont="Search"></f:Button>
                                            <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server" MarginLeft="6px"></f:ToolbarSeparator>
                                            <f:Button ID="btnAssignSelected" Icon="ArrowSwitch" runat="server" Text="分配业务员">
                                            </f:Button>
                                        </Items>
                                    </f:Panel>
                                    <f:Panel runat="server" Layout="HBox" ShowBorder="false" ShowHeader="false">
                                        <Items>

                                        </Items>
                                    </f:Panel>

                                </Items>
                            </f:SimpleForm>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
            <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                EnableCheckBoxSelect="true" AllowColumnLocking="true" AutoScroll="true"
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
                    <f:RowNumberField Width="20px" EnablePagingNumber="true" EnableLock="true" Locked="true" />
                    <f:WindowField DataTextField="Name" SortField="Name" Width="60px" HeaderText="客户名" EnableLock="true" Locked="true"
                        WindowID="Window1" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/Admin/UserView.aspx?id={0}" />
                    <f:GroupField ColumnID="SchoolInfo" HeaderText="回访情况" HeaderTextAlign="Center">
                        <Columns>
                            <f:BoundField Width="30px" ColumnID="TraceNum" DataField="TraceNum" HeaderText="次数" HeaderToolTip="回访次数" />
                            <f:BoundField Width="90px" DataField="LastTraceDate" DataToolTipField="LastTraceDate" DataToolTipFormatString="{0:yyyy-MM-dd hh:MM:dd}" DataFormatString="{0:yyyy-MM-dd}" HeaderText="最后回访时间" />
                           <f:WindowField MinWidth="200px" ExpandUnusedSpace="true" DataTextField="LastTraceMsg" HtmlEncode="true" SortField="LastTraceMsg" HeaderText="最后回访结果"
                                WindowID="Window1" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/Business/ClientTrace.aspx?id={0}" />
                        </Columns>
                    </f:GroupField>
                    <f:BoundField DataField="Tel" SortField="Tel" Width="80px" HeaderText="电话" />
                    <f:BoundField DataField="HousesName" Width="80px"  HeaderText="楼盘" DataToolTipField="HousesName" />
                    <f:BoundField DataField="HouseStructTypeName" Width="80px"  HeaderText="类型" DataToolTipField="HouseStructTypeName" />
                    <f:BoundField DataField="BuiltupArea" SortField="BuiltupArea" Width="40px"  HeaderText="㎡" HeaderToolTip="面积" />
                    <f:BoundField DataField="Budget" SortField="Budget" Width="40px"  HeaderText="预" HeaderToolTip="预算(万元)" />
                    <f:BoundField DataField="DesignUserDisplayName" Width="60px"  HeaderText="设计师" />
                    <f:BoundField DataField="ClientNeedIDs" Width="100px" HeaderText="客户需求" />
                    <f:BoundField DataField="ClientTraceStatusName" SortField="ClientTraceStatusName" Width="110px"  HeaderText="跟进状态" />
                    <f:BoundField ColumnID="bfTraceNum" runat="server" Width="70px" TextAlign="Center"></f:BoundField>
                    <f:WindowField ColumnID="changeToNextField" TextAlign="Center" Icon="ArrowBranch" ToolTip="选部门" HeaderText=""
                        WindowID="Window1" Title="选部门" Text="选部门" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/Business/ClientTransfer.aspx?id={0}"
                        Width="70px" />
                    <f:WindowField ColumnID="disableClientField" TextAlign="Center" Icon="VcardDelete" ToolTip="申请废单" HeaderText="废单"
                        WindowID="Window1" Title="申请废单" Text="申请" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/Business/DisableClient.aspx?id={0}"
                        Width="70px" />

                    <f:BoundField Width="70px" DataField="ClientFromName"  HeaderText="客户来源" />
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
            var formId = this.el.parents('.f-form').attr('id');

            F(formId).reset();
        }

    </script>
</asp:Content>