$(document).ready(function () {
    var config = window.jobFilterConfig || {};
    var totalJobs = config.totalJobs || 0;
    var pageSize = config.pageSize || 6;
    var currentPage = config.currentPage || 1;
    var search = config.search || '';
    var category = config.category || '';
    var location = config.location || '';
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
            console.log('Rendering', jobs.length, 'job cards');
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
            $jobListings.html(jobCards.join(''));
        } else {
            $jobListings.html('<div class="text-center"><p class="text-muted">No top jobs available at the moment.</p></div>');
        }
    }

    $('#pagination-container').pagination({
        dataSource: Array(totalJobs).fill({}),
        pageSize: pageSize,
        pageNumber: currentPage,
        totalNumber: totalJobs,
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

            console.log('Fetching page', pagination.pageNumber);
            $('#loading-spinner').removeClass('d-none');
            $('#job-listings').addClass('opacity-50');

            var params = new URLSearchParams();
            if (search) params.append('search', search);
            if (category) params.append('category', category);
            if (location) params.append('location', location);
            params.append('page', pagination.pageNumber);
            params.append('pageSize', pagination.pageSize);

            $.ajax({
                url: `/api/Job/filter?${params.toString()}`,
                method: 'GET',
                dataType: 'json',
                beforeSend: function (xhr) {
                    console.log('Sending request to:', this.url);
                },
                success: function (response) {
                    console.log('Received filter response:', response);
                    updateJobListings(response.jobs);
                    if (response.totalJobs !== totalJobs) {
                        totalJobs = response.totalJobs;
                        $('#pagination-container').pagination('updateItems', totalJobs);
                    }
                },
                error: function (xhr, status, error) {
                    console.error('AJAX error details:', {
                        status: status, error: error, statusCode: xhr.status, responseText: xhr.responseText
                    });
                    alert('Error loading jobs. Please try again.');
                },
                complete: function (xhr, status) {
                    console.log('Request completed with status:', status);
                    $('#loading-spinner').addClass('d-none');
                    $('#job-listings').removeClass('opacity-50');
                }
            });
        },
        afterRender: function () {
            $('#pagination-container .paginationjs-pages ul').addClass('pagination');
            $('#pagination-container .paginationjs-pages li').addClass('page-item');
            $('#pagination-container .paginationjs-pages li a').addClass('page-link');
            $('#pagination-container .paginationjs-pages li.active').addClass('active');
            $('#pagination-container .paginationjs-pages li.disabled').addClass('disabled');
        }
    });
});