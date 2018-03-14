<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BudgetItem_Form.aspx.cs" Inherits="Infobasis.Web.Pages.Budget.BudgetItem_Form"
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
                            <f:DropDownList ID="DropDownPartType" Label="部位名称" Required="true" ShowRedStar="false" runat="server">
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
                </Rows>
            </f:Form>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>
