<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ckeditor_tabstrip.aspx.cs" ValidateRequest="false"
    Inherits="FineUIPro.Examples.aspnet.ckeditor_tabstrip" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="_form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:TabStrip ID="TabStrip1" Width="800px" Height="450px" ShowBorder="true" TabPosition="Top"
            EnableTabCloseMenu="false" ActiveTabIndex="0"
            runat="server">
            <Tabs>
                <f:Tab ID="Tab1" Title="选项卡一" BodyPadding="5px" Layout="Anchor" runat="server">
                    <Items>
                        <f:HtmlEditor runat="server" Label="编辑器一" ID="HtmlEditor1"
                            Editor="CKEditor" BasePath="~/third-party/res/ckeditor/" ToolbarSet="Full" Height="300px">
                        </f:HtmlEditor>
                    </Items>
                </f:Tab>
                <f:Tab ID="Tab2" Title="选项卡二" BodyPadding="5px" Layout="Fit" runat="server">
                    <Items>
                        <f:HtmlEditor runat="server" Label="编辑器二" ID="HtmlEditor2"
                            Editor="CKEditor" BasePath="~/third-party/res/ckeditor/" ToolbarSet="Basic">
                        </f:HtmlEditor>
                    </Items>
                </f:Tab>
            </Tabs>
            <Toolbars>
                <f:Toolbar runat="server" Position="Bottom">
                    <Items>
                        <f:Button runat="server" Text="获取编辑器的值" ID="Button1" OnClick="Button1_Click"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:TabStrip>
    </form>
</body>
</html>
