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

        $jobListings.removeClass('justify-content-center');
        if (jobs && jobs.length > 0) {
            if (jobs.length < 3) {
                $jobListings.addClass('justify-content-center');
            }
            
            const fetchPromises = jobs.map(job =>
                $.ajax({
                    url: `/api/Job/card/${job.id}`,
                    method: 'GET',
                    dataType: 'json'
                }).then(data => {
                    const daysAgo = Math.floor((new Date() - new Date(data.createdDate)) / (1000 * 60 * 60 * 24));
                    const postedText = daysAgo === 0 ? "Posted today" : `Posted ${daysAgo} day${daysAgo === 1 ? "" : "s"} ago`;

                    return `
                    <div class="col">
                        <div class="card h-100 border-light shadow-sm">
                            <div class="card-body">
                                <div class="d-flex align-items-center justify-content-between mb-2">
                                    <div>
                                        <h5 class="card-title mb-0 text-primary fs-5">${data.title || 'Untitled'}</h5>
                                        <p class="card-subtitle mb-0 text-muted">${data.companyName || 'N/A'}</p>
                                    </div>
                                    <img src="${data.companyLogo || '/default-logo.png'}" alt="Company Logo" class="me-2" style="height: 40px;" />
                                </div>
                                <p class="card-text text-muted mb-1">${data.province || 'N/A'}</p>
                                <p class="card-text text-muted mb-1">${(data.salary || 0).toLocaleString('vi-VN')} ₫</p>
                                <div class="d-flex justify-content-between align-items-center">
                                    <span class="badge bg-success">${data.status || 'N/A'}</span>
                                    <a href="/JobSeeker/Job/Details/${data.id}" class="btn btn-outline-primary btn-sm">Details</a>
                                </div>
                            </div>
                        </div>
                    </div>
                `;
                }).catch(error => {
                    console.error(`Error fetching job card for job ${job.id}:`, error);
                    return `
                    <div class="col">
                        <div class="card h-100 border-light shadow-sm">
                            <div class="card-body">
                                <p class="text-danger">Error loading job</p>
                            </div>
                        </div>
                    </div>
                `;
                })
            );

            Promise.all(fetchPromises)
                .then(jobCards => {
                    $jobListings.html(jobCards.join(''));
                })
                .catch(error => {
                    console.error('Error updating job listings:', error);
                    $jobListings.html('<div class="text-center"><p class="text-danger">Error loading jobs</p></div>');
                });
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
                isInitialLoad = false;
                return;
            }

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
                    updateJobListings(response.jobs);
                    if (response.totalJobs !== totalJobs) {
                        totalJobs = response.totalJobs;
                        $('#pagination-container').pagination('updateItems', totalJobs);
                    }
                },
                error: function (xhr, status, error) {
                    console.error('AJAX error details:', {
                        status: status,              // e.g., "parsererror", "timeout"
                        error: error,               // e.g., "SyntaxError: Unexpected token"
                        statusCode: xhr.status,     // e.g., 200, 400, 500
                        responseText: xhr.responseText // Raw response body
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