

app.controller('CarsController', function ($scope, $http, $rootScope) {
    

    $scope.LoggedOn = false;
    if ($rootScope.Token) {
        $scope.LoggedOn = true;

        $scope.Loading = true;
        //we dont pass json to this token provider
        $http.get('/api/Cars',
            { headers: { 'Authorization': "Bearer " + $rootScope.Token.access_token } })
            .then(function (res) {
                    console.log(res);
                    $scope.Cars = res.data;
                    $scope.Loading = false;
                }
                , function (error) {
                    console.log(error);
                    $scope.Loading = false;
                });
    }


});