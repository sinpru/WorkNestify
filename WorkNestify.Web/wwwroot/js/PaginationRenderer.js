$(document).ready(function () {
    var config = window.jobFilterConfig || {};
    var totalJobs = config.totalJobs || 0;
    var pageSize = config.pageSize || 6;
    var currentPage = config.currentPage || 1;
    var search = config.search || '';
    var category = config.category || '';
    var location = (config.location && !isNaN(parseInt(config.location, 10))) ? parseInt(config.location, 10) : null;
    var isInitialLoad = true;

    function updateJobListings(jobs) {
        const $jobListings = $('#job-listings');
        $jobListings.empty();

        // Determine the layout based on the page (e.g., via a global variable or URL check)
        const isJobsListPage = window.location.pathname.includes('/JobSeeker/Job');
        const layout = isJobsListPage ? 'wide' : 'compact';

        if (!isJobsListPage && jobs && jobs.length > 0 && jobs.length < 3) {
            $jobListings.addClass('justify-content-center');
        } else {
            $jobListings.removeClass('justify-content-center');
        }

        if (jobs && jobs.length > 0) {
            const jobCards = jobs.map(job => {
                const daysAgo = Math.floor((new Date() - new Date(job.createdDate)) / (1000 * 60 * 60 * 24));
                const postedText = daysAgo === 0 ? "Posted today" : `Posted ${daysAgo} day${daysAgo === 1 ? "" : "s"} ago`;

                if (layout === 'compact') {
                    return `
                    <div class="col">
                        <div class="card h-100 border-light shadow-sm">
                            <div class="card-body">
                                <div class="d-flex align-items-center justify-content-between mb-2">
                                    <div>
                                        <h5 class="card-title mb-0 text-primary fs-5">${job.title || 'Untitled'}</h5>
                                        <p class="card-subtitle mb-0 text-muted">${job.companyName || 'N/A'}</p>
                                    </div>
                                    <img src="${job.companyLogo || '/default-logo.png'}" alt="Company Logo" class="me-2" style="height: 40px;" />
                                </div>
                                <p class="card-text text-muted mb-1">${job.province || 'N/A'}</p>
                                <p class="card-text text-muted mb-1">${(job.salary || 0).toLocaleString('vi-VN')} ₫</p>
                                <div class="d-flex justify-content-between align-items-center">
                                    <span class="badge bg-success">${job.status || 'N/A'}</span>
                                    <a href="/JobSeeker/Job/Details/${job.id}" class="btn btn-outline-primary btn-sm">Details</a>
                                </div>
                            </div>
                        </div>
                    </div>
                `;
                } else {
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
                                                <a href="/JobSeeker/Job/Details/${job.id}" class="btn btn-outline-primary">Details</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                `;
                }
            });
            $jobListings.append(jobCards.join(''));
        } else {
            $jobListings.html('<div class="text-center"><p class="text-muted">No top jobs available at the moment.</p></div>');
        }
    }

    function fetchJobs(page) {
        var params = new URLSearchParams();
        if (search) params.append('search', search);

        // Get selected categories (multiple checkboxes)
        var categories = $('#filter-form input[name="category"]:checked').map(function () {
            return this.value;
        }).get();

        if (categories.length > 0) {
            params.append('category', categories.join(','));
        }

        // Get selected job type
        var type = $('#filter-form input[name="type"]:checked').val();
        if (type && type !== 'All') params.append('type', type);

        // Get selected level
        var level = $('#filter-form input[name="level"]:checked').val();
        if (level && level !== 'All') params.append('level', level);

        // Get selected salary range
        var salary = $('#filter-form input[name="salary"]:checked').val();
        if (salary && salary !== 'All') params.append('salary', salary);

        if (location !== null && !isNaN(location)) params.append('location', location);
        params.append('page', page);
        params.append('pageSize', pageSize);

        $('#loading-spinner').removeClass('d-none');
        $('#job-listings').addClass('opacity-50');

        $.ajax({
            url: `/api/Job/filter-jobs?${params.toString()}`,
            method: 'GET',
            dataType: 'json',
            success: function (response) {
                updateJobListings(response.jobs);
                if (response.totalJobs !== totalJobs) {
                    totalJobs = response.totalJobs;
                    $('#pagination-container').pagination('destroy');
                    initializePagination(page, totalJobs);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching jobs:', error);
                alert('Error loading jobs. Please try again.');
            },
            complete: function () {
                $('#loading-spinner').addClass('d-none');
                $('#job-listings').removeClass('opacity-50');
            }
        });
    }

    function initializePagination(page, total){
        $('#pagination-container').pagination({
            dataSource: Array(total).fill({}),
            pageSize: pageSize,
            pageNumber: page,
            totalNumber: total,
            alias: {
                pageNumber: 'page'
            },
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
                fetchJobs(pagination.pageNumber);
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
    initializePagination(currentPage, totalJobs);

    let debounceTimer;
    $('#filter-form input').on('change', function () {
        currentPage = 1;
        $('#pagination-container').pagination('go', 1);
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(() => fetchJobs(1), 300);
    });
});