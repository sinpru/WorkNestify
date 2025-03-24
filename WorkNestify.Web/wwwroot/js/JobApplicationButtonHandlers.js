function acceptJobApplication($button, table){
    const applicationId = $button.data('id');

    $button.prop('disabled', true);

    $.ajax({
        url: `/api/JobApplication/reviewed?id=${applicationId}`,
        method: 'POST',
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                table.ajax.reload(null, false);
            } else {
                alert('Failed to accept application: ' + (response.message || 'Unknown error'));
            }
        },
        error: function (xhr) {
            let errorMsg = 'Error occurred while accepting application';
            try {
                const errorData = JSON.parse(xhr.responseText);
                errorMsg += ': ' + (errorData.message || xhr.statusText);
            } catch {
                errorMsg += ': ' + xhr.statusText;
            }
            alert(errorMsg);
        },
        complete: function () {
            $button.prop('disabled', false);
        }
    });
}

function declineJobApplication($button, table){
    const applicationId = $button.data('id');

    $button.prop('disabled', true);

    $.ajax({
        url: `/api/JobApplication/declined?id=${applicationId}`,
        method: 'POST',
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                table.ajax.reload(null, false);
            } else {
                alert('Failed to decline application: ' + (response.message || 'Unknown error'));
            }
        },
        error: function (xhr) {
            let errorMsg = 'Error occurred while declining application';
            try {
                const errorData = JSON.parse(xhr.responseText);
                errorMsg += ': ' + (errorData.message || xhr.statusText);
            } catch {
                errorMsg += ': ' + xhr.statusText;
            }
            alert(errorMsg);
        },
        complete: function () {
            $button.prop('disabled', false);
        }
    });
}