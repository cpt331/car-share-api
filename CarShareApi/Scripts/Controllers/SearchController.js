

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

    var mapOptions = {
        zoom: 12,
        center: new google.maps.LatLng($scope.Criteria.Latitude, $scope.Criteria.Longitude),
        mapTypeId: google.maps.MapTypeId.TERRAIN
    }

    $scope.Map = new google.maps.Map(document.getElementById('map'), mapOptions);

    $scope.Search = function () {

        $scope.SetPositionMarker($scope.Criteria.Latitude, $scope.Criteria.Longitude);
        $scope.Map.setCenter(new google.maps.LatLng($scope.Criteria.Latitude, $scope.Criteria.Longitude));

        $scope.Loading = true;
        //we dont pass json to this token provider
        $http.post('/api/cars/search', $scope.Criteria,
                { headers: { 'Authorization': "Bearer " + $rootScope.Token.access_token } })
            .then(function (res) {
                    console.log(res);
                    $scope.Cars = res.data;
                    $scope.ClearMarkers();
                    for (var i = 0; i < $scope.Cars.length; i++) {
                        $scope.CreateMarker($scope.Cars[i]);
                    }
                    $scope.Loading = false;
                }
                , function (error) {
                    console.log(error);
                    $scope.Loading = false;
                });
    };

    $scope.ClearMarkers = function() {
        for (var i = 0; i < $scope.Markers.length; i++) {
            $scope.Markers[i].setMap(null);
        }
    }

    $scope.Markers = [];
    var infoWindow = new google.maps.InfoWindow();
    $scope.CreateMarker = function (car) {

        var marker = new google.maps.Marker({
            map: $scope.Map,
            position: new google.maps.LatLng(car.LatPos, car.LongPos),
            title: (car.Make + " " + car.Model)
        });
        marker.content = '<div class="infoWindowContent">' + car.Suburb + '</div>';

        google.maps.event.addListener(marker, 'click', function () {
            infoWindow.setContent('<h2>' + marker.title + '</h2>' + marker.content);
            infoWindow.open($scope.Map, marker);
        });

        $scope.Markers.push(marker);

    }

    var image = {
        url: 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png',
        // This marker is 20 pixels wide by 32 pixels high.
        size: new google.maps.Size(20, 32),
        // The origin for this image is (0, 0).
        origin: new google.maps.Point(0, 0),
        // The anchor for this image is the base of the flagpole at (0, 32).
        anchor: new google.maps.Point(0, 32)
    };
    $scope.PositionMarker = null;
    $scope.SetPositionMarker = function(lat, lng) {

        if ($scope.PositionMarker) {
            $scope.PositionMarker.setMap(null);
        }
        $scope.PositionMarker = new google.maps.Marker({
            map: $scope.Map,
            icon: image,
            position: new google.maps.LatLng(lat, lng),
            title: "You are here"
        });
        $scope.PositionMarker.content = '<div class="infoWindowContent">I think...</div>';

        google.maps.event.addListener($scope.PositionMarker, 'click', function () {
            infoWindow.setContent('<h2>' + $scope.PositionMarker.title + '</h2>' + $scope.PositionMarker.content);
            infoWindow.open($scope.Map, $scope.PositionMarker);
        });

    }

    $scope.SetPositionMarker($scope.Criteria.Latitude, $scope.Criteria.Longitude);

});