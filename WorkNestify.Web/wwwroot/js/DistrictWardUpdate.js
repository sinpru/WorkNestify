$(document).ready(function () {
    $("#ProvinceId").change(function () {
        var provinceId = $(this).val();
        if (provinceId) {
            $.get("/api/location/districts/" + provinceId, function (data) {
                $("#DistrictId").empty().append('<option value="" disabled selected>Select District</option>');
                $("#WardCode").empty().append('<option value="" disabled selected>Select Ward</option>');

                $.each(data, function (index, district) {
                    $("#DistrictId").append($('<option>', {
                        value: district.id,
                        text: district.name
                    }));
                });
            }).fail(function (xhr, status, error) {
                console.error("Error fetching districts:", error);
            });
        }
    });

    $("#DistrictId").change(function () {
        var districtId = $(this).val();
        if (districtId) {
            $.get("/api/location/wards/" + districtId, function (data) {
                $("#WardCode").empty().append('<option value="" disabled selected>Select Ward</option>');

                $.each(data, function (index, ward) {
                    $("#WardCode").append($('<option>', {
                        value: ward.code || ward.Code,
                        text: ward.name
                    }));
                });
            }).fail(function (xhr, status, error) {
                console.error("Error fetching wards:", error);
            });
        }
    });
});