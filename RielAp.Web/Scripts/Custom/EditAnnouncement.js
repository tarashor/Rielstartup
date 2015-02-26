/// <reference path="Map.js" />
var EditAnnouncement = (function () {
    return function (announcementId, addPhotoUrl, deletePhotoUrl, setMainPhotoUrl, addPhotoElement, addPhotoElementHidden, photoListElement, mainPhotoElement) {
        var self = this;
        var _myMap;
        var placemark;

        var _initMap = function (inited) {
            _myMap = new Map("YMapsID", [49.838891, 24.017365], 12, function () {
                _myMap.addClickHandler(function (e) {
                    document.getElementById("inputAddress").checked = false;
                    self.addressInputMode(false);
                    _myMap.clearMap();
                    var coords = e.get('coordPosition');
                    _myMap.getAddress(coords, function (res) {
                        placemark = res.geoObjects.get(0);
                        var addressdetails = _getAddressDetails(placemark);

                        _fillInAdressInputs(coords, addressdetails);
                        _myMap.addObjectToMap(placemark);
                        //_myMap.setCenter(placemark.geometry.getCoordinates());
                    });

                });
                _myMap.setCrossHairCursor();
                inited();
            });
            
        };

        var _getAddressDetails = function (object) {
            var addressDetails;
            if (object) {
                addressDetails = object.properties.get('metaDataProperty').GeocoderMetaData.AddressDetails;
            }
            return addressDetails;
        };

        var _getAddressType = function (object) {
            return object.properties.get('metaDataProperty').GeocoderMetaData.kind == 'locality' ? "Villarge": "City";
        };

        var _fillInAdressInputs = function (coords, addressDetails) {
            _cleanInputs();
            $('#longitude').val(coords[0]);
            $('#latitude').val(coords[1]);

            $('#districtValue').hide();
            $('#districtLabel').hide();

            var addressType = "City";

            var address = addressDetails;
            if (address.Country && address.Country.CountryName == "Україна") {
                address = address.Country;
            }
            if (address.AdministrativeArea) {
                $('#AdministrativeAreaInput').show();
                $('#AdministrativeAreaLabel').show();
                $('#AdministrativeAreaInput').val(address.AdministrativeArea.AdministrativeAreaName);
                address = address.AdministrativeArea;
            }
            else {
                $('#AdministrativeAreaInput').hide();
                $('#AdministrativeAreaLabel').hide();
            }

            if (address.SubAdministrativeArea) {
                $('#SubAdministrativeAreaInput').val(address.SubAdministrativeArea.SubAdministrativeAreaName);
                address = address.SubAdministrativeArea;
                addressType = "Villarge";
            }

            if (address.Locality) {
                $('#LocalityNameInput').val(address.Locality.LocalityName);
                address = address.Locality;
            }


            if (address.DependentLocality) {
                $('#DistrictInput').val(address.DependentLocality.DependentLocalityName);
                address = address.DependentLocality;
            }
            else {
                if (addressType == "City") {
                    _myMap.getDistrict(coords, function (res) {
                        if (res.geoObjects.getLength() > 0) {
                            var addresswithdistrictdetails = _getAddressDetails(res.geoObjects.get(0));
                            var address = addresswithdistrictdetails;
                            if (address.Country) {
                                address = address.Country;
                            }
                            if (address.AdministrativeArea) {
                                address = address.AdministrativeArea;
                            }

                            if (address.SubAdministrativeArea) {
                                address = address.SubAdministrativeArea;
                            }

                            if (address.Locality) {
                                address = address.Locality;
                            }

                            if (address.DependentLocality) {
                                $('#DistrictInput').val(address.DependentLocality.DependentLocalityName);
                            }
                        }
                    });
                }
            }


            if (address.Thoroughfare) {
                $('#StreetInput').val(address.Thoroughfare.ThoroughfareName);
            }

            $('#addressTypeInput').val(addressType);
            $('#addressTypeInput').change();

        };

        var _cleanInputs = function () {
            $('#address input[type="text"]').each(function () {
                $(this).val('');
                
            });
            $('#address input[type="hidden"]').each(function () {
                $(this).val('');

            });
        };


        var _getPhotoIdFromContainer = function (photoContainer) {
            return $(photoContainer).attr('data-id');
        };

        var _createPhoto = function (photoId, photoUrl) {
            var photoContainer = $('<div>');
            photoContainer.addClass('photoContainer');
            photoContainer.attr('data-id', photoId);

            var photoImage = $('<img/>');
            photoImage.attr('src', photoUrl);
            photoImage.addClass('photo');
            photoContainer.append(photoImage);

            var photoDelete = $('<div>');
            photoDelete.addClass('photoDelete');
            photoDelete.click(_deleteHandler);

            var photoDeleteImage = $('<img/>');
            photoDeleteImage.attr('src', "/Content/images/removePhoto.png");
            photoDeleteImage.attr('style', "position:absolute;");
            photoDelete.append(photoDeleteImage);
            photoContainer.append(photoDelete);

            return photoContainer;
        };

        var _deleteHandler = function (e) {
            var deleteBtn = $(e.target);
            _deletePhoto(deleteBtn.parents('.photoContainer').eq(0));
        };

        var _deletePhoto = function (photoCont) {
            var photoId = _getPhotoIdFromContainer(photoCont);
            $.post(deletePhotoUrl, { photoId: photoId }, function (response) {
                if (response.Done) {
                    photoListElement.carouselDraggable('removeItem', photoCont);
                }
                if (response.Message) {
                    $('#photosLeft').html(response.Message);
                }
            });
        };

        var _setMainPhoto = function (url, photoId) {
            $.post(setMainPhotoUrl, { announcementId: announcementId, photoId: photoId }, function (res) {
                if (res.Done) {
                    mainPhotoElement.attr('src', url);
                }
            });
        };

        var _initFileUploader = function () {

            var uploader = new qq.FileUploader({
                element: addPhotoElementHidden.get(0),
                action: addPhotoUrl,
                params: { announcementId: announcementId },
                sizeLimit: 20971520,
                allowedExtensions: addPhotoElementHidden.data('allowedextensions').split(','),
                multiple: false,
                onSubmit: function (id, fileName) {
                    //alert('startUploading');
                    $('#progress').show();
                },
                onComplete: function (id, fileName, responseJson) {
                    $('#progress').hide();
                    if (responseJson.Done) {
                        var photoItem = _createPhoto(responseJson.PhotoId, responseJson.PhotoUrl);
                        photoListElement.carouselDraggable('addItem', photoItem);
                        $(".qq-upload-success").remove();
                    }
                    $('#photosLeft').html(responseJson.Message);
                },
                messages: {
                    typeError: 'Type error',
                    sizeError: 'size error'
                }
            });

            addPhotoElement.click(function () {
                addPhotoElementHidden.find('input').click();
            });
        };

        var _initPhotoList = function () {
            photoListElement.carouselDraggable({
                selectedItemClass: 'selectedItem',
                isDraggable: true,
                selectionChanged: function (e, ui) {
                    _setMainPhoto($(ui.element).find('.photo').attr('src'), $(ui.element).data('id'));
                },
                click: function (e, ui) {
                    //alert('click');
                },
                moved: function (e, ui) {
                    //alert('move');
                },
                itemsContainer: $('#listPhotos')
            });
        };

        self.addressCityMode = function () {
            $('#address .city-address').show();
            $('#address .villarge-address').hide();
            
        };

        self.addressVillargeMode = function () {
            $('#address .city-address').hide();
            $('#address .villarge-address').show();

        };

        self.addressInputMode = function (keyboardInput) {

            $('#address input').each(function () {
                var input = $(this);
                if ((input.attr('id') != 'inputAddress') && (input.attr('id') != 'showButton')) {
                    this.disabled = !keyboardInput;
                }
            });

            $('#address select').each(function () {
                this.disabled = !keyboardInput;
            });
            if (keyboardInput) {
                $('#showButtonRow').show();
                
            }
            else {
                $('#showButtonRow').hide();
            }
        };

        var _showOnMap = function () {

            var handlerFunction = function (res) {
                placemark = res.geoObjects.get(0);
                if (placemark) {
                    var addressdetails = _getAddressDetails(placemark);
                    _fillInAdressInputs(placemark.geometry.getCoordinates(), addressdetails);
                    _myMap.addObjectToMap(placemark);
                    _myMap.setCenter(placemark.geometry.getCoordinates());
                }
                else {
                    alert('Такого об"єкту незнайдено.')
                }
            };
            if ($('#addressTypeInput').val() == 'City') {
                _myMap.findStreet(_cityAddressToString(), handlerFunction);
            }
            else {
                _myMap.findLocality(_localityAddressToString(), handlerFunction);
            }
        };


        var _cityAddressToString = function () {
            return $('#LocalityNameInput').val() + ', ' + $('#StreetInput').val();
        };
        var _localityAddressToString = function () {
            return $('#SubAdministrativeAreaInput').val() + ', ' + $('#LocalityNameInput').val();
        };

        

        self.init = function () {
            _initMap(function () {
                var longitude = $('#longitude').val();
                var latitude = $('#latitude').val();
                if (longitude && latitude) {
                    _myMap.getAddress([longitude, latitude], function (res) {
                        placemark = res.geoObjects.get(0);
                        _myMap.addObjectToMap(placemark);
                        _myMap.setCenter(placemark.geometry.getCoordinates());
                    });
                }
                
            });
            _initFileUploader();
            _initPhotoList();
            
            $('#addressTypeInput').change(function () {
                if ($(this).val() == 'City') {
                    self.addressCityMode();
                }
                else {
                    self.addressVillargeMode();
                }
            });
            $('#inputAddress').change(function () {
                var keyboardInput = $(this).is(':checked');
                self.addressInputMode(keyboardInput);
                if (keyboardInput) {
                    _cleanInputs();
                    _myMap.clearMap();
                }
            });
            $('#showButton').click(_showOnMap);

            $('.photoDelete').click(_deleteHandler);

            $("form").submit(function () {
                self.addressInputMode(true);
            });
        };

        self.init();
    };
})();