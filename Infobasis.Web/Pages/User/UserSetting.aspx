<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSetting.aspx.cs" Inherits="Infobasis.Web.Pages.User.UserSetting" 
    MasterPageFile="~/PageMaster/Popup.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="Panel1" ShowHeader="false" ShowBorder="false" Layout="Fit"
        runat="server">
        <Items>
            <f:SimpleForm ID="SimpleForm1" runat="server" AutoScroll="true" BodyPadding="10px"
                Width="380px" LabelAlign="Left" ShowBorder="false" ShowHeader="false" LabelWidth="120px">
                <Items>
                    <f:Label ID="tbxUserName" runat="server" Label="姓名">
                    </f:Label>
                    <f:DropDownList ID="ddlGridPageSize" Width="200px" Required="true" Label="表格默认记录数" LabelWidth="100px"
                        runat="server">
                        <f:ListItem Text="10" Value="10" />
                        <f:ListItem Text="20" Value="20" />
                        <f:ListItem Text="50" Value="50" />
                        <f:ListItem Text="100" Value="100" />
                    </f:DropDownList>

                    <f:Button ID="btnSave" runat="server" Icon="SystemSave" OnClick="btnSave_OnClick"
                        ValidateForms="SimpleForm1" ValidateTarget="Top" Text="保存设置">
                    </f:Button>
                </Items>
            </f:SimpleForm>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>