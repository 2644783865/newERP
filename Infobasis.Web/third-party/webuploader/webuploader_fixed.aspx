<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="webuploader_fixed.aspx.cs" Inherits="FineUIPro.Examples.thirdparty.webuploader_fixed" %>

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

        .theuploadbutton {
            display: inline-block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:Grid ID="Grid1" runat="server" ShowHeader="true" ShowBorder="true" Title="论文分段上传"
            Width="750px" Height="350px" EnableCollapse="false" EnableCheckBoxSelect="false" EmptyText="尚未上传文件"
            DataIDField="id">
            <Columns>
                <f:RenderField ColumnID="SectionName" DataField="sectionname" HeaderText="章节" Width="80px" />
                <f:RenderField ColumnID="FileName" DataField="name" HeaderText="文件名" ExpandUnusedSpace="True" />
                <f:RenderField ColumnID="FileType" DataField="type" HeaderText="类型" Width="100px" />
                <f:RenderField ColumnID="FileSize" DataField="size" HeaderText="大小" Renderer="FileSize" Width="100px" />
                <f:RenderField ColumnID='FileStatus' DataField="status" HeaderText="状态" Width="100px" RendererFunction="renderStatus" />
                <f:RenderField ColumnID='FileActions' Width="150px" EnableColumnHide="false" RendererFunction="renderActions" />
                <f:RenderField ColumnID="FileSavedName" Hidden="true" EnableColumnHide="false" DataField="savedName"></f:RenderField>
            </Columns>
            <Listeners>
                <f:Listener Event="dataload" Handler="onGridDataLoad" />
            </Listeners>
        </f:Grid>
        <br />
        <br />
        注：由于第三方组件WebUploader限制，本示例支持的浏览器版本为：Chrome、Firefox、Safari、IE10+ 。
    </form>
    <script src="../res/webuploader/webuploader.nolog.js" type="text/javascript"></script>
    <script type="text/javascript">

        var BASE_URL = '<%= ResolveUrl("~/") %>';
        var SERVER_URL = BASE_URL + 'third-party/webuploader/fileupload.ashx';

        function renderStatus(value, params) {
            if (!value) {
                return '尚未上传';
            }
            if (value === 'uploaded') {
                return '上传成功';
            }
        }

        function renderActions(value, params) {
            // 如果正在上传，则删除所有的动作按钮
            if (value === 'uploading') {
                return '';
            }


            var html = '<a href="javascript:;" class="theuploadbutton">上传</a>';

            // 状态、保存到服务器的文件名、
            var fileId = params.rowId;
            var status = params.rowValue['FileStatus'];
            var savedName = params.rowValue['FileSavedName'];

            // 如果已经上传成功，则显示下载按钮
            if (status === 'uploaded') {
                var downloadUrl = BASE_URL + 'upload/' + encodeURIComponent(savedName);
                html += '&nbsp;&nbsp;<a href="' + downloadUrl + '" target="_blank" class="thedownloadbutton">下载</a>';

                html += '&nbsp;&nbsp;<a href="javascript:__doPostBack(\'\',\'DeleteRow_' + fileId + '\');" class="thedeletebutton">删除</a>';
            }

            return html;
        }

        // sizeLimit： 单个文件大小限制，单位为MB
        function initUploader(gridId, rowId, ownerId, accept, sizeLimit, success) {
            var grid = F(gridId);

            var uploaderOptions = {

                // swf文件路径
                swf: BASE_URL + 'third-party/res/webuploader/Uploader.swf',

                // 文件接收服务端。
                server: SERVER_URL,

                // 选择文件的按钮。可选。
                // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                pick: {
                    id: '#' + gridId + ' tr[data-rowid=' + rowId + '] a.theuploadbutton',
                    multiple: false
                },

                // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                resize: false,

                // 自动上传
                auto: true,

                // 文件上传参数表，用来标记文件的所有者（如果是一篇文章的附件，就是文章的ID）
                formData: {
                    owner: ownerId + '#' + rowId
                },

                // 单个文件大小限制（单位：byte），这里限制为 10M
                fileSingleSizeLimit: sizeLimit * 1024 * 1024,

                // 验证文件总数量, 超出则不允许加入队列
                fileNumLimit: 1

            };

            if (accept) {
                $.extend(uploaderOptions, {
                    accept: accept
                });
            }


            var uploader = WebUploader.create(uploaderOptions);

            // 添加到上传队列
            uploader.on('fileQueued', function (file) {
                // 将文件所属的表格行ID保存到 WebUploader 内部的 file 对象上
                file.gridRowId = rowId;

                // 更新当前行的数据（最后一个参数是强制更新，不显示左上角红色标记）
                grid.updateCellValue(rowId, {
                    'FileName': file.name,
                    'FileType': file.ext,
                    'FileSize': file.size,
                    'FileStatus': '等待上传',
                    'FileActions': 'uploading'
                }, true);
            });

            uploader.on('uploadProgress', function (file, percentage) {
                var cellEl = grid.getCellEl(file.gridRowId, 'FileStatus').find('.f-grid-cell-inner');

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
                grid.updateCellValue(file.gridRowId, 'FileStatus', '上传成功', true);
            });

            uploader.on('uploadError', function (file) {
                grid.updateCellValue(file.gridRowId, 'FileStatus', '上传出错', true);
            });

            // 不管上传成功还是失败，都会触发 uploadComplete 事件
            uploader.on('uploadComplete', function (file) {
                uploader.removeFile(file, true);

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

        var gridClientID = '<%= Grid1.ClientID %>';

        function onGridDataLoad(event) {
            var grid = this;

            grid.el.find('a.theuploadbutton').each(function () {
                var targetEl = $(this);
                var rowEl = targetEl.parents('.f-grid-row');
                var rowData = grid.getRowData(rowEl);

                initUploader(gridClientID, rowData.id, 'webuploader.webuploader_fixed', {
                    mimeTypes: '.doc,.docx'
                }, 10, function () {
                    __doPostBack('', 'RebindGrid');
                });
            });
        }
        
    </script>
</body>
</html>
