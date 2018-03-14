<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmsTemplateForm.aspx.cs" Inherits="Infobasis.Web.Pages.CRM.SmsTemplateForm"
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style>
        .place-holder {background-color: #ffd800}
    </style>
</asp:Content>

<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server"
        BodyPadding="10px" AutoScroll="true">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                        Text="关闭">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                        runat="server" Text="保存后关闭">
                    </f:Button>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server"
                Title="SimpleForm" EnableTableStyle="true" MessageTarget="Qtip">
                <Rows>
                    <f:FormRow ID="FormRow1" runat="server">
                        <Items>
                            <f:TextBox ID="tbxName" runat="server" Label="模版名称" Required="true" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow7" runat="server">
                        <Items>
                            <f:DropDownList ID="DropDownTemplateType" Label="模版类型" Required="false" ShowRedStar="false" runat="server">
                                <f:ListItem Value="0" Text="活动优惠短信" Selected="true" />
                                <f:ListItem Value="1" Text="回访短信" />
                                <f:ListItem Value="2" Text="节日问候短信" />
                                <f:ListItem Value="3" Text="分阶段施工告知短信" />
                                <f:ListItem Value="4" Text="邀约短信" />
                            </f:DropDownList> 
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow10" runat="server">
                        <Items>
                            <f:TextArea ID="tbxContent" runat="server" Label="内容">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow2" runat="server">
                        <Items>
                            <f:CheckBox ID="cbxEnabled" runat="server" Label="是否启用">
                            </f:CheckBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow runat="server">
                        <Items>
                            <f:ContentPanel runat="server">
                                <div>占位符</div>
                                <div><span class="place-holder">%UserName%</span> - 用户名字</div>
                            </f:ContentPanel>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>
