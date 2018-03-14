<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EE.aspx.cs" Inherits="Infobasis.Web.Pages.HR.EE" 
    MasterPageFile="~/PageMaster/Page.master"%>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" BodyPadding="5px" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
        ShowBorder="false" ShowHeader="false" AutoScroll="true" Layout="Fit">
        <Items>
            <f:TabStrip ID="TabStrip1" runat="server" ShowBorder="false" TabPosition="Top" TabBorderColor="true" 
                TabPlain="true" BodyPadding="5px" MarginTop="5px" AutoScroll="true" CssClass="f-tabstrip-theme-simple">
                <Tabs>
                    <f:Tab ID="Tab1" Title="人事信息" runat="server" AutoScroll="true">
                        <Items>
                            <f:Form ID="Form2" runat="server" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                                <Rows>
                                    <f:FormRow ID="FormRow1" runat="server">
                                        <Items>
                                            <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在员工名称中搜索"
                                                Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                                OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                                            </f:TwinTriggerBox>
                                            <f:RadioButtonList ID="rblEnableStatus" AutoPostBack="true" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged"
                                                Label="雇佣状态" ColumnNumber="3" runat="server">
                                                <f:RadioItem Text="全部" Value="all" />
                                                <f:RadioItem Text="在职" Selected="true" Value="enabled" />
                                                <f:RadioItem Text="离职" Value="disabled" />
                                            </f:RadioButtonList>
                                        </Items>
                                    </f:FormRow>
                                </Rows>
                            </f:Form>
                            <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                                EnableCheckBoxSelect="true" AutoScroll="true" PageSize="20"
                                DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ChineseName"
                                SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true" OnPreDataBound="Grid1_PreDataBound" OnPreRowDataBound="Grid1_PreRowDataBound"
                                OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange"
                                MouseWheelSelection="true" QuickPaging="true" EnableTextSelection="true"
                                AllowColumnLocking="true">
                                <Toolbars>
                                    <f:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                            <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增员工">
                                            </f:Button>
                                        </Items>
                                    </f:Toolbar>
                                </Toolbars>
                                <PageItems>
                                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                    </f:ToolbarSeparator>
                                    <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                    </f:ToolbarText>
                                    <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="false" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                                        runat="server">
                                        <f:ListItem Text="10" Value="10" />
                                        <f:ListItem Text="20" Value="20" />
                                        <f:ListItem Text="50" Value="50" />
                                        <f:ListItem Text="100" Value="100" />
                                    </f:DropDownList>
                                </PageItems>
                                <Columns>
                                    <f:RowNumberField Width="25px" EnablePagingNumber="true" />
                                    <f:TemplateField HeaderText="姓名" SortField="ChineseName" Width="100px" EnableLock="true" Locked="true">
                                        <ItemTemplate>
                                            <a href="javascript:;" onclick="<%# GetEditUrl(Eval("Id"), Eval("ChineseName")) %>"><%# Eval("ChineseName") %></a>
                                        </ItemTemplate>
                                    </f:TemplateField>
                                    <f:BoundField DataField="Gender" SortField="Gender" Width="30px" HeaderText="" />
                                    <f:BoundField DataField="EmployeeCode" SortField="EmployeeCode" Width="80px" HeaderText="员工工号" />
                                    <f:BoundField DataField="DepartmentName" SortField="DepartmentName" Width="100px" HeaderText="部门" />
                                    <f:BoundField DataField="JobName" SortField="JobName" Width="100px" HeaderText="职称" />
                                    <f:BoundField DataField="EmploymentTypeName" SortField="EmploymentType" Width="80px" HeaderText="用工形式" />
                                    <f:WindowField TextAlign="Center" ToolTip="修改雇佣状态" Title="雇佣状态" DataTextField="HireStatusName" HeaderText="雇佣状态"
                                        WindowID="Window1" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/HR/ChangeHireStatus.aspx?id={0}"
                                        Width="80px" />
                                    <f:BoundField DataField="ReportsToName" SortField="ReportsToName" Width="100px" HeaderText="汇报上级" />
                                    <f:BoundField DataField="Email" SortField="Email" Width="180px" HeaderText="邮箱" />
                                    <f:BoundField Width="90px" DataField="OnBoardDate" DataToolTipField="OnBoardDate" DataToolTipFormatString="{0:yyyy-MM-dd}" DataFormatString="{0:yyyy-MM-dd}" HeaderText="入职日期" />
                                    <f:BoundField Width="90px" DataField="TerminateDate" DataToolTipField="TerminateDate" DataToolTipFormatString="{0:yyyy-MM-dd}" DataFormatString="{0:yyyy-MM-dd}" HeaderText="离职日期" />
                                    <f:BoundField DataField="Name" SortField="Name" Width="80px" HeaderText="账号" />
                                    <f:BoundField DataField="Remark" ExpandUnusedSpace="true" HeaderText="备注" />
                                    <f:WindowField TextAlign="Center" Icon="Information" ToolTip="查看详细信息" Title="查看详细信息"
                                        WindowID="Window1" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/Admin/UserView.aspx?id={0}"
                                        Width="50px" />
                                </Columns>
                            </f:Grid>
                        </Items>
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
    <script>
        function getNewWindowUrl() {
            return F.baseUrl + 'Pages/HR/EE_Form.aspx?parenttabid=' + parent.getActiveTabId();
        }


        function getEditWindowUrl(id, name) {
            return F.baseUrl + 'Pages/HR/EE_Form.aspx?id=' + id + '&name=' + name + '&parenttabid=' + parent.getActiveTabId();
        }
    </script>
</asp:Content>