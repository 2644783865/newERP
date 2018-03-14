<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Form.aspx.cs" Inherits="Infobasis.Web.Pages.Admin.User_Form"
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
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
                            <f:TextBox ID="tbxName" runat="server" Label="用户名" Required="true" ShowRedStar="true">
                            </f:TextBox>
                            <f:TextBox ID="tbxRealName" runat="server" Label="中文名" Required="true" ShowRedStar="true">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow2" runat="server">
                        <Items>
                            <f:CheckBox ID="cbxEnabled" runat="server" Label="是否启用">
                            </f:CheckBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow5" runat="server">
                        <Items>
                            <f:TextBox ID="tbxPassword" runat="server" TextMode="Password" Label="登录密码" Required="true" ShowRedStar="true">
                            </f:TextBox>
                            <f:TextBox ID="tbxEmail" runat="server" Label="邮箱" Required="false" ShowRedStar="false">
                            </f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow7" runat="server">
                        <Items>
                            <f:DropDownBox runat="server" ID="ddbRoles" Label="所属角色" AutoShowClearIcon="true" DataControlID="cblRoles" EnableMultiSelect="true">
                                <PopPanel>
                                    <f:SimpleForm ID="SimpleForm2" BodyPadding="10px" runat="server" AutoScroll="true"
                                        ShowBorder="True" ShowHeader="false" Hidden="true">
                                        <Items>
                                            <f:CheckBoxList ID="cblRoles" ColumnNumber="3" DataTextField="Name" DataValueField="ID" runat="server">
                                            </f:CheckBoxList>
                                        </Items>
                                    </f:SimpleForm>
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
    <f:Window ID="Window1" Title="编辑" Hidden="true" EnableIFrame="true" runat="server"
        EnableMaximize="true" EnableResize="true" Target="Top" IsModal="True" Width="550px"
        Height="350px">
    </f:Window>
</asp:Content>

<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>