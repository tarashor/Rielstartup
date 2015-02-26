var Map = (function () {
    return function (mapElementId, center, zoom, ready) {
        var self = this;
        var _myMap;

        self.initMap = function () {
            if (!self.isCoordinatesValid(center)) {
                center = [49.838891, 24.017365];
            }
            _myMap = new ymaps.Map(mapElementId, {
                center: center,
                zoom: zoom
            });
            _myMap.controls
                .add('zoomControl', { left: 5, top: 5 })
                .add('mapTools', { left: 35, top: 5 });
            ready();

        };

        self.setCenter = function (center) {
            if (self.isCoordinatesValid(center)) {
                _myMap.setCenter(center);
            }
        };

        self.setCrossHairCursor = function () {
            //_myMap.addCursor(ymaps.Cursor.CROSSHAIR);
            _myMap.cursors.push('crosshair');
        };

        self.addClickHandler = function (handler) {
            _myMap.events.add('click', handler);
        };

        self.findStreet = function (streetName, callback) {
            ymaps.geocode(streetName,{ results: 10, boundedBy: _myMap.getBounds(), kind: 'street', strictBounds: false })
               .then(function (res) {
                   callback(res);
               }, function (err) {
                   alert(err.message);
               });
        };

        self.findLocality = function (streetName, callback) {
            ymaps.geocode(streetName, { results: 10, boundedBy: _myMap.getBounds(), kind: 'locality', strictBounds: false })
               .then(function (res) {
                   callback(res);
               }, function (err) {
                   alert(err.message);
               });
        };

        self.getAddress = function (coords, callback) {
            ymaps.geocode(coords/*, { kind: 'street', strictBounds: false }*/)
               .then(function (res) {
                   callback(res);
               }, function (err) {
                   alert(err.message);
               });
        };

        self.getDistrict = function (coords, callback) {
            ymaps.geocode(coords, { kind: 'district'})
               .then(function (res) {
                   callback(res);
               }, function (err) {
                   alert(err.message);
               });
        };

        self.markStreetOnMap = function (streetName) {
            self.findStreet(streetName, function (res) {
                self.addObjectToMap(res.geoObjects.get(0));
            });
        };

        self.addObjectToMap = function (geoObj) {
            self.addMarkToMap(geoObj.geometry.getCoordinates(), geoObj.properties.get('balloonContentBody'));
        };

        self.addMarkToMap = function (coords, ballon, content, openEmptyBallon) {
            if (self.isCoordinatesValid(coords)) {
                if (_myMap) {
                    myPlacemark = new ymaps.Placemark(coords, {
                        balloonContent: ballon,
                        iconContent: content
                    }, {
                        preset: 'twirl#blueIcon',
                        openEmptyBalloon: !!openEmptyBallon
                    });
                    _myMap.geoObjects.add(myPlacemark);
                    return myPlacemark;
                }
            }
        };

        self.clearMap= function () {
            if (_myMap) {
                _myMap.geoObjects.each(function (geoObject) {
                    _myMap.geoObjects.remove(geoObject);
                });
            }
        };

        self.isCoordinatesValid = function (coords) {
            if (coords instanceof Array) {
                if (coords.length == 2) {
                    if (coords[0] && coords[1]) {
                        return true;
                    }
                }
            }
        };

        self.startDrawing = function (points) {
            var myPolygon = new ymaps.Polygon([], {}, {
                editorDrawingCursor: "crosshair",
                strokeColor: '#0000FF',
                strokeWidth: 1,
                draggable: true
            });
            _myMap.geoObjects.add(myPolygon);

            var stateMonitor = new ymaps.Monitor(myPolygon.editor.state);
            stateMonitor.add("drawing", function (newValue) {
                myPolygon.options.set("strokeColor", newValue ? '#FF0000' : '#0000FF');
            });
            var objects = ymaps.geoQuery(points);
            /*myPolygon.events.add('geometrychange', function () {
                // Объекты, попадающие в круг, будут становиться красными.
                var objectsInsideCircle = objects.searchInside(myPolygon);
                objectsInsideCircle.setOptions('preset', 'twirl#redIcon');
                // Оставшиеся объекты - синими.
                objects.remove(objectsInsideCircle).setOptions('preset', 'twirl#blueIcon');
            });*/
            myPolygon.editor.startDrawing();
        };
        ymaps.ready(self.initMap);
    }
})();