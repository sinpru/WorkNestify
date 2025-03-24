function handleAcceptJob($button, table) {
    const jobId = $button.data('id');

    $button.prop('disabled', true); // Disable button during request

    $.ajax({
        url: `/api/Job/accept-job?id=${jobId}`,
        method: 'POST',
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                table.ajax.reload(null, false); // Reload the table without resetting pagination
            } else {
                alert('Failed to accept job: ' + (response.message || 'Unknown error'));
            }
        },
        error: function (xhr) {
            let errorMsg = 'Error occurred while accepting job';
            try {
                const errorData = JSON.parse(xhr.responseText);
                errorMsg += ': ' + (errorData.message || xhr.statusText);
            } catch {
                errorMsg += ': ' + xhr.statusText;
            }
            alert(errorMsg);
        },
        complete: function () {
            $button.prop('disabled', false); // Re-enable button
        }
    });
}

function handleDeclineJob($button, table) {
    const jobId = $button.data('id');

    $button.prop('disabled', true); // Disable button during request

    $.ajax({
        url: `/api/Job/decline-job?id=${jobId}`,
        method: 'POST',
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                table.ajax.reload(null, false); // Reload the table without resetting pagination
            } else {
                alert('Failed to decline job: ' + (response.message || 'Unknown error'));
            }
        },
        error: function (xhr) {
            let errorMsg = 'Error occurred while declining job';
            try {
                const errorData = JSON.parse(xhr.responseText);
                errorMsg += ': ' + (errorData.message || xhr.statusText);
            } catch {
                errorMsg += ': ' + xhr.statusText;
            }
            alert(errorMsg);
        },
        complete: function () {
            $button.prop('disabled', false); // Re-enable button
        }
    });
}

function toggleStatusJob($button, table) {
    const jobId = $button.data('id');

    $button.prop('disabled', true); // Disable button during request

    $.ajax({
        url: `/api/Job/toggle-job?id=${jobId}`,
        method: 'POST',
        contentType: 'application/json',
        success: function (response) {
            if (response.success) {
                table.ajax.reload(null, false);
            } else {
                alert('Failed to toggle status: ' + (response.message || 'Unknown error'));
            }
        },
        error: function (xhr) {
            let errorMsg = 'Error occurred while toggling status';
            try {
                const errorData = JSON.parse(xhr.responseText);
                errorMsg += ': ' + (errorData.message || xhr.statusText);
            } catch {
                errorMsg += ': ' + xhr.statusText;
            }
            alert(errorMsg);
        },
        complete: function () {
            $button.prop('disabled', false); // Re-enable button
        }
    });
}