<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="webuploader_test.aspx.cs" Inherits="FineUIPro.Examples.thirdparty.webuploader_test" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../res/css/main.css" rel="stylesheet" type="text/css" />
    <meta name="sourcefiles" content="~/third-party/webuploader/fileupload.ashx" />
    <style>
        .webuploader-container {
            position: relative;
        }

        .webuploader-element-invisible {
            position: absolute !important;
            clip: rect(1px,1px,1px,1px);
        }

        .webuploader-pick-disable {
            opacity: 0.6;
            pointer-events: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" OnCustomEvent="PageManager1_CustomEvent" />
        <f:Grid ID="Grid1" runat="server" ShowHeader="true" ShowBorder="true" Title="文件上传"
            Width="650px" Height="350px"
            EnableCollapse="false" EnableCheckBoxSelect="false" EmptyText="尚未上传文件"
            DataIDField="id" OnRowCommand="Grid1_RowCommand">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:Button ID="btnSelectFiles" Text="选择文件上传" IconFont="Plus" runat="server" EnablePostBack="false">
                        </f:Button>
                        <f:ToolbarSeparator runat="server"></f:ToolbarSeparator>
                        <f:Button ID="btnDelete" Text="删除选中文件" IconFont="Remove" runat="server" OnClick="btnDelete_Click">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns>
                <f:BoundField ColumnID="FileName" DataField="name" HeaderText="文件名" ExpandUnusedSpace="True" />
                <f:BoundField ColumnID="FileType" DataField="type" HeaderText="类型" Width="100px" />
                <f:RenderField ColumnID="FileSize" DataField="size" HeaderText="大小" Renderer="FileSize" Width="100px" />
                <f:BoundField ColumnID='FileStatus' DataField="status" NullDisplayText="已完成" HeaderText="状态" Width="100px" />
                <f:LinkButtonField Width="80px" ConfirmText="你确定要删除这个文件吗？" ConfirmTarget="Top"
                    CommandName="Delete" IconUrl="~/res/icon/delete.png" />
            </Columns>
        </f:Grid>
        <f:HtmlEditor runat="server" Label="内容" ID="ed_Content" Editor="UMEditor" BasePath="~/third-party/res/umeditor/" Height="250px">
                                </f:HtmlEditor>
    </form>
    <script src="../res/webuploader/webuploader.nolog.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var btnSelectFilesClientID = '<%= btnSelectFiles.ClientID %>';
        var gridClientID = '<%= Grid1.ClientID %>';

        var BASE_URL = '<%= ResolveUrl("~/") %>';
        var SERVER_URL = BASE_URL + 'third-party/webuploader/fileupload.ashx';

        F.ready(function () {
            var grid = F(gridClientID);

            var uploader = WebUploader.create({

                // swf文件路径
                swf: BASE_URL + 'third-party/res/webuploader/Uploader.swf',

                // 文件接收服务端。
                server: SERVER_URL,

                // 选择文件的按钮。可选。
                // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                pick: '#' + btnSelectFilesClientID,

                // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                resize: false,

                // 自动上传
                auto: true,

                // 文件上传参数表，用来标记文件的所有者（如果是一篇文章的附件，就是文章的ID）
                formData: {
                    owner: 'webuploader.webuploader'
                },

                // 单个文件大小限制（单位：byte），这里限制为 10M
                fileSingleSizeLimit: 10 * 1024 * 1024

            });

            // 添加到上传队列
            uploader.on('fileQueued', function (file) {
                grid.addNewRecord(file.id, {
                    'FileName': file.name,
                    'FileType': file.ext,
                    'FileSize': file.size,
                    'FileStatus': '等待上传'
                }, true);
            });

            uploader.on('uploadProgress', function (file, percentage) {
                var cellEl = grid.getCellEl(file.id, 'FileStatus').find('.f-grid-cell-inner');
                
                var barEl = cellEl.find('.ui-progressbar-value');

                // 避免重复创建
                if (!barEl.length) {
                    cellEl.html('<div class="ui-progressbar ui-widget ui-widget-content ui-corner-all" style="height:12px;">' +
                      '<div class="ui-progressbar-value ui-widget-header ui-corner-left" style="width:0%">' +
                      '</div></div>');
                    barEl = cellEl.find('.ui-progressbar-value');
                }

                barEl.css('width', percentage * 100 + '%');
            });

            uploader.on('uploadSuccess', function (file) {
                var cellEl = grid.getCellEl(file.id, 'FileStatus').find('.f-grid-cell-inner');
                cellEl.text('上传成功');
            });

            uploader.on('uploadError', function (file) {
                var cellEl = grid.getCellEl(file.id, 'FileStatus').find('.f-grid-cell-inner');
                cellEl.text('上传出错');
            });

            // 不管上传成功还是失败，都会触发 uploadComplete 事件
            uploader.on('uploadComplete', function (file) {
                uploader.removeFile(file, true);
            });


            // 所有文件上传成功
            uploader.on('uploadFinished', function () {
                F.customEvent('RebindGrid');
            });


            uploader.on('error', function (type, arg, file) {
                if (type === 'F_EXCEED_SIZE') {
                    F.alert('文件[' + file.name + ']大小超出限制值（' + F.format.fileSize(arg) + '）');
                }
            });
        });
    </script>
</body>
</html>
