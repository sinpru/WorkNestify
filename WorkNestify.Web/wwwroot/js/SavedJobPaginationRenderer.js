$(document).ready(function () {
    var config = window.savedJobsListConfig || {};
    var totalSavedJobs = config.totalSavedJobs || 0;
    var pageSize = config.pageSize || 5;
    var currentPage = config.currentPage || 1;
    var isInitialLoad = true;

    function updateSavedJobListings(savedJobs) {
        const $savedJobListings = $('#saved-job-listings');
        $savedJobListings.empty();

        if (savedJobs && savedJobs.length > 0) {
            const jobCards = savedJobs.map(job => {
                const daysAgo = Math.floor((new Date() - new Date(job.createdDate)) / (1000 * 60 * 60 * 24));
                const postedText = daysAgo === 0 ? "Posted today" : `Posted ${daysAgo} day${daysAgo === 1 ? "" : "s"} ago`;
                const isSaved = job.isSaved || true;
                const saveIconClass = isSaved ? 'bi-heart-fill' : 'bi-heart';
                const saveText = isSaved ? 'Saved' : 'Save';
                const location = job.streetAddress ? `${job.streetAddress}, ${job.ward || ''}, ${job.district || ''}, ${job.province || ''}` : 'N/A';

                return `
                <div class="col">
                    <div class="card border-light shadow-sm">
                        <div class="card-body">
                            <div class="d-flex align-items-start">
                                <div class="me-4 align-self-center">
                                    <img src="${job.companyLogo || '/default-logo.png'}" alt="Company Logo" style="width: 140px;" />
                                </div>
                                <div class="flex-grow-1 border-start ps-4">
                                    <div class="d-flex justify-content-between align-items-start mb-2">
                                        <div>
                                            <h5 class="card-title mb-1 text-primary">${job.title || 'Untitled'}</h5>
                                            <p class="card-subtitle text-muted mb-0">${job.companyName || 'N/A'}</p>
                                        </div>
                                        <p class="card-text mb-0">
                                            <strong>Salary:</strong> ${(job.salary || 0).toLocaleString('vi-VN')} ₫
                                        </p>
                                    </div>
                                    <p class="card-text text-muted mb-2">
                                        <strong>Location:</strong> ${location}
                                    </p>
                                    <p class="card-text text-muted mb-2">
                                        <strong>Experience Level:</strong> ${job.level || 'N/A'}
                                    </p>
                                    <p class="card-text text-muted mb-2">
                                        <strong>Type:</strong> ${job.type || 'N/A'}
                                    </p>
                                    <div class="d-flex justify-content-between align-items-center mt-4">
                                        <span class="badge bg-success">${job.status || 'N/A'}</span>
                                        <div class="d-flex align-items-center gap-3">
                                            <span class="text-muted">${postedText}</span>
                                            <button class="btn btn-outline-secondary save-job-btn" data-job-id="${job.id}">
                                                <i class="bi ${saveIconClass} save-job-icon"></i> <span class="save-job-text">${saveText}</span>
                                            </button>
                                            <a href="/JobSeeker/Job/Details/${job.id}" class="btn btn-outline-primary">Details</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                `;
            });
            $savedJobListings.append(jobCards.join(''));

            if (typeof initializeSaveJobHandler === 'function') {
                initializeSaveJobHandler();
            }
        } else {
            $savedJobListings.html('<div class="text-center"><p class="text-muted">You haven’t saved any jobs yet.</p></div>');
        }
    }

    function fetchSavedJobs(page) {
        var params = new URLSearchParams();
        params.append('page', page);
        params.append('pageSize', pageSize);

        $('#loading-spinner').removeClass('d-none');
        $('#saved-job-listings').addClass('opacity-50');

        $.ajax({
            url: `/api/SavedJob/list?${params.toString()}`,
            method: 'GET',
            dataType: 'json',
            success: function (response) {
                updateSavedJobListings(response.savedJobs);
                if (response.totalSavedJobs !== totalSavedJobs) {
                    totalSavedJobs = response.totalSavedJobs;
                    $('#pagination-container').pagination('destroy');
                    initializePagination(page, totalSavedJobs);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching saved jobs:', error);
                alert('Error loading saved jobs. Please try again.');
            },
            complete: function () {
                $('#loading-spinner').addClass('d-none');
                $('#saved-job-listings').removeClass('opacity-50');
            }
        });
    }

    function initializePagination(page, total) {
        $('#pagination-container').pagination({
            dataSource: Array(total).fill({}),
            pageSize: pageSize,
            pageNumber: page,
            totalNumber: total,
            alias: { pageNumber: 'page' },
            prevText: '«',
            nextText: '»',
            showPrevious: true,
            showNext: true,
            pageRange: 2,
            callback: function (data, pagination) {
                if (isInitialLoad) {
                    console.log('Initial load, skipping fetch');
                    isInitialLoad = false;
                    return;
                }
                currentPage = pagination.pageNumber; // Update currentPage
                fetchSavedJobs(pagination.pageNumber);
            },
            afterRender: function () {
                $('#pagination-container .paginationjs-pages ul').addClass('pagination');
                $('#pagination-container .paginationjs-pages li').addClass('page-item');
                $('#pagination-container .paginationjs-pages li a').addClass('page-link');
                $('#pagination-container .paginationjs-pages li.active').addClass('active');
                $('#pagination-container .paginationjs-pages li.disabled').addClass('disabled');
            }
        });
    }

    // Initial pagination setup
    initializePagination(currentPage, totalSavedJobs);

    // Expose fetchSavedJobs globally
    window.savedJobsList = window.savedJobsList || {};
    window.savedJobsList.fetchSavedJobs = function(page) {
        fetchSavedJobs(page || currentPage);
    };
});