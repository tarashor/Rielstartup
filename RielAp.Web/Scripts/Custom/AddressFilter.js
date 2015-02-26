/// <reference path="Map.js" />
var AddressFilter = (function () {
    return function (getAdministrativeAreaUrl, getSubAdministrativeAreaUrl, getLocalitiesUrl, getDistrictsUrl) {
        var self = this;

        var _addressTypeSelector = "#AddressType";
        var _administrativeAreaSelector = "#AdministrativeArea";
        var _subadministrativeAreaSelector = "#SubAdministrativeArea";
        var _localitySelector = "#LocalityNames";
        var _districtSelector = "#Districts";

        self.addressCityMode = function () {
            $('.address').show();
            $('.city-address').show();
            $('.villarge-address').hide();
            
        };

        self.addressVillargeMode = function () {
            $('.address').show();
            $('.city-address').hide();
            $('.villarge-address').show();
           

        };

        self.addressUnknownMode = function () {
            $('.address').hide();
            _deleteAdministrativeAreas();
            _deleteSubAdministrativeAreas();
            _deleteLocalities();
            _deleteDistricts();
            
        };


        var _deleteAdministrativeAreas = function () {
            $(_administrativeAreaSelector).attr('disabled', true);
            $(_administrativeAreaSelector).html('');
        };
        var _deleteSubAdministrativeAreas = function () {
            $(_subadministrativeAreaSelector).attr('disabled', true);
            $(_subadministrativeAreaSelector).html('');
        };
        var _deleteLocalities = function () {
            $(_localitySelector).attr('disabled', true);
            $(_localitySelector).html('');
        };
        var _deleteDistricts = function () {
            $(_districtSelector).attr('disabled', true);
            $(_districtSelector).html('');
        };

        var _deleteStreet = function () {
            $(_districtSelector).attr('disabled', true);
            $(_districtSelector).html('');
        };

        var _prepareDataBeforeSend = function (data) {
            return data.replace(/'/g, "&#39;");
        };

        var _getAdministrativeAreas = function (addressType, handler) {
            if (addressType != "Unknown") {
                $.post(getAdministrativeAreaUrl, { addresstype: addressType }, function (res) {
                    if (res.Done) {
                        res.Items.unshift("");
                        var items = '';
                        $.each(res.Items, function (i, value) {
                            items += "<option value='" + _prepareDataBeforeSend(value) + "'>" + value + "</option>";
                        });
                        $(_administrativeAreaSelector).removeAttr('disabled');
                        $(_administrativeAreaSelector).html(items);
                    }
                    else {
                        _deleteAdministrativeAreas();
                    }

                    _deleteSubAdministrativeAreas();
                    _deleteLocalities();
                    _deleteDistricts();
                    if (handler) {
                        handler();
                    }

                });
            }
        };

        var _getSubAdministrativeAreas = function (adminArea, handler) {
            $.post(getSubAdministrativeAreaUrl, { adminArea: _prepareDataBeforeSend(adminArea) }, function (res) {
                if (res.Done) {
                    res.Items.unshift("");
                    var items = '';
                    $.each(res.Items, function (i, value) {
                        items += "<option value='" + _prepareDataBeforeSend(value) + "'>" + value + "</option>";
                    });
                    $(_subadministrativeAreaSelector).removeAttr('disabled');
                    $(_subadministrativeAreaSelector).html(items);
                    
                }
                else {
                    _deleteSubAdministrativeAreas();
                }
                _deleteLocalities();
                _deleteDistricts();
                if (handler) {
                    handler();
                }
            });
        };

        var _getLocalities = function (addresstype, adminArea, subadminArea, handler) {
            $.post(getLocalitiesUrl, { addresstype: addresstype, adminArea: _prepareDataBeforeSend(adminArea), subadminArea: _prepareDataBeforeSend(subadminArea) }, function (res) {
                if (res.Done) {
                    res.Items.unshift("");
                    var items = '';
                    $.each(res.Items, function (i, value) {
                        items += "<option value='" + _prepareDataBeforeSend(value) + "'>" + value + "</option>";
                    });
                    $(_localitySelector).removeAttr('disabled');
                    $(_localitySelector).html(items);
                }
                else {
                    _deleteLocalities();
                }
                _deleteDistricts();
                if (handler) {
                    handler();
                }
                
            });
        };

        var _getDistricts = function (locality, handler) {
            $.post(getDistrictsUrl, { locality: _prepareDataBeforeSend(locality) }, function (res) {
                if (res.Done) {
                    res.Items.unshift("");
                    var items = '';
                    $.each(res.Items, function (i, value) {
                        items += "<option value='" + _prepareDataBeforeSend(value) + "'>" + value + "</option>";
                    });
                    $(_districtSelector).removeAttr('disabled');
                    $(_districtSelector).html(items);
                }
                else {
                    _deleteDistricts();
                }
                if (handler) {
                    handler();
                }
                
            });
        };

        var _applyAddressType = function () {
            if ($(_addressTypeSelector).val() == 'City') {
                self.addressCityMode();
            }
            else {
                if ($(_addressTypeSelector).val() == 'Villarge') {
                    self.addressVillargeMode();
                }
                else {
                    self.addressUnknownMode();
                }
            }
        };

        _setAdministrativeArea = function (adminArea) {
            if (adminArea) {
                $(_administrativeAreaSelector).val(adminArea);
            }
        };

        _setSubAdministrativeArea = function (subadminArea) {
            if (subadminArea) {
                $(_subadministrativeAreaSelector).val(subadminArea);
            }
        };

        _setLocality = function (locality) {
            if (locality) {
                $(_localitySelector).val(locality);
            }
        };

        _setDistrict = function (district) {
            if (district) {
                $(_districtSelector).val(district);
            }
        };

        _setAddressType = function (type) {
            if (type) {
                $(_addressTypeSelector).val(type);
            }
        };

        self.init = function (addresstype, adminArea, subadminArea, locality, district) {

            $(_addressTypeSelector).change(function () {
                if ($(_addressTypeSelector).val() == 'City') {
                    _getLocalities($(_addressTypeSelector).val(), adminArea, subadminArea);
                } else {
                    if ($(_addressTypeSelector).val() == 'Villarge') {
                        _getAdministrativeAreas($(_addressTypeSelector).val());
                    }
                }
                _applyAddressType();
            });

            $(_administrativeAreaSelector).change(function () {
                _getSubAdministrativeAreas($(_administrativeAreaSelector).val());
            });

            $(_subadministrativeAreaSelector).change(function () {
                _getLocalities($(_addressTypeSelector).val(), $(_administrativeAreaSelector).val(), $(_subadministrativeAreaSelector).val());

            });

            $(_localitySelector).change(function () {
                if ($(_addressTypeSelector).val() == 'City') {
                    _getDistricts($(_localitySelector).val());

                }
            });
            _applyAddressType();

            if (addresstype == 'Villarge') {
                _getAdministrativeAreas(addresstype, function () {
                    _setAdministrativeArea(adminArea);
                    _getSubAdministrativeAreas(adminArea, function () {
                        _setSubAdministrativeArea(subadminArea);
                        _getLocalities(addresstype, adminArea, subadminArea, function () {
                            _setLocality(locality);
                        });
                    });
                });
            }
            else {
                if (addresstype == 'City') {
                    _getLocalities(addresstype, adminArea, subadminArea, function () {
                        _setLocality(locality);
                        _getDistricts(locality, function () {
                            _setDistrict(district);
                        });
                    });

                }
            }

        };
    };
})();