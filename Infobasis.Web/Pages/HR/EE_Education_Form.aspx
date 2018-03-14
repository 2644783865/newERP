<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EE_Education_Form.aspx.cs" Inherits="Infobasis.Web.Pages.HR.EE_Education_Form"
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="FormEducation" runat="server" />
    <f:Form ID="FormEducation" MessageTarget="Qtip" ShowHeader="false" LabelAlign="Right" LabelWidth="90px" Width="600px" BodyPadding="5px" Title="" runat="server">
            <Items>
                <f:Panel ID="Panel3" ShowHeader="false" CssClass="" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:Label ID="Label2" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                            Text="学校/机构：">
                        </f:Label>
                        <f:TextBox ID="tbxEducationalInstitution" ShowLabel="false" Label="学校/机构" Required="true" Width="200px" CssClass="marginr" runat="server">
                        </f:TextBox>
                        <f:Label ID="Label4" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                            Text="专业：">
                        </f:Label>
                        <f:TextBox ID="tbxMajor" ShowLabel="false" Label="专业" Required="true" Width="100px" CssClass="marginr" runat="server">
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
                <f:Panel ID="Panel6" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:Label ID="Label1" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                            Text="学位：">
                        </f:Label>
                        <f:TextBox ID="tbxAcademicDegree" ShowLabel="false" Label="学位" Required="false" Width="150px" CssClass="marginr" runat="server">
                        </f:TextBox>
                        <f:Label ID="Label6" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                            Text="学历：">
                        </f:Label>
                        <f:DropDownList runat="server" Label="学历" ShowLabel="false" ID="DropDownEducation">
                            <f:ListItem Text="" Value="0" Selected="true" />
                            <f:ListItem Text="博士研究生" Value="1" />
                            <f:ListItem Text="硕士研究生" Value="2" />
                            <f:ListItem Text="大学本科" Value="3" />
                            <f:ListItem Text="大学专科" Value="4" />
                            <f:ListItem Text="高职及中专" Value="5" />
                            <f:ListItem Text="高中" Value="6" />
                            <f:ListItem Text="初中" Value="7" />
                            <f:ListItem Text="其他" Value="8" />
                        </f:DropDownList>
                    </Items>
                </f:Panel>
                <f:Panel ID="Panel7" ShowHeader="false" ShowBorder="false" Layout="Column" CssClass="" runat="server">
                    <Items>
                        <f:Label ID="Label3" Width="100px" runat="server" CssClass="marginr" ShowLabel="false"
                            Text="教育类型：">
                        </f:Label>
                        <f:DropDownList runat="server" Label="教育类型" ShowLabel="false" ID="DropDownEducationType">
                            <f:ListItem Text="" Value="0" Selected="true" />
                            <f:ListItem Text="全日制" Value="1" />
                            <f:ListItem Text="在职教育" Value="2" />
                        </f:DropDownList>
                        <f:CheckBox ID="cbxIsHighest" runat="server" Checked="false" Label="是否为最高学历">
                        </f:CheckBox>
                    </Items>
                </f:Panel>
                <f:Panel ID="Panel13" ShowHeader="false" ShowBorder="false" Layout="Column" CssClass="" runat="server">
                    <Items>
                        <f:TextArea ID="tbxRemark" runat="server" Label="备注">
                        </f:TextArea>
                    </Items>
                </f:Panel>
                <f:Button ID="btnEducation" CssClass="marginr" Text="保存" ValidateForms="FormEducation" ValidateMessageBox="true" runat="server" OnClick="btnEducation_Click">
                </f:Button>
                <f:Button ID="btnClose" Text="关闭" runat="server" EnablePostBack="false"></f:Button>
            </Items>
        </f:Form>

</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>