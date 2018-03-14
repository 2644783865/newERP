var app = angular.module('infobasisApp');

app.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            controller: "cloudEntryController",
            templateUrl: "/res/js/app/cloud/views/cloud-entry.html"
        })
        .when("/cloud/my", {
            controller: "cloudMyFileController",
            templateUrl: "/res/js/app/cloud/views/cloud-my.html"
        })
        .when("/cloud/my/:id", {
            controller: "cloudMyFileController",
            templateUrl: "/res/js/app/cloud/views/cloud-my.html"
        })
        .when("/cloud/public", {
            controller: "cloudMyFileController",
            templateUrl: "/res/js/app/cloud/views/cloud-my.html"
        })
        .when("/cloud/public/:id", {
            controller: "cloudMyFileController",
            templateUrl: "/res/js/app/cloud/views/cloud-my.html"
        })
        .otherwise({ redirectTo: "/" });
});
