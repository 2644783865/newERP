<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cloud.aspx.cs" Inherits="Infobasis.Web.Pages.OA.Cloud" 
     MasterPageFile="~/PageMaster/SPA.master" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link type="text/css" rel="stylesheet" href="/res/css/photoswipe.css" />
    <link type="text/css" rel="stylesheet" href="/res/js/default-skin/default-skin.css" />
    <style>

        a {
            text-decoration: none;
            color: #111;
        }
        .btn {
            display: inline-block;
            margin-bottom: 0;
            padding: .1em 0.5em;
            vertical-align: middle;
            font-size: 1rem;
            font-weight: 400;
            line-height: 1.2;
            text-align: center;
            white-space: nowrap;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 0;
            cursor: pointer;
            outline: 0;
            -webkit-appearance: none;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            -webkit-transition: background-color .3s ease-out,border-color .3s ease-out;
            transition: background-color .3s ease-out,border-color .3s ease-out;
        }

        .btn-primary {
            color: #fff;
            background-color: #0e90d2;
            border-color: #0e90d2;
        }

        .btn-success {
            color: #fff;
            background-color: #5eb95e;
            border-color: #5eb95e;
        }

        .btn-warning {
            color: #fff;
            background-color: #F37B1D;
            border-color: #F37B1D;
        }

        .btn-danger {
            color: #fff;
            background-color: #dd514c;
            border-color: #dd514c;
        }

        .btn:disabled {
            color: #c3c3c3;
            background: #d7e9f6;
            border-color: #d7e9f6;
        }

        .btn-primary:focus, .btn-primary:hover {
            background-color: #0c79b1;
        }

        .btn-success:focus, .btn-success:hover {
            background-color: #4aaa4a;
        }

        .folder-list-entry {
            margin: 0;
            padding: 0;
            list-style: none;
        }
        .folder-list-entry li {
            position: relative;
            font-size: 16px;
            border-bottom: 1px solid #ededed;
        }
        .folder-list-entry li a {
            word-break: break-all;
            padding: 15px 60px 15px 15px;
            display: block;
            line-height: 1.2;
            transition: color 0.4s;
            text-decoration: none;
            color: #1b6aaa;
        }

        .folder-list-entry li a i {
            padding-right: 2px;
        }

        .float-right {
            float: right;
        }
        .body-box {
            border-top: 1px solid #ddd;
            margin-top: 10px;        
        }
        .upload-list {
            overflow: hidden;
        }

        .folder-list,
        .upload-list {
            padding: 0;
            margin: 0;
        }
        .folder-list li,
        .upload-list li {
            list-style:none;
        }

        .upload-list li {
            float: left;
            margin: 10px;
            border: 1px solid #e2e2e2;
            padding: 10px;
            width: 135px;
            height: 138px;
            border-radius: 2px;
            box-shadow: 0 1px 5px rgba(50, 50, 50, .1);
        }

        .upload-list li:hover {
            border: 1px solid #0e90d2;
        }

        .upload-list li span {
            display:block;
            margin-top: 2px;
            max-height: 1.5em;
            position: relative;
            overflow: hidden;
        }

        .upload-list li span:after {
            conent: "";
            content: "";
            display: block;
            position: absolute;
            bottom: 0;
            width: 100%;
            height: 1em;
            background: linear-gradient(360deg,#fff 5%,rgba(255,255,255,0) 100%);
        }

        .upload-list li a {
            cursor: pointer;
        }

        .upload-list li img {
            width: 120px;
            height: 120px;
        }

        .upload-list li input {
            height: 30px;
            line-height: 30px;
            width: 200px;
        }

        .folder-list li {
            position: relative;
            font-size: 16px;
            border-bottom: 1px solid #ededed;
        }
        .folder-list li a {
            word-break: break-all;
            padding: 15px 60px 15px 0;
            display: block;
            line-height: 1.2;
            transition: color 0.4s;
            text-decoration: none;
            color: #1b6aaa;
        }

        .modal-footer span {
            font-size: 12px;
        }

        .my-drop-zone { 
            border: dotted 3px lightgray;
            width: 200px;
            margin: 0 auto;
            text-align: center;
            color: rgba(66, 185, 131, 0.64);
            height: 100px;
            margin-top: 5px;
            padding-top: 10px;
        }
        .nv-file-over { border: dotted 3px red; } /* Default class applied to drop zones on over */
        .another-file-over-class { border: dotted 3px green; }
        html, body { height: 100%; }
        canvas {
            background-color: #f3f3f3;
            -webkit-box-shadow: 3px 3px 3px 0 #e3e3e3;
            -moz-box-shadow: 3px 3px 3px 0 #e3e3e3;
            box-shadow: 3px 3px 3px 0 #e3e3e3;
            border: 1px solid #c3c3c3;
            height: 100px;
            margin: 6px 0 0 6px;
        }

        .no-file-msg {
            color: #1b6aaa;
            border: 1px solid #29d;
            padding: 10px;
            border-left: 10px solid #29d;
            margin-top: 10px;
        }

        .upload-status {
            color: red;
        }

        .upload-status .fa-check {
            color: green;
        }

    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div ng-view></div>
    <!-- Root element of PhotoSwipe. Must have class pswp. -->
    <div class="pswp" tabindex="-1" role="dialog" aria-hidden="true">

        <!-- Background of PhotoSwipe. 
             It's a separate element, as animating opacity is faster than rgba(). -->
        <div class="pswp__bg"></div>

        <!-- Slides wrapper with overflow:hidden. -->
        <div class="pswp__scroll-wrap">

            <!-- Container that holds slides. PhotoSwipe keeps only 3 slides in DOM to save memory. -->
            <div class="pswp__container">
                <!-- don't modify these 3 pswp__item elements, data is added later on -->
                <div class="pswp__item"></div>
                <div class="pswp__item"></div>
                <div class="pswp__item"></div>
            </div>

            <!-- Default (PhotoSwipeUI_Default) interface on top of sliding area. Can be changed. -->
            <div class="pswp__ui pswp__ui--hidden">

                <div class="pswp__top-bar">

                    <!--  Controls are self-explanatory. Order can be changed. -->

                    <div class="pswp__counter"></div>

                    <button class="pswp__button pswp__button--close" title="Close (Esc)"></button>

                    <button class="pswp__button pswp__button--share" title="Share"></button>

                    <button class="pswp__button pswp__button--fs" title="Toggle fullscreen"></button>

                    <button class="pswp__button pswp__button--zoom" title="Zoom in/out"></button>

                    <!-- Preloader demo https://codepen.io/dimsemenov/pen/yyBWoR -->
                    <!-- element will get class pswp__preloader--active when preloader is running -->
                    <div class="pswp__preloader">
                        <div class="pswp__preloader__icn">
                          <div class="pswp__preloader__cut">
                            <div class="pswp__preloader__donut"></div>
                          </div>
                        </div>
                    </div>
                </div>

                <div class="pswp__share-modal pswp__share-modal--hidden pswp__single-tap">
                    <div class="pswp__share-tooltip"></div> 
                </div>

                <button class="pswp__button pswp__button--arrow--left" title="Previous (arrow left)">
                </button>

                <button class="pswp__button pswp__button--arrow--right" title="Next (arrow right)">
                </button>

                <div class="pswp__caption">
                    <div class="pswp__caption__center"></div>
                </div>

              </div>

            </div>

    </div>

</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="/res/js/app/cloudApp.js"></script>
    <script src="/res/js/es5-shim.min.js"></script>
    <script src="/res/js/es5-sham.min.js"></script>
    <script src="/res/js/angular-file-upload.min.js"></script>
    <script src="/res/js/photoswipe.min.js"></script>
    <script src="/res/js/photoswipe-ui-default.min.js"></script>
    <script>
        //var app = angular.module('infobasisApp');
        app.controller('cloudMyFileController',
        ['$scope', '$route', '$routeParams', 'cfpLoadingBar', 'FileUploader', '$window', '$location',
        function ($scope, $route, $routeParams, cfpLoadingBar, FileUploader, $window, $location) {
            $scope.$route = $route;
            $scope.showModal = false;
            $scope.newFolderName = '';
            $scope.folderCode = $routeParams.id;
            $scope.showUploadBox = false;
            $scope.hashRoute = $location.path();
            $scope.isPublic = $scope.hashRoute.indexOf('/cloud/my') >= 0 ? false : true;
            $scope.routeRoot = !$scope.isPublic ? '/cloud/my' : '/cloud/public';

            $scope.showUploadBoxFn = function () {
                $scope.showUploadBox = !$scope.showUploadBox;
            }

            $scope.canUpload = $scope.folderCode != null && $scope.folderCode.length > 0 ? true : false;

            $scope.uploads = [];
            $scope.folders = [];
            $scope.showModalBox = function () {
                $scope.showModal = true;
            };
            $scope.closeModal = function () {
                $scope.showModal = false;
            };
            $scope.saveData = function () {
                var val = $scope.newFolderName;
                if (val == null || val.length == 0)
                    return;

                var newFolder = {
                    name: val,
                    isPublic: $scope.isPublic
                };

                var folderCode = $scope.folderCode != undefined && $scope.folderCode != null ? $scope.folderCode : '0';
                //console.log(folderCode);
                infobasisService.getAjaxInstance.post('/cloudfile/folderlist/' + folderCode, newFolder)
                  .then(function (response) {
                      var data = response.data;
                      $scope.folders.push(data);
                      $scope.showModal = false;
                      $scope.newFolderName = '';
                      $scope.$apply();
                  })
                  .catch(function (error) {
                      console.log(error);
                  });
            };

            $scope.currentFolderInfo = { name: ''};
            function loadCurrentFolderInfo() {
                var folderCode = $scope.folderCode;
                if (folderCode == undefined || folderCode == null) {
                    $scope.currentFolderInfo.name = '我的文件';
                } else {
                    infobasisService.getAjaxInstance.get('/cloudfile/folder/' + folderCode)
                      .then(function (response) {
                          var data = response.data;
                          $scope.currentFolderInfo = data;
                          cfpLoadingBar.complete();
                      })
                      .catch(function (error) {
                          console.log(error);
                          cfpLoadingBar.complete();
                      });
                }
            }

            function init() {
                loadCurrentFolderInfo();
                loadFolderList();
                loadFileList();
            }

            function loadFileList() {
                var folderCode = $scope.folderCode != undefined && $scope.folderCode != null ? $scope.folderCode : '0';
                var folderType = $scope.hashRoute.indexOf('/cloud/my') >= 0 ? 'my' : 'public';
                cfpLoadingBar.start();
                infobasisService.getAjaxInstance.get('/cloudfile/filelist/' + folderCode + '?folderType=' + folderType)
                  .then(function (response) {
                      var data = response.data;
                      //console.log(data);
                      angular.forEach(data, function (itemfile, i) {
                          itemfile.clientName = itemfile.clientName ? itemfile.clientName : itemfile.originName;
                          var fileType = itemfile.fileTypeName.toLowerCase();
                          if (fileType == '.pdf') {
                              itemfile.filePath = '../../res/images/filetype/pdf.jpg';
                          } else if (fileType == '.docx' || fileType == '.doc') {
                              itemfile.filePath = '../../res/images/filetype/word.jpg';
                          } else if (fileType == '.xls' || fileType == '.xlsx') {
                              itemfile.filePath = '../../res/images/filetype/excel.jpg';
                          }
                      });
                      $scope.uploads = data;
                      $scope.$apply();
                      cfpLoadingBar.complete();
                  })
                  .catch(function (error) {
                      console.log(error);
                      cfpLoadingBar.complete();
                  });
            };

            function loadFolderList() {
                var folderCode = $scope.folderCode != undefined && $scope.folderCode != null ? $scope.folderCode : '0';
                var folderType = $scope.hashRoute.indexOf('/cloud/my') >= 0 ? 'my' : 'public';
                cfpLoadingBar.start();
                infobasisService.getAjaxInstance.get('/cloudfile/folderlist/' + folderCode + '?folderType=' + folderType)
                  .then(function (response) {
                      var data = response.data;
                      //console.log(data);
                      $scope.folders = data;
                      $scope.$apply();
                      cfpLoadingBar.complete();
                  })
                  .catch(function (error) {
                      console.log(error);
                      cfpLoadingBar.complete();
                  });
            }

            init();

            function getCookie(name) {
                var findCookies = new RegExp(name + "=([^;]*)");
                var submatches = findCookies.exec(document.cookie);
                if (submatches != null && submatches.length > 1)
                    return submatches[1];
                else
                    return null;
            }

            $scope.clickUpload = function () {
                angular.element("#fileInput").click();
            }

            var jwtToken = getCookie('JWTToken');
            var uploader = $scope.uploader = new FileUploader({
                url: pageSetting.apiUrl + 'upload/' + $scope.folderCode,
                headers: { 'Authorization': 'Bearer ' + jwtToken },
                removeAfterUpload: true
            });

            // FILTERS

            uploader.filters.push({
                name: 'fileFilter',
                fn: function (item /*{File|FileLikeObject}*/, options) {
                    var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                    //console.log(type);
                    return '|vnd.openxmlformats-officedocument.wordprocessingml.document|vnd.ms-excel|vnd.openxmlformats-officedocument.spreadsheetml.sheet|xls|msword|pdf|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
                }
            });

            // CALLBACKS

            uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
                console.info('onWhenAddingFileFailed', item, filter, options);
            };
            uploader.onAfterAddingFile = function (fileItem) {
                console.info('onAfterAddingFile', fileItem);
            };
            uploader.onAfterAddingAll = function (addedFileItems) {
                console.info('onAfterAddingAll', addedFileItems);
            };
            uploader.onBeforeUploadItem = function (item) {
                console.info('onBeforeUploadItem', item);
            };
            uploader.onProgressItem = function (fileItem, progress) {
                console.info('onProgressItem', fileItem, progress);
            };
            uploader.onProgressAll = function (progress) {
                console.info('onProgressAll', progress);
            };
            uploader.onSuccessItem = function (fileItem, response, status, headers) {
                console.info('onSuccessItem', fileItem, response, status, headers);
            };
            uploader.onErrorItem = function (fileItem, response, status, headers) {
                console.info('onErrorItem', fileItem, response, status, headers);
            };
            uploader.onCancelItem = function (fileItem, response, status, headers) {
                console.info('onCancelItem', fileItem, response, status, headers);
            };
            uploader.onCompleteItem = function (fileItem, response, status, headers) {
                console.info('onCompleteItem', fileItem, response, status, headers);
            };
            uploader.onCompleteAll = function () {
                console.info('onCompleteAll');
                loadFileList();
            };

            //console.info('uploader', uploader);

            var controller = $scope.controller = {
                isImage: function (item) {
                    var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                    return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
                }
            };

            $scope.curFileID = "";
            $scope.editFileName = function (upload) {
                $scope.curFileID = upload.id;
            }

            function saveFileName(fileID, upload) {
                cfpLoadingBar.start();
                var fileData = {
                    fileName: upload.clientName
                };
                infobasisService.getAjaxInstance.put('/cloudfile/file/' + fileID, fileData)
                  .then(function (response) {
                      var data = response.data;
                      console.log(data);
                      $scope.$apply();
                      cfpLoadingBar.complete();
                  })
                  .catch(function (error) {
                      console.log(error);
                      cfpLoadingBar.complete();
                  });
            }

            $scope.updateFileName = function (event, upload) {
                var event = $window.event || event;
                var keycode = event.keyCode;
                if (keycode == 13) {
                    saveFileName($scope.curFileID, upload);
                    $scope.curFileID = "";
                    //paygroupService.updatePayGroup(paygroup).then(function (response) {
                    //    var data = response.data;
                    //});
                }
            }

            $scope.inactiveEditFileName = function (event) {
                var event = $window.event || event;
                if (event.target.nodeName.toLowerCase() != "input") {
                    $scope.curFileID = "";
                }
            }

            $scope.openPhotoSwipe = function (upload, itemIndex) {
                if (upload.isValidImage) {
                    openPhotoSwipe(upload, itemIndex);
                }
            };


        }]);

        app.controller('cloudEntryController',
        ['$scope', '$route',
        function ($scope, $route) {
            $scope.$route = $route;
        }]);

        app.controller('cloudPublicFileController',
        ['$scope', '$route',
        function ($scope, $route) {
            $scope.$route = $route;
        }]);

        app.directive('ngThumb', ['$window', function($window) {
            var helper = {
                support: !!($window.FileReader && $window.CanvasRenderingContext2D),
                isFile: function (item) {
                    return angular.isObject(item) && item instanceof $window.File;
                },
                isImage: function(file) {
                    var type =  '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
                    return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
                }
            };

            return {
                restrict: 'A',
                template: '<canvas/>',
                link: function(scope, element, attributes) {
                    if (!helper.support) return;

                    var params = scope.$eval(attributes.ngThumb);

                    if (!helper.isFile(params.file)) return;
                    if (!helper.isImage(params.file)) return;

                    var canvas = element.find('canvas');
                    var reader = new FileReader();

                    reader.onload = onLoadFile;
                    reader.readAsDataURL(params.file);

                    function onLoadFile(event) {
                        var img = new Image();
                        img.onload = onLoadImage;
                        img.src = event.target.result;
                    }

                    function onLoadImage() {
                        var width = params.width || this.width / this.height * params.height;
                        var height = params.height || this.height / this.width * params.width;
                        canvas.attr({ width: width, height: height });
                        canvas[0].getContext('2d').drawImage(this, 0, 0, width, height);
                    }
                }
            };
        }]);
        var openPhotoSwipe = function(upload, itemIndex) {
            var pswpElement = document.querySelectorAll('.pswp')[0];
            //console.log($('.upload-list li a img').length);
            var items = [];
            $('.upload-list li a img').each(function (index, element) {
                var isImage = $(this).attr('data-org-isimage');
                if (isImage == 'true') {
                    items.push({
                        src: $(this).attr('data-org-src'),
                        w: 964,
                        h: 683
                    });
                }
            });
    
            //console.log(items);
            // define options (if needed)
            var options = {
                // history & focus options are disabled on CodePen  
                index: itemIndex,
                escKey: true,
                history: false,
                focus: false,

                showAnimationDuration: 0,
                hideAnimationDuration: 0
        
            };
    
            var gallery = new PhotoSwipe( pswpElement, PhotoSwipeUI_Default, items, options);
            gallery.init();
        };

        //openPhotoSwipe();

    </script>

</asp:Content>