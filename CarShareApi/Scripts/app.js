// create the module and name it scotchApp
var app = angular.module('CarShareApp', ['ngRoute']);

// configure our routes
app.config(function ($routeProvider) {
    $routeProvider


        // route for the home page
        .when('/logon',
            {
                templateUrl: '../Pages/Logon.html',
                controller: 'LogonController'
            })

        // route for the about page
        .when('/about',
            {
                templateUrl: '../Pages/About.html',
                controller: 'AboutController'
            })

        // route for the cars page
        .when('/cars',
            {
                templateUrl: '../Pages/Cars.html',
                controller: 'CarsController'
        })

        // route for the car detail page
        .when('/cars/:carId',
            {
                templateUrl: '../Pages/Car.html',
                controller: 'CarController'
            })


        // route for the home page
        .otherwise(
            {
                templateUrl: '../Pages/Home.html',
                controller: 'HomeController'
            });
});

// create the controller and inject Angular's $scope
app.controller('HomeController', function ($scope) {
    // create a message to display in our view
  
});
