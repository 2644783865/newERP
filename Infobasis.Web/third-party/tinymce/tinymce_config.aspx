<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tinymce_config.aspx.cs" ValidateRequest="false"
    Inherits="FineUIPro.Examples.aspnet.tinymce_config" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" runat="server" LabelAlign="Top" EnableCollapse="true"
            Title="表单" Width="850px">
            <Items>
                <f:HtmlEditor runat="server" Label="文本编辑器" ID="HtmlEditor1"
                    Editor="TinyMCE" BasePath="~/third-party/res/tinymce/" ToolbarSet="Full" Height="350px">
                    <Options>
                        <f:OptionItem Key="menubar" Value="false" PersistOriginal="true" />
                        <f:OptionItem Key="plugins" Value="['link','code','textcolor colorpicker']" PersistOriginal="true" />
                        <f:OptionItem Key="toolbar1" Value="bold italic underline strikethrough | numlist bullist | alignleft aligncenter alignright | link unlink | code" />
                    </Options>
                </f:HtmlEditor>
            </Items>
        </f:SimpleForm>
        <br />
        <f:Button ID="Button2" runat="server" CssClass="marginr" Text="设置编辑器的值" OnClick="Button2_Click">
        </f:Button>
        <f:Button ID="Button1" runat="server" Text="获取编辑器的值" OnClick="Button1_Click">
        </f:Button>
        <br />
        <br />
        注：本示例通过 Options 属性设置 TinyMCE 的菜单栏（无）和工具栏（自定义）。参考文档：http://www.tinymce.com/wiki.php/Configuration
    </form>
</body>
</html>
