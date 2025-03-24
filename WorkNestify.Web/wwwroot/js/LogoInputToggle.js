function logoInputToggle() {
    let urlInput = document.getElementById('logoUrl');
    let fileInput = document.getElementById('logoFile');
    let toggleButton = document.getElementById('toggleMode');

    if (urlInput.classList.contains('d-none')) {
        // Switch to URL mode
        fileInput.classList.add('d-none');
        fileInput.value = ''; // Clear file input
        urlInput.classList.remove('d-none');
        toggleButton.innerHTML = '<i class="bi bi-file-earmark-image"></i>'; // Show file icon
    } else {
        // Switch to File mode
        urlInput.classList.add('d-none');
        urlInput.value = ''; // Clear URL input
        fileInput.classList.remove('d-none');
        toggleButton.innerHTML = '<i class="bi bi-link-45deg"></i>'; // Show URL icon
    }
}