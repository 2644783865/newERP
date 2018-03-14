<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Org_Form.aspx.cs" Inherits="Infobasis.Web.Pages.HR.Org_Form"
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server"
        AutoScroll="true" BodyPadding="10px">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                        Text="关闭">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnContinue" Icon="DatabaseSave" ValidateForms="SimpleForm1" runat="server" Text="保存后继续添加" OnClick="btnSaveContinue_Click">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose"
                        OnClick="btnSaveClose_Click" runat="server" Text="保存后关闭">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server"
                Title="SimpleForm" EnableTableStyle="true" MessageTarget="Qtip" Width="600px">
                <Items>
                    <f:TextBox ID="tbxName" runat="server" Label="名称" Required="true" ShowRedStar="true">
                    </f:TextBox>
                    <f:NumberBox ID="tbxDisplayOrder" Label="排序" Required="true" Text="1" ShowRedStar="true" runat="server">
                    </f:NumberBox>
                    <f:DropDownBox runat="server" ID="ddbParent" Label="上级部门" AutoShowClearIcon="true">
                        <PopPanel>
                            <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" runat="server"
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
                    <f:DropDownBox runat="server" ID="ddbLeader" Label="负责人" AutoShowClearIcon="true"
                        EmptyText="请从下拉表格中选择" DataControlID="Grid2" 
                                EnableMultiSelect="false" MatchFieldWidth="false" Width="550px">
                        <PopPanel>
                            <f:Panel ID="Panel7" runat="server" BodyPadding="5px" Width="550px" Height="300px" Hidden="true"
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
                                                    <f:RadioButtonList ID="rblEnableStatus" AutoPostBack="true" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged"
                                                        Label="启用状态" ColumnNumber="3" runat="server">
                                                        <f:RadioItem Text="全部" Value="all" />
                                                        <f:RadioItem Text="启用" Value="enabled" Selected="true" />
                                                        <f:RadioItem Text="禁用" Value="disabled" />
                                                    </f:RadioButtonList>
                                                </Items>
                                            </f:FormRow>
                                        </Rows>
                                    </f:Form>
                                    <f:Grid ID="Grid2"
                                        DataIDField="ID" DataTextField="ChineseName" EnableMultiSelect="false"
                                        PageSize="10" ShowBorder="true" ShowHeader="false"
                                        AllowPaging="true" IsDatabasePaging="true" runat="server" EnableCheckBoxSelect="True"
                                        DataKeyNames="ID" OnPageIndexChange="Grid2_PageIndexChange"
                                        AllowSorting="true" SortField="ChineseName" SortDirection="ASC"
                                        OnSort="Grid2_Sort">
                                        <Columns>
                                            <f:RowNumberField />
                                            <f:BoundField Width="100px" DataField="ChineseName" SortField="ChineseName" DataFormatString="{0}"
                                                HeaderText="姓名" />
                                            <f:BoundField ID="BoundField1" runat="server" DataField="Gender" HeaderText="性别"></f:BoundField>
                                            <f:BoundField ExpandUnusedSpace="True" DataField="Department.Name" HeaderText="部门" />
                                        </Columns>
                                    </f:Grid>
                                </Items>
                            </f:Panel>
                        </PopPanel>
                    </f:DropDownBox>
                    <f:TextArea ID="tbxRemark" runat="server" Label="备注">
                    </f:TextArea>
                    <f:DropDownBox runat="server" Label="部门类型" ID="ddbControlType" DataControlID="rblControlType" EnableMultiSelect="false" AutoShowClearIcon="true">
                        <PopPanel>
                            <f:SimpleForm ID="SimpleForm2" BodyPadding="10px" runat="server" AutoScroll="true"
                                ShowBorder="True" ShowHeader="false" Hidden="true">
                                <Items>
                                    <f:Label ID="Label1" runat="server" Text="部门属性："></f:Label>
                                    <f:RadioButtonList ID="rblControlType" ColumnNumber="2" runat="server">
                                        <f:RadioItem Text="业务部门" Value="1" />
                                        <f:RadioItem Text="设计部门" Value="2" />
                                        <f:RadioItem Text="财务部门" Value="3" />
                                        <f:RadioItem Text="工程部门" Value="4" />
                                        <f:RadioItem Text="客服部门" Value="5" />
                                        <f:RadioItem Text="行政部门" Value="6" />
                                        <f:RadioItem Text="市场部门" Value="7" />
                                        <f:RadioItem Text="总办" Value="8" />
                                    </f:RadioButtonList>
                                </Items>
                            </f:SimpleForm>
                        </PopPanel>
                    </f:DropDownBox>  
                    <f:DropDownList ID="DropDownProvince" Label="区域" Required="false" ShowRedStar="false" runat="server">
                        <f:ListItem />
                    </f:DropDownList>                                      
                    <f:CheckBox ID="cbxEnabled" runat="server" Checked="true" Label="是否启用">
                    </f:CheckBox>
                </Items>
            </f:SimpleForm>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>