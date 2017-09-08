// create the module and name it scotchApp
var app = angular.module('CarShareApp', ['ngRoute']);

// configure our routes
app.config(function ($routeProvider) {
    $routeProvider

        // route for the home page
        .when('/', {
            templateUrl: '../Pages/Home.html',
            controller: 'mainController'
        })

        // route for the home page
        .when('/logon', {
            templateUrl: '../Pages/Logon.html',
            controller: 'LogonController'
        })

        // route for the about page
        .when('/about', {
            templateUrl: '../Pages/About.html',
            controller: 'aboutController'
        })

        // route for the contact page
        .when('/contact', {
            templateUrl: '../Pages/Contact.html',
            controller: 'contactController'
        });
});

// create the controller and inject Angular's $scope
app.controller('mainController', function ($scope) {
    // create a message to display in our view
    $scope.message = 'Everyone come and see how good I look!';
});

app.controller('aboutController', function ($scope) {
    $scope.message = 'Look! I am an about page.';
});

app.controller('contactController', function ($scope) {
    $scope.message = 'Contact us! JK. This is just a demo.';
});