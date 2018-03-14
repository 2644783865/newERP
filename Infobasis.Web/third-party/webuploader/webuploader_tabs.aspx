<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="webuploader_tabs.aspx.cs" Inherits="FineUIPro.Examples.thirdparty.webuploader_tabs" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <meta name="sourcefiles" content="~/third-party/webuploader/fileupload.ashx" />
    <style>
        
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
        <f:PageManager ID="PageManager1" runat="server" />
        <f:TabStrip ID="TabStrip1" Width="800px" Height="350px" ShowBorder="true" TabPosition="Top"
            EnableTabCloseMenu="false" ActiveTabIndex="0"
            runat="server">
            <Tabs>
                <f:Tab ID="Tab1" Title="上传压缩文档" BodyPadding="5px" Layout="Fit"
                    runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" ShowHeader="false" ShowBorder="true"
                            Height="250px"
                            EnableCollapse="false" EnableCheckBoxSelect="false" EmptyText="尚未上传文件"
                            DataIDField="id" OnRowCommand="Grid1_RowCommand">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button runat="server" ID="btnSelectFiles" EnablePostBack="false" IconFont="Plus" Text="选择文件上传"></f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField ColumnID="FileName" DataField="name" HeaderText="文件名" ExpandUnusedSpace="True" />
                                <f:RenderField ColumnID="FileSize" DataField="size" HeaderText="大小" Renderer="FileSize" Width="100px" />
                                <f:BoundField ColumnID='FileStatus' DataField="status" NullDisplayText="已完成" HeaderText="状态" Width="100px" />
                                <f:LinkButtonField Width="60px" ConfirmText="你确定要删除这个文件吗？" ConfirmTarget="Top"
                                    CommandName="Delete" IconUrl="~/res/icon/delete.png" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Tab>
                <f:Tab ID="Tab2" Title="上传图片文档" BodyPadding="5px" Layout="Fit"
                    runat="server">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" ShowHeader="false" ShowBorder="true"
                            Height="250px"
                            EnableCollapse="false" EnableCheckBoxSelect="false" EmptyText="尚未上传图片"
                            DataIDField="id" OnRowCommand="Grid2_RowCommand">
                            <Toolbars>
                                <f:Toolbar runat="server">
                                    <Items>
                                        <f:Button runat="server" ID="btnSelectFiles2" EnablePostBack="false" IconFont="Plus" Text="选择图片上传"></f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField ColumnID="FileName" DataField="name" HeaderText="文件名" ExpandUnusedSpace="True" />
                                <f:RenderField ColumnID="FileSize" DataField="size" HeaderText="大小" Renderer="FileSize" Width="100px" />
                                <f:BoundField ColumnID='FileStatus' DataField="status" NullDisplayText="已完成" HeaderText="状态" Width="100px" />
                                <f:LinkButtonField Width="60px" ConfirmText="你确定要删除这个图片吗？" ConfirmTarget="Top"
                                    CommandName="Delete" IconUrl="~/res/icon/delete.png" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Tab>
            </Tabs>
            <Listeners>
                <f:Listener Event="tabchange" Handler="onTabStripChange" />
            </Listeners>
        </f:TabStrip>
        <br />
        <br />
        注：由于第三方组件WebUploader限制，本示例支持的浏览器版本为：Chrome、Firefox、Safari、IE10+ 。
    </form>
    <script src="../res/webuploader/webuploader.nolog.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var BASE_URL = '<%= ResolveUrl("~/") %>';
        var SERVER_URL = BASE_URL + 'third-party/webuploader/fileupload.ashx';

        // sizeLimit： 单个文件大小限制，单位为MB
        function initUploader(gridId, pickerId, ownerId, accept, sizeLimit, success) {
            var grid = F(gridId);

            var uploaderOptions = {

                // swf文件路径
                swf: BASE_URL + 'third-party/res/webuploader/Uploader.swf',

                // 文件接收服务端。
                server: SERVER_URL,

                // 选择文件的按钮。可选。
                // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                pick: '#' + pickerId,

                // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                resize: false,

                // 自动上传
                auto: true,

                // 文件上传参数表，用来标记文件的所有者（如果是一篇文章的附件，就是文章的ID）
                formData: {
                    owner: ownerId
                },

                // 单个文件大小限制（单位：byte），这里限制为 10M
                fileSingleSizeLimit: sizeLimit * 1024 * 1024

            };

            if (accept) {
                $.extend(uploaderOptions, {
                    accept: accept
                });
            }


            var uploader = WebUploader.create(uploaderOptions);

            // 添加到上传队列
            uploader.on('fileQueued', function (file) {
                grid.addNewRecord(file.id, {
                    'FileName': file.name,
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
                if (success) {
                    success.call(uploader);
                }
            });


            uploader.on('error', function (type, arg, file) {
                if (type === 'F_EXCEED_SIZE') {
                    F.alert('文件[' + file.name + ']大小超出限制值（' + F.format.fileSize(arg) + '）');
                }
            });
        }


        // 如果初始化 WebUploader 时按钮处于隐藏状态，则点击上传按钮无效。这里是做了一个补救措施
        function onTabStripChange(event, tab) {
            var container = tab.el.find('.webuploader-container');
            var pick = container.find('.webuploader-pick');
            var file = pick.next();

            var pickPos = pick.position();

            file.css({
                width: pick.width(),
                height: pick.height(),
                top: pickPos.top,
                left: pickPos.left
            });
        }


        var btnSelectFiles1ClientID = '<%= btnSelectFiles.ClientID %>';
        var grid1ClientID = '<%= Grid1.ClientID %>';

        var btnSelectFiles2ClientID = '<%= btnSelectFiles2.ClientID %>';
        var grid2ClientID = '<%= Grid2.ClientID %>';

        F.ready(function () {

            initUploader(grid1ClientID, btnSelectFiles1ClientID, 'webuploader.webuploader_tabs.1', undefined, 10, function () {
                __doPostBack('', 'RebindGrid1');
            });


            initUploader(grid2ClientID, btnSelectFiles2ClientID, 'webuploader.webuploader_tabs.2', {
                title: 'Images',
                extensions: 'gif,jpg,jpeg,bmp,png',
                mimeTypes: 'image/*'
            }, 1, function () {
                __doPostBack('', 'RebindGrid2');
            });

        });



    </script>
</body>
</html>
