<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ueditor.aspx.cs" ValidateRequest="false"
    Inherits="FineUIPro.Examples.aspnet.ueditor" %>

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
                <f:HtmlEditor runat="server" Label="文本编辑器" ID="HtmlEditor1" Readonly="true"
                    Editor="UEditor" BasePath="~/third-party/res/ueditor/" ToolbarSet="Full" Height="350px">
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
        <f:Button ID="Button3" runat="server" CssClass="marginr" Text="设置编辑器只读" OnClick="Button3_Click">
        </f:Button>
        <f:Button ID="Button4" runat="server" Text="取消编辑器只读" OnClick="Button4_Click">
        </f:Button>
        <br />
        <br />
        注：
        <ul>
            <li>默认编辑器处于只读状态。</li>
            <li>本示例不支持文件上传，请根据UEditor官网文档自行配置。</li>
        </ul>
    </form>
</body>
</html>
