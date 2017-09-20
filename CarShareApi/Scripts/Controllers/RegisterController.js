
angular.module('CarShareApp').controller('RegisterController', function ($scope, $rootScope ,$http) {
    // create a message to display in our view
    $scope.Loading = false;
    $scope.RegisterRequest = { Email: '', Password: '', ConfirmPassword: '' };
    $scope.Error = null;
    $scope.Register = function () {
        $scope.Loading = true;

        $http.post('/api/account/register',$scope.RegisterRequest)
            .then(function (res) {
                    console.log(res);
                    $scope.Error = null;
                   
                    $scope.Loading = false;
                }
                , function (error) {
                    console.log(error);
                    $scope.Error = error.data;
                    $scope.Loading = false;
                });
    };
});