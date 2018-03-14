<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Announcement.aspx.cs" Inherits="Infobasis.Web.Pages.OA.Announcement" 
    MasterPageFile="~/PageMaster/SPA.master"%>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style>

        a {
            text-decoration: none;
            color: #111;
        }

        table.announce-list {
            border-collapse: collapse;
            border: 1px solid #ddd;
            border-spacing: 0;
        }

        table.announce-list th {
            background-color: #f5f5f5;
            font-weight: normal;
            color: #428bca;
        }

        table.announce-list th,
        table.announce-list td
        {
            border: 1px solid #ddd;
            padding: 3px 8px;
        }

    </style>
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">
    <div ng-view></div>
</asp:Content>
<asp:Content ID="ScriptContent" runat="server" ContentPlaceHolderID="ScriptContent">
    <script src="/res/js/app/announceApp.js"></script>
    <script>
        app.controller('announceEntryController',
        ['$scope', '$route', '$routeParams', 'cfpLoadingBar', '$window', '$location',
        function ($scope, $route, $routeParams, cfpLoadingBar, $window, $location) {
            $scope.$route = $route;
            $scope.annID = $routeParams.id;
            $scope.hashRoute = $location.path();

            $scope.announcements = [{ group: '公司通知', title: '3月月会', publishDate: '2016-12-10', endDate: '2016-12-30', publisher: '孙嘉艺', readNum: 20 },
            { group: '公司通知', title: '2月月会', publishDate: '2016-12-10', endDate: '2016-12-30', publisher: '孙嘉艺', readNum: 20 }];

            function init() {
                loadAnnouncements();
            }

            function loadAnnouncements() {
                cfpLoadingBar.start();
                infobasisService.getAjaxInstance.get('/oa/announcements/')
                  .then(function (response) {
                      var data = response.data;
                      //console.log(data);
                      $scope.$apply();
                      cfpLoadingBar.complete();
                  })
                  .catch(function (error) {
                      console.log(error);
                      cfpLoadingBar.complete();
                  });
            }

            init();

        }]);



    </script>

</asp:Content>
