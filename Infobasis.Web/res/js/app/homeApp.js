var homeApp = angular.module('homeApp', []);

homeApp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        .when("/", {
            controller: "homeController",
            templateUrl: "/app/authApp/views/login.html"
        });
}]);
