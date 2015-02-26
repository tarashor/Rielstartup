var Details = (function () {
    return function (photoListElement, coords, callback) {
        var self = this;
        var _myMap;

        var _initMap = function (inited) {
            //coords = [49.838891, 24.017365];
            _myMap = new Map("YMapsID", coords, 15, function () {
                _myMap.addMarkToMap(coords);
                inited();
            });
            
        };

        var _initPhotoList = function () {
            photoListElement.carouselDraggable({
                selectedItemClass: '',
                isDraggable: false,
                selectionChanged: function (e, ui) {
                    //_setMainPhoto($(ui.element).find('.photo').attr('src'), $(ui.element).data('id'));
                },
                click: function (e, ui) {
                    //alert('click');
                },
                moved: function (e, ui) {
                    //alert('move');
                },
            });
        };

        self.init = function () {
            

            _initMap(callback);
            _initPhotoList();

        };

        self.init();
    };
})();