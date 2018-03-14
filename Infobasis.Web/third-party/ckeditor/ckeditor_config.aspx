<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ckeditor_config.aspx.cs" ValidateRequest="false"
    Inherits="FineUIPro.Examples.aspnet.ckeditor_config" %>

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
                    Editor="CKEditor" BasePath="~/third-party/res/ckeditor/" ToolbarSet="Full" Height="350px">
                    <Options>
                        <f:OptionItem Key="uiColor" Value="#aed0ea" />
                        <f:OptionItem Key="language" Value="zh" />
                        <f:OptionItem Key="toolbar"
                            Value="[['Bold','Italic','Underline','Strike'],['NumberedList','BulletedList'],['JustifyLeft','JustifyCenter','JustifyRight'],['Link','Unlink'],['Source']]"
                            PersistOriginal="true" />
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
        注：本示例通过 Options 属性设置 CKEditor 的背景色（浅蓝色），语言（繁体中文）和工具栏（自定义）。参考文档：http://docs.ckeditor.com/#!/guide/dev_configuration
    </form>
</body>
</html>
