
angular.module('CarShareApp').controller('LogonController', function ($scope, $rootScope ,$http) {
    // create a message to display in our view
    $scope.Loading = false;
    $scope.LogonRequest = { Email: '', Password: '' };
    $scope.Error = null;
    $scope.Logon = function () {
        $scope.Loading = true;

        //we dont pass json to this token provider
        $http.post('/Token',
            "userName=" + encodeURIComponent($scope.LogonRequest.Email) +
            "&password=" + encodeURIComponent($scope.LogonRequest.Password) +
            "&grant_type=password",
            { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
            .then(function (res) {
                console.log(res);
                $scope.Error = null;
                //but we do receive json funnily enough
                $scope.Token = res.data;
                $rootScope.Token = res.data;
                $scope.Loading = false;
            }
            , function (error) {
                console.log(error);
                $scope.Error = error.data;
                $scope.Loading = false;
            });
    };
});