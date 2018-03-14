<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ueditor_window.aspx.cs" ValidateRequest="false"
    Inherits="FineUIPro.Examples.aspnet.ueditor_window" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:Window ID="Window1" runat="server" Width="900px" Height="500px" Layout="Fit" BodyPadding="5px" EnableCollapse="true"
            Title="简单表单" IsModal="false" EnableResize="true">
            <Items>
                <f:SimpleForm ID="SimpleForm1" Layout="Fit" runat="server" LabelAlign="Top" ShowBorder="false" ShowHeader="false">
                    <Items>
                        <f:HtmlEditor runat="server" Label="文本编辑器" ID="HtmlEditor1"
                            Editor="UEditor" BasePath="~/third-party/res/ueditor/" ToolbarSet="Full">
                            <Options>
                                <f:OptionItem Key="zIndex" Value="99999" />
                            </Options>
                        </f:HtmlEditor>
                    </Items>
                </f:SimpleForm>
            </Items>
            <Toolbars>
                <f:Toolbar Position="Bottom" ToolbarAlign="Right" runat="server">
                    <Items>
                        <f:Button ID="Button2" runat="server" CssClass="marginr" Text="设置编辑器的值" OnClick="Button2_Click">
                        </f:Button>
                        <f:Button ID="Button1" runat="server" Text="获取编辑器的值" OnClick="Button1_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:Window>
        <br />
        注：本示例不支持文件上传，请根据UEditor官网文档自行配置。
    </form>
</body>
</html>
