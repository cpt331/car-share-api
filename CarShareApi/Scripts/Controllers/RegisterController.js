
angular.module('CarShareApp').controller('RegisterController', function ($scope, $rootScope ,$http) {
    // create a message to display in our view
    $scope.Loading = false;
    $scope.RegisterRequest = { Email: '', Password: '', ConfirmPassword: '' };
    
});