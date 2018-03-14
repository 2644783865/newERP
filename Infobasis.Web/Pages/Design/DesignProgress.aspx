<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DesignProgress.aspx.cs" Inherits="Infobasis.Web.Pages.Design.DesignProgress"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
    <f:Panel ID="PanelMain" ShowBorder="false" ShowHeader="false" runat="server"
        BodyPadding="10px" AutoScroll="true">
        <Items>
            <f:TabStrip ID="TabStripPage" runat="server" ShowBorder="false" TabPosition="Top" TabBorderColor="true" 
                TabPlain="true" BodyPadding="5px" MarginTop="3px" AutoScroll="false" CssClass="f-tabstrip-theme-simple" Height="525px">
                <Tabs>
                    <f:Tab ID="Tab1" Title="量房" runat="server"></f:Tab>
                    <f:Tab ID="Tab2" Title="设计资料" runat="server"></f:Tab>
                </Tabs>
            </f:TabStrip>
        </Items>
    </f:Panel>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>