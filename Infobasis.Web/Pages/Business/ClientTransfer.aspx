<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientTransfer.aspx.cs" Inherits="Infobasis.Web.Pages.Business.ClientTransfer"
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" />
    <f:Panel ID="PanelInfo1" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox">
        <Items>
            <f:Label runat="server" ID="labNo" Label="客户编号" BoxFlex="1" LabelAlign="Right"></f:Label>
            <f:Label runat="server" ID="labName" Label="客户名称" BoxFlex="1" LabelAlign="Right"></f:Label>
            <f:Label runat="server" ID="labTel" Label="联系电话" BoxFlex="1" LabelAlign="Right"></f:Label>
        </Items>
    </f:Panel>
    <f:Panel ID="PanelInfo2" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox">
        <items>
            <f:Label runat="server" ID="labHousesName" Label="楼盘" BoxFlex="1" LabelAlign="Right"></f:Label>
            <f:Label runat="server" ID="labAddress" Label="地址" BoxFlex="1" LabelAlign="Right"></f:Label>
        </items>
    </f:Panel>

    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server"
        BodyPadding="10px" AutoScroll="true" MarginTop="10px">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                        Text="关闭">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                        runat="server" Text="确认">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server"
                Title="SimpleForm" EnableTableStyle="true" MessageTarget="Qtip">
                <Rows>
                    <f:FormRow ID="FormRow7" runat="server">
                        <Items>
                            <f:DropDownBox runat="server" ID="DropDownBoxDesigner" EmptyText="请从下拉表格中选择设计师" DataControlID="Grid1" 
                                EnableMultiSelect="false" MatchFieldWidth="false" Width="350px">
                                <PopPanel>
                                    <f:Panel ID="Panel7" runat="server" BodyPadding="5px" Width="350px" Height="300px" Hidden="true"
                                        ShowBorder="true" ShowHeader="false" Layout="VBox" BoxConfigAlign="Stretch">
                                        <Items>
                                            <f:Form ID="Form5" ShowBorder="False" ShowHeader="False" runat="server">
                                                <Rows>
                                                    <f:FormRow>
                                                        <Items>
                                                            <f:TwinTriggerBox Width="300px" runat="server" EmptyText="在姓名中查找" ShowLabel="false" ID="ttbSearch"
                                                                ShowTrigger1="false" OnTrigger1Click="ttbSearch_Trigger1Click" OnTrigger2Click="ttbSearch_Trigger2Click"
                                                                Trigger1Icon="Clear" Trigger2Icon="Search">
                                                            </f:TwinTriggerBox>
                                                        </Items>
                                                    </f:FormRow>
                                                </Rows>
                                            </f:Form>
                                            <f:Grid ID="Grid1" BoxFlex="1"
                                                DataIDField="ID" DataTextField="ChineseName" EnableMultiSelect="false"
                                                PageSize="10" ShowBorder="true" ShowHeader="false"
                                                AllowPaging="true" IsDatabasePaging="true" runat="server" EnableCheckBoxSelect="True"
                                                DataKeyNames="ID" OnPageIndexChange="Grid1_PageIndexChange"
                                                AllowSorting="true" SortField="ChineseName" SortDirection="ASC"
                                                OnSort="Grid1_Sort">
                                                <Columns>
                                                    <f:RowNumberField />
                                                    <f:BoundField Width="100px" DataField="ChineseName" SortField="ChineseName" DataFormatString="{0}"
                                                        HeaderText="姓名" />
                                                    <f:BoundField runat="server" DataField="Gender" HeaderText="性别"></f:BoundField>
                                                    <f:BoundField ExpandUnusedSpace="True" DataField="DeptName" HeaderText="部门" />
                                                </Columns>
                                            </f:Grid>
                                        </Items>
                                    </f:Panel>
                                </PopPanel>
                            </f:DropDownBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow10" runat="server">
                        <Items>
                            <f:TextArea ID="tbxRemark" runat="server" Label="备注">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>