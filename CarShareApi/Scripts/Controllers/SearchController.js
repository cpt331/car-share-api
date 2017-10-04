

app.controller('SearchController', function ($scope, $http, $rootScope) {
    
    $scope.Criteria = {
         MaxResults: 10,
         Latitude: -33.89806198, //defaulting to newtown (sydney)
         Longitude: 151.17925644,
         Radius: 5000 //no further than 5km away
    };
    $scope.LoggedOn = false;
    if ($rootScope.Token) {
        $scope.LoggedOn = true;
        $scope.Loading = true;
        $http.get('/api/cars/categories',
                { headers: { 'Authorization': "Bearer " + $rootScope.Token.access_token } })
            .then(function (res) {
                    console.log(res);
                    $scope.Categories = res.data;
                    $scope.Loading = false;
                }
                , function (error) {
                    console.log(error);
                    $scope.Loading = false;
                });
    }

    $scope.Search = function () {

        $scope.Loading = true;
        //we dont pass json to this token provider
        $http.post('/api/cars/search', $scope.Criteria,
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
    };


});