function initializeSaveJobHandler(jobId) {
    const saveButton = document.querySelector('#saveJobBtn');
    const saveIcon = document.querySelector('#saveIcon');
    const saveText = document.querySelector('#saveText');

    if (!saveButton || !saveIcon || !saveText) {
        console.error('Save button elements not found in the DOM.');
        return;
    }

    // Check if job is already saved
    fetch(`/JobSeeker/SavedJob/IsJobSaved?jobId=${jobId}`, { method: 'GET' })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to check save status');
            }
            return response.json();
        })
        .then(data => {
            if (data.saved) {
                saveIcon.classList.remove('bi-heart');
                saveIcon.classList.add('bi-heart-fill');
                saveText.textContent = 'Saved';
            }
        })
        .catch(error => {
            console.error('Error checking save status:', error);
        });

    // Handle save button click
    saveButton.addEventListener('click', function (e) {
        e.preventDefault();
        saveButton.disabled = true;
        saveText.textContent = 'Saving...';

        fetch(`/JobSeeker/SavedJob/SaveJob?jobId=${jobId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-Requested-With': 'XMLHttpRequest'
            },
        })
            .then(response => {
                if (!response.ok) {
                    if (response.status === 401) {
                        alert('Please log in to save jobs.');
                        return;
                    }
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                if (data.success) {
                    if (data.saved) {
                        saveIcon.classList.remove('bi-heart');
                        saveIcon.classList.add('bi-heart-fill');
                        saveText.textContent = 'Saved';
                    } else {
                        saveIcon.classList.remove('bi-heart-fill');
                        saveIcon.classList.add('bi-heart');
                        saveText.textContent = 'Save';
                    }
                } else {
                    alert(data.message || 'Failed to save the job.');
                }
            })
            .catch(error => {
                console.error('Error saving job:', error);
                alert('An error occurred while saving the job.');
            })
            .finally(() => {
                saveButton.disabled = false;
            });
    });
}