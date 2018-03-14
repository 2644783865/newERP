<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EE_Contract_Form.aspx.cs" Inherits="Infobasis.Web.Pages.HR.EE_Contract_Form"
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="FormEmployeement" runat="server" />
    <f:Form ID="FormEmployeement" MessageTarget="Qtip" ShowHeader="false" LabelAlign="Right" LabelWidth="90px" Width="600px" BodyPadding="5px" Title="" runat="server">
            <Items>
                <f:Panel ID="Panel3" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:Label ID="Label2" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                            Text="合同编号：">
                        </f:Label>
                        <f:TextBox ID="tbxContractNo" ShowLabel="false" Label="合同编号" Required="true" Width="200px" CssClass="marginr" runat="server">
                        </f:TextBox>
                        <f:Label ID="Label4" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                            Text="职位：">
                        </f:Label>
                        <f:TextBox ID="tbxJobTitle" ShowLabel="false" Label="职位" Required="true" Width="100px" CssClass="marginr" runat="server">
                        </f:TextBox>
                    </Items>
                </f:Panel>
                <f:Panel ID="Panel9" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:Label ID="Label7" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                            Text="起始日期：">
                        </f:Label>
                        <f:DatePicker ID="tbxStartDate" EnableEdit="false" ShowLabel="false" Label="起始日期" Required="false" Width="150px" CssClass="marginr" runat="server">
                        </f:DatePicker>
                        <f:Label ID="Label5" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                            Text="结束日期：">
                        </f:Label>
                        <f:DatePicker ID="tbxEndDate" EnableEdit="false" ShowLabel="false" Label="结束日期" Required="false" Width="150px" CssClass="marginr" runat="server">
                        </f:DatePicker>
                    </Items>
                </f:Panel>
                <f:Panel ID="Panel13" ShowHeader="false" ShowBorder="false" Layout="Column" CssClass="" runat="server">
                    <Items>
                        <f:TextArea ID="tbxRemark" runat="server" Label="备注">
                        </f:TextArea>
                    </Items>
                </f:Panel>
                <f:Button ID="btnEmployeement" CssClass="marginr" Text="保存" ValidateForms="FormEmployeement" ValidateMessageBox="true" runat="server" OnClick="btnEmployeement_Click">
                </f:Button>
                <f:Button ID="btnClose" Text="关闭" runat="server" EnablePostBack="false"></f:Button>
            </Items>
        </f:Form>

</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>