<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmsMgr.aspx.cs" Inherits="Infobasis.Web.Pages.CRM.SmsMgr"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="PanelMain" />
    <f:Panel ID="PanelMain" ShowBorder="false" ShowHeader="false" runat="server"
        BodyPadding="10px" AutoScroll="true">
        <Items>
            <f:TabStrip ID="TabStripPage" runat="server" ShowBorder="false" TabPosition="Top" TabBorderColor="true" 
                TabPlain="true" BodyPadding="5px" MarginTop="3px" AutoScroll="false" CssClass="f-tabstrip-theme-simple" Height="525px">
                <Tabs>
                    <f:Tab ID="Tab1" Title="短息发送" runat="server">
                        <Items>
                            <f:Form ID="Form2" runat="server" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                                <Rows>
                                    <f:FormRow ID="FormRow1" runat="server">
                                        <Items>
                                            <f:TextBox runat="server" ID="tbxName" EmptyText="客户名称/手机/地址等"></f:TextBox>
                                            <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server" MarginLeft="6px"></f:ToolbarSeparator>
                                            <f:Button runat="server" ID="btnSearch" CssStyle="margin-left:6px;" Text="查询" ValidateForms="Form2" OnClick="btnSearch_Click" IconFont="Search"></f:Button>
                                            <f:Button ID="btnSend" runat="server" Icon="Add" EnablePostBack="true" Text="确定选择" OnClick="btnSend_Click"></f:Button>
                                        </Items>
                                    </f:FormRow>
                                </Rows>
                            </f:Form>
                            <f:Grid ID="GridSmsSend" runat="server" ShowBorder="true" ShowHeader="false"
                                EnableCheckBoxSelect="true" AutoScroll="true"
                                DataKeyNames="ID,Name,Tel" AllowSorting="true" SortField="Name" OnPageIndexChange="GridSmsSend_PageIndexChange"
                                SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true"
                                MouseWheelSelection="true" QuickPaging="true" EnableTextSelection="true" KeepCurrentSelection="true"
                                DataIDField="ID" ClearSelectionBeforePaging="false" ClearSelectionBeforeBinding="false" KeepPagedSelection="true"
                                AllowColumnLocking="false">
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
                                    <f:RowNumberField Width="25px" EnablePagingNumber="true" />
                                    <f:BoundField DataField="ClientTraceStatusName" SortField="ClientTraceStatusName" Width="130px"  HeaderText="客户状态" />
                                    <f:BoundField DataField="Name" SortField="Name" Width="80px" HeaderText="姓名" />                                    
                                    <f:BoundField DataField="ClientNeedIDs" Width="130px" HeaderText="客户需求" />
                                    <f:BoundField DataField="Tel" SortField="Tel" Width="80px" HeaderText="电话" />
                                    <f:BoundField DataField="HousesName" Width="180px"  HeaderText="楼盘" DataToolTipField="HousesName" />
                                    <f:BoundField DataField="DeliverHouseDate" Width="80px"  HeaderText="交房日期" DataFormatString="{0:yyyy-MM-dd}" DataToolTipField="DeliverHouseDate" ExpandUnusedSpace="true" />
                                </Columns>
                            </f:Grid>
                        </Items>
                    </f:Tab>
                    <f:Tab ID="Tab2" Title="短信模版" runat="server">
                        <Items>
                            <f:Form ID="FormSMSTemplate" runat="server" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                                <Rows>
                                    <f:FormRow ID="FormRow2" runat="server">
                                        <Items>
                                            <f:TextBox runat="server" ID="tbxSMSTemplateName" EmptyText="短信模版名称"></f:TextBox>
                                            <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server" MarginLeft="6px"></f:ToolbarSeparator>
                                            <f:Button runat="server" ID="btnSMSTemplate" CssStyle="margin-left:6px;" Text="查询" ValidateForms="FormSMSTemplate" OnClick="btnSMSTemplate_Click" IconFont="Search"></f:Button>
                                            <f:Button runat="server" ID="btnSMSTemplateAdd" EnablePostBack="false" Text="添加" Icon="Add"></f:Button>
                                        </Items>
                                    </f:FormRow>
                                </Rows>
                            </f:Form>
                            <f:Grid ID="GridSMSTemplateList" runat="server" ShowBorder="true" ShowHeader="false"
                                EnableCheckBoxSelect="true" AutoScroll="true"
                                DataKeyNames="ID" AllowSorting="true" SortField="Name"
                                SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true"
                                MouseWheelSelection="true" QuickPaging="true" EnableTextSelection="true" KeepCurrentSelection="true"
                                DataIDField="ID" ClearSelectionBeforePaging="false" ClearSelectionBeforeBinding="false" KeepPagedSelection="true"
                                AllowColumnLocking="false">
                                <PageItems>
                                    <f:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                    </f:ToolbarSeparator>
                                    <f:ToolbarText ID="ToolbarText2" runat="server" Text="每页记录数：">
                                    </f:ToolbarText>
                                    <f:DropDownList ID="ddlGridSMSTemplateListPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridSMSTemplateListPageSize_SelectedIndexChanged"
                                        runat="server">
                                        <f:ListItem Text="10" Value="10" />
                                        <f:ListItem Text="20" Value="20" />
                                        <f:ListItem Text="50" Value="50" />
                                        <f:ListItem Text="100" Value="100" />
                                    </f:DropDownList>
                                </PageItems>
                                <Columns>
                                    <f:RowNumberField Width="25px" EnablePagingNumber="true" />
                                    <f:BoundField DataField="Name" SortField="Name" Width="80px" HeaderText="模版名称" />   
                                    <f:BoundField DataField="Content" SortField="Context" Width="280px" HeaderText="内容" />                                 
                                    <f:BoundField DataField="TemplateTypeName" Width="130px" HeaderText="模版类型" />
                                    <f:BoundField DataField="CreateDatetime" Width="80px"  HeaderText="创建日期" DataFormatString="{0:yyyy-MM-dd}" DataToolTipField="CreateDatetime" ExpandUnusedSpace="true" />
                                    <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                                        WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Pages/CRM/SmsTemplateForm.aspx?id={0}"
                                        Width="50px" />
                                </Columns>
                            </f:Grid>
                        </Items>
                    </f:Tab>
                    <f:Tab ID="Tab3" Title="发送记录" runat="server">
                        <Items>
                            <f:Form ID="Form1" ShowBorder="False" ShowHeader="False" runat="server">
                                <Rows>
                                    <f:FormRow>
                                        <Items>
                                            <f:TwinTriggerBox Width="300px" runat="server" EmptyText="在短信记录中查找" ShowLabel="false" ID="ttbHistorySearch"
                                                ShowTrigger1="false" OnTrigger1Click="ttbHistorySearch_Trigger1Click" OnTrigger2Click="ttbHistorySearch_Trigger2Click"                         
                                                Trigger1Icon="Clear" Trigger2Icon="Search">
                                            </f:TwinTriggerBox>
                                        </Items>
                                    </f:FormRow>
                                </Rows>
                            </f:Form>
                            <f:Grid ID="GridSendHistory" BoxFlex="1"
                                DataIDField="ID" DataTextField="Name" EnableMultiSelect="false" KeepCurrentSelection="true"
                                PageSize="10" ShowBorder="true" ShowHeader="false" OnPageIndexChange="GridSendHistory_PageIndexChange"
                                AllowPaging="true" IsDatabasePaging="true"
                                runat="server" EnableCheckBoxSelect="false" DataKeyNames="ID,Name"
                                AllowSorting="true" SortField="CreateDatetime" SortDirection="DESC"
                                OnSort="GridSendHistory_Sort">
                                <PageItems>
                                    <f:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                                    </f:ToolbarSeparator>
                                    <f:ToolbarText ID="ToolbarText3" runat="server" Text="每页记录数：">
                                    </f:ToolbarText>
                                    <f:DropDownList ID="ddlGridSendHistoryPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridSendHistoryPageSize_SelectedIndexChanged"
                                        runat="server">
                                        <f:ListItem Text="10" Value="10" Selected="true" />
                                        <f:ListItem Text="20" Value="20" />
                                        <f:ListItem Text="50" Value="50" />
                                        <f:ListItem Text="100" Value="100" />
                                    </f:DropDownList>
                                </PageItems>
                                <Columns>
                                    <f:RowNumberField />
                                    <f:BoundField Width="100px" DataField="SMSType" SortField="SMSType" DataFormatString="{0}"
                                        HeaderText="发送类型" />
                                    <f:BoundField Width="100px" DataField="Tel" SortField="Tel" DataFormatString="{0}"
                                        HeaderText="手机号码" />
                                    <f:BoundField Width="100px" DataField="Name" SortField="Name" DataFormatString="{0}"
                                        HeaderText="接收人" />
                                    <f:BoundField Width="300px" DataField="Content" SortField="Content" DataFormatString="{0}"
                                        HeaderText="短信内容" ExpandUnusedSpace="true" />
                                    <f:BoundField Width="150px" SortField="CreateDatetime" DataField="CreateDatetime" HeaderText="创建日期" />
                                </Columns>
                            </f:Grid>
                        </Items>
                    </f:Tab>
                    <f:Tab ID="Tab4" Title="短信账单" runat="server">
                        <Content>
                            <div>Come soon...</div>
                        </Content>
                    </f:Tab>
                </Tabs>
            </f:TabStrip>
        </Items>
    </f:Panel>
    <f:Window ID="Window2" Width="650px" Height="300px" Icon="TagBlue" Title="窗体"
        EnableMaximize="true" EnableCollapse="true" runat="server" EnableResize="true"
        IsModal="false" CloseAction="HidePostBack" Hidden="true" OnClose="Window2_Close" AutoScroll="true" BodyPadding="10px">
        <Items>
            <f:TextArea runat="server" ID="tbxReceiverNames" Label="短信接收人"></f:TextArea>
            <f:HiddenField runat="server" ID="tbxReceiverTels"></f:HiddenField>
            <f:DropDownBox runat="server" ID="DropDownBoxSmsTemplate" Label="可选择短信模版" AutoPostBack="true" EmptyText="可选择短信模版" DataControlID="GridSmsTemplate" OnTextChanged="DropDownBoxSmsTemplate_TextChanged" EnableMultiSelect="false" MatchFieldWidth="false">
                <PopPanel>
                    <f:Panel ID="Panel7" runat="server" BodyPadding="5px" Width="650px" Height="300px" Hidden="true"
                        ShowBorder="true" ShowHeader="false" Layout="VBox" BoxConfigAlign="Stretch">
                        <Items>
                            <f:Form ID="Form5" ShowBorder="False" ShowHeader="False" runat="server">
                                <Rows>
                                    <f:FormRow>
                                        <Items>
                                            <f:TwinTriggerBox Width="300px" runat="server" EmptyText="在短信模版中查找" ShowLabel="false" ID="ttbSearch"
                                                ShowTrigger1="false" OnTrigger1Click="ttbSearch_Trigger1Click" OnTrigger2Click="ttbSearch_Trigger2Click"                         
                                                Trigger1Icon="Clear" Trigger2Icon="Search">
                                            </f:TwinTriggerBox>
                                        </Items>
                                    </f:FormRow>
                                </Rows>
                            </f:Form>
                            <f:Grid ID="GridSmsTemplate" BoxFlex="1"
                                DataIDField="ID" DataTextField="Name" EnableMultiSelect="false" KeepCurrentSelection="true"
                                PageSize="10" ShowBorder="true" ShowHeader="false"
                                AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="GridSmsTemplate_PageIndexChange"
                                runat="server" EnableCheckBoxSelect="false" DataKeyNames="ID,Name"
                                AllowSorting="true" SortField="Name" SortDirection="ASC"
                                OnSort="GridSmsTemplate_Sort">
                                <Columns>
                                    <f:RowNumberField />
                                    <f:BoundField Width="100px" DataField="Name" SortField="Name" DataFormatString="{0}"
                                        HeaderText="模版名称" />
                                    <f:BoundField Width="80px" SortField="LastUpdateDatetime" DataField="LastUpdateDatetime" HeaderText="创建日期" />
                                    <f:BoundField ExpandUnusedSpace="True" DataField="TemplateTypeName" HeaderText="模版名称" />
                                </Columns>
                            </f:Grid>
                        </Items>
                    </f:Panel>
                </PopPanel>
            </f:DropDownBox>
            <f:TextArea runat="server" Label="短信内容" ID="tbxSmsText"></f:TextArea>
            <f:Button runat="server" ID="btnSendSms" Text="发送" OnClick="btnSendSms_Click"></f:Button>
        </Items>
    </f:Window>
    <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
        Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
        Width="900px" Height="500px" OnClose="Window1_Close">
    </f:Window>
</asp:Content>
