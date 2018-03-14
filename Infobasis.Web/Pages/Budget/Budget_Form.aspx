<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Budget_Form.aspx.cs" Inherits="Infobasis.Web.Pages.Budget.Budget_Form"
     MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="PanelInfo1" runat="server" />
    <f:Panel ID="PanelInfo1" runat="server" ShowBorder="false" ShowHeader="false">
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
                    <f:FormRow>
                        <Items>
                            <f:TextBox ID="tbxCode" Required="true" runat="server" Width="120px" Label="模板编号"></f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow7" runat="server">
                        <Items>
                            <f:TextBox ID="tbxName" Required="true" runat="server" Width="120px" Label="模板名称"></f:TextBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow1" runat="server">
                        <Items>
                            <f:DropDownList ID="DropDownProvince" Label="区域" Required="false" ShowRedStar="false" runat="server">
                                <f:ListItem Text="浦东" Value="浦东" />
                                <f:ListItem Text="青浦" Value="青浦" />
                            </f:DropDownList>                            
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:DropDownList ID="DropDownBoxBudgetType" Label="区域" Required="true" ShowRedStar="false" runat="server">
                                <f:ListItem Text="" Value="0" />
                            </f:DropDownList>  
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:NumberBox runat="server" ID="tbxDisplayOrder" Label="排序" Text="1"></f:NumberBox>
                        </Items>
                    </f:FormRow>
                    <f:FormRow ID="FormRow10" runat="server">
                        <Items>
                            <f:TextArea ID="tbxRemark" runat="server" Label="备注">
                            </f:TextArea>
                        </Items>
                    </f:FormRow>
                    <f:FormRow>
                        <Items>
                            <f:CheckBox ID="tbxIsActive" runat="server" Label="启用"></f:CheckBox>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>
