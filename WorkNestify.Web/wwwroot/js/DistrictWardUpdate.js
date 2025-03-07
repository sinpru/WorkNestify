$(document).ready(function () {
    $("#ProvinceId").change(function () {
        var provinceId = $(this).val();
        if (provinceId) {
            $.get("/api/location/districts/" + provinceId, function (data) {
                $("#DistrictId").empty().append('<option value="" disabled selected>Select District</option>');
                $("#WardId").empty().append('<option value="" disabled selected>Select Ward</option>');

                $.each(data, function (index, district) {
                    $("#DistrictId").append('<option value="' + district.id + '">' + district.name + '</option>');
                });
            });
        }
    });

    $("#DistrictId").change(function () {
        var districtId = $(this).val();
        if (districtId) {
            $.get("/api/location/wards/" + districtId, function (data) {
                $("#WardId").empty().append('<option value="" disabled selected>Select Ward</option>');

                $.each(data, function (index, ward) {
                    $("#WardId").append('<option value="' + ward.id + '">' + ward.name + '</option>');
                });
            });
        }
    });
});