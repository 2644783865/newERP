var app = angular.module('infobasisApp');

app.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            controller: "announceEntryController",
            templateUrl: "/res/js/app/announce/views/announce-entry.html"
        })
        .otherwise({ redirectTo: "/" });
});
