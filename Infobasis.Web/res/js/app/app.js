
var app = angular.module('infobasisApp',
    [
        'ngRoute',
        'angular-loading-bar',
        'angularFileUpload'
    ]);

app.config(function(cfpLoadingBarProvider) {
    cfpLoadingBarProvider.includeBar = false;
    cfpLoadingBarProvider.spinnerTemplate = '<div><span class="fa fa-spinner">Loading...</div>';
});

app.run(function ($rootScope, $location, $window) {
    var history = [];

    $rootScope.$on('$routeChangeSuccess', function () {
        history.push($location.$$path);
    });

    $rootScope.back = function () {
        var prevUrl = history.length > 1 ? history.splice(-2)[0] : "/";
        $location.path(prevUrl);
    };
});

app.constant('infobasisSettings', {
    apiServiceBaseUri: pageSetting.apiUrl,
    isSSL: false
    //apiServiceBaseUri: 'http://192.168.5.229:8008/'
});

app.directive('backButton', function () {
    return {
        restrict: 'A',

        link: function (scope, element, attrs) {
            element.bind('click', goBack);

            function goBack() {
                history.back();
                scope.$apply();
            }
        }
    }
});

//app.config(function ($httpProvider) {
//    $httpProvider.interceptors.push('authInterceptorService');
//});
