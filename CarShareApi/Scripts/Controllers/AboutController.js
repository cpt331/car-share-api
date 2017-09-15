

app.controller('AboutController', function ($scope, $rootScope) {

    $scope.Token = $rootScope.Token;
    $scope.LoggedOn = false;
    if ($rootScope.Token) {
        $scope.LoggedOn = true;

    }
});