

app.controller('CarController', function ($scope, $http, $rootScope, $routeParams) {

    var carId = $routeParams.carId;
    $scope.LoggedOn = false;
    if ($rootScope.Token) {
        $scope.LoggedOn = true;

        $scope.Loading = true;
        //we dont pass json to this token provider
        $http.get('/api/cars/detail/' + carId,
            { headers: { 'Authorization': "Bearer " + $rootScope.Token.access_token } })
            .then(function (res) {
                    console.log(res);
                    $scope.Car = res.data;
                    $scope.Loading = false;
                }
                , function (error) {
                    console.log(error);
                    $scope.Loading = false;
                });
    }


});