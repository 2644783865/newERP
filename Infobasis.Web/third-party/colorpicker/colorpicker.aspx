<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="colorpicker.aspx.cs" Inherits="FineUIPro.Examples.third_party.colorpicker.colorpicker" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../res/spectrum/spectrum.css" />
    <style>
        .mycolor .f-field-body {
            display: inline-block !important;
            width: 150px;
            margin-right: 5px;
        }

        .mycolor .sp-replacer {
            border-width: 0;
            padding: 0;
            position: absolute;
            top: 50%;
            margin-top: -10px;
        }

        .mycolor .sp-dd {
            display: none;
        }

        .mycolor .sp-preview {
            margin-right: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:SimpleForm ID="SimpleForm1" BodyPadding="5px" runat="server" Width="450px" EnableCollapse="true"
            ShowBorder="True" Title="表单" ShowHeader="True">
            <Items>
                <f:DatePicker runat="server" Required="true" Label="日期一" EmptyText="请选择日期一"
                    ID="DatePicker1" ShowRedStar="True">
                </f:DatePicker>
                <f:TextBox ID="tbxMyBox" CssClass="mycolor" Text="#f90" Required="true" ShowRedStar="True" Label="选择颜色" runat="server">
                </f:TextBox>
                <f:Button ID="btnSubmit" runat="server" ValidateForms="SimpleForm1" Text="提交表单"
                    OnClick="btnSubmit_Click">
                </f:Button>
            </Items>
        </f:SimpleForm>
        <f:Label ID="labResult" ShowLabel="false" EncodeText="false" runat="server">
        </f:Label>
    </form>
    <script src="../res/spectrum/spectrum.js" type="text/javascript"></script>
    <script src="../res/spectrum/i18n/jquery.spectrum-zh-cn.js" type="text/javascript"></script>
    <script type="text/javascript">
        var tbxMyBoxClientID = '<%= tbxMyBox.ClientID %>';

        F.ready(function () {

            var tbxMyBox = F(tbxMyBoxClientID);

            tbxMyBox.bodyEl.spectrum({
                preferredFormat: "hex3",
                showInput: true,
                showPalette: true,
                palette: [
                    ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                    ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                    ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                    ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                    ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                    ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                    ["#900", "#b45f06", "#bf9000", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                    ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
                ]
            });


        });
    </script>
</body>
</html>
