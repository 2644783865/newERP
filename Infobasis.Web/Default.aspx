<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Infobasis.Web._default" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>创域国际家居ERP系统</title>
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <meta name="Title" content="创域国际家居ERP系统" />
    <meta name="Description" content="创域国际家居ERP系统" />
    <meta name="Keywords" content="创域国际家居ERP系统" />
    <link type="text/css" rel="stylesheet" href="~/res/css/default.css" />
    <meta name="sourcefiles" content="~/captcha/captcha.ashx;~/captcha/CaptchaImage.cs" />

    <script type="text/javascript">
    
        // 本页面一定是顶层窗口，不会嵌在IFrame中
        if (top.window != window) {
            alert('登录超时, 请重新登录');
            top.window.location.href = "./Default.aspx";
        }

        // 将 localhost 转换为 localhost/default.aspx
        if (window.location.href.indexOf('/Default.aspx') < 0) {
            window.location.href = "./Default.aspx";
        }

    </script>

    <style>
        .imgcaptcha .f-field-label {
            margin: 0;
        }

        .login-image {
            border-width: 0 1px 0 0;
            width: 116px;
            height: 116px;
            background-size:unset;
            background-repeat:no-repeat;
        }

        .login-image .ui-icon {
            font-size: 96px;
        }

        .loginWindow {
            margin-top:60px;
            margin-left:60px;
            background-color:#fff;
            border:0;
        }

        .s-skin-container {
            position: fixed;
            _position: absolute;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            min-width: 1000px;
            z-index: -10;
            background-position: center 0;
            background-repeat: no-repeat;
            background-size: cover;
            -webkit-background-size: cover;
            -o-background-size: cover;
            zoom: 1;
            background-image: url("res/images/bg/bg_index2.jpg");
        }

    </style>
</head>
<body>
    <div class="s-skin-container s-isindex-wrap" style="background-color:#404040;">  </div>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:Window ID="Window1" runat="server" Title="创域国际家居ERP系统" IsModal="false" EnableClose="false" IconFont="SignIn" ShowBorder="true" ShowHeader="true" CssClass="loginWindow"
            WindowPosition="GoldenSection" Width="450px" Layout="HBox" EnableDrag ="false" PositionX="Left" PositionY="Top" FixedPosition="true" CssStyle="" AutoScroll="true">
            <Items>
                <f:ContentPanel ID="ContentPanel1" CssClass="login-image" BodyPadding="10px" ShowBorder="false" ShowHeader="false" runat="server">
                    <i class="ui-icon f-icon-key"></i>
                </f:ContentPanel>
                <f:SimpleForm ID="SimpleForm1" runat="server" BoxFlex="1" ShowBorder="false" BodyPadding="10px"
                    LabelWidth="80px" ShowHeader="false" LabelAlign="Right" AutoScroll="true">
                    <Items>
                        <f:TextBox ID="tbxCompanyCode" Label="公司代号" Required="true" runat="server"  NextFocusControl="tbxUserName" >
                        </f:TextBox>
                        <f:TextBox ID="tbxUserName" Label="用户名" Required="true" runat="server" NextFocusControl="tbxPassword">
                        </f:TextBox>
                        <f:TextBox ID="tbxPassword" Label="密码" TextMode="Password" Required="true" runat="server" NextFocusControl="tbxCaptcha">
                        </f:TextBox>
                        <f:Panel ID="panelCaptcha" ShowBorder="false" ShowHeader="false" Layout="HBox" BoxConfigAlign="Stretch" runat="server">
                            <Items>
                                <f:TextBox ID="tbxCaptcha" BoxFlex="1" Margin="0 5px 0 0" Label="验证码" Required="true" runat="server" NextFocusControl="imgCaptcha">
                                </f:TextBox>
                                <f:LinkButton ID="imgCaptcha" CssClass="imgcaptcha" Width="100px" EncodeText="false" runat="server" OnClick="imgCaptcha_Click">
                                </f:LinkButton>
                            </Items>
                        </f:Panel>
                    </Items>
                </f:SimpleForm>
            </Items>
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Bottom" ToolbarAlign="Right">
                    <Items>
                        <f:HyperLink ID="linkFindPassword" Text="忘记密码?" RegionPosition="Left" Target="_blank" LabelAlign="Left" NavigateUrl="/FindPassword.aspx"
                            runat="server" BoxConfigPosition="Left">
                        </f:HyperLink>
                        <f:Button ID="btnLogin" Text="登录" Type="Submit" ValidateForms="SimpleForm1" ValidateTarget="Top"
                            runat="server" OnClick="btnLogin_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:Window>
    </form>
</body>
<script>
    // 通知框
    function notify(msg) {
        F.notify({
            message: msg,
            messageIcon: 'information',
            target: '_top',
            header: false,
            displayMilliseconds: 3 * 1000,
            positionX: 'center',
            positionY: 'center'
        });
    }
</script>
</html>

