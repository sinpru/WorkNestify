$(document).ready(function() {
    $('.withdraw-btn').on('click', function() {
        const applicationId = $(this).data('application-id');
        const $button = $(this);
        const $statusBadge = $button.closest('.card-body').find('.badge');

        if (confirm('Are you sure you want to withdraw this application?')) {
            $.ajax({
                url: `/api/JobApplication/withdrawn?id=${applicationId}`,
                type: 'POST',
                contentType: 'application/json',
                headers: {
                    'X-CSRF-TOKEN': $('input[name="__RequestVerificationToken"]').val()
                },
                success: function(response) {
                    if (response.success) {
                        $statusBadge.text('Withdrawn');
                        $statusBadge.removeClass().addClass('badge bg-dark');
                        $button.remove();
                        alert('Application withdrawn successfully');
                    } else {
                        alert(response.message || 'Failed to withdraw application');
                    }
                },
                error: function(xhr, status, error) {
                    console.error('Withdraw error:', xhr.responseText);
                    alert('An error occurred while withdrawing the application');
                }
            });
        }
    });
});

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
