<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Meeting.aspx.cs" Inherits="Infobasis.Web.Pages.OA.Meeting" 
    MasterPageFile="~/PageMaster/Page.master"%>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
        ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
        ShowHeader="false" Title="会议记录" Width="300px">
        <Items>
            <f:Form ID="Form2" runat="server" ShowHeader="false" ShowBorder="false" LabelAlign="Right" Width="300px">
                <Rows>
                    <f:FormRow ID="FormRow1" runat="server">
                        <Items>
                            <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在会议记录中搜索"
                                Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                OnTrigger1Click="ttbSearchMessage_Trigger1Click" Width="300px">
                            </f:TwinTriggerBox>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
            <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                EnableCheckBoxSelect="true" EnableTextSelection="true"
                DataKeyNames="ID,Topic" AllowSorting="true" OnSort="Grid1_Sort" SortField="Topic"
                SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true" OnPreDataBound="Grid1_PreDataBound"
                OnPageIndexChange="Grid1_PageIndexChange"
                MouseWheelSelection="true" QuickPaging="true">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnDeleteSelected" Icon="Delete" runat="server" Text="删除选中记录" OnClick="btnDeleteSelected_Click">
                            </f:Button>
                            <f:ToolbarFill ID="ToolbarFill1" runat="server">
                            </f:ToolbarFill>
                            <f:Button ID="btnNew" OnClientClick="window.open('/Pages/OA/Meeting_Form.aspx','_self');" runat="server" Icon="Add" EnablePostBack="false" Text="新增会议记录">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
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
                    <f:RowNumberField Width="35px" EnablePagingNumber="true" />
                    <f:BoundField DataField="MeetingTypeName" SortField="MeetingTypeName" Width="100px" HeaderText="会议类型" />
                    <f:BoundField DataField="Topic" SortField="Topic" Width="300px" HeaderText="会议标题" />
                    <f:BoundField DataField="CreateByName" SortField="CreateByName" Width="100px" HeaderText="发布人" />
                    <f:BoundField Width="90px" DataField="CreateDatetime" DataToolTipField="CreateDatetime" DataToolTipFormatString="{0:yyyy-MM-dd hh:MM:dd}" DataFormatString="{0:yyyy-MM-dd}" HeaderText="发布时间" />

                    <f:HyperLinkField ColumnID="editField" TextAlign="Center" Text="<img class='f-grid-cell-icon' src='/res/icon/pencil.png'>" ToolTip="编辑" 
                        DataNavigateUrlFields="ID" DataNavigateUrlFormatString="~/Pages/OA/Meeting_Form.aspx?id={0}" Target="_self" ></f:HyperLinkField>
                    <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="删除"
                        ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px" />
                </Columns>
            </f:Grid>
        </Items>
    </f:Panel>

</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>