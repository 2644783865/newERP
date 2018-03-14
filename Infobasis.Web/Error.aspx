<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Infobasis.Web.Error"
    MasterPageFile="~/PageMaster/Page.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style>
        .errorBox {
            border: solid 1px red;
            padding: 5px;
        }

        .debugBox {
            border: solid 1px red;
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <f:Label ID="errorLabel" EncodeText="false" CssClass="errorBox" Text="" runat="server" />
    <f:Label ID="debugLabel" EncodeText="false" CssClass="debugBox" Text="" runat="server" />
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
</asp:Content>