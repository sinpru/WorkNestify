$(document).ready(function () {
    var config = window.reviewPaginationConfig || {};
    var totalReviews = config.totalReviews || 0;
    var pageSize = config.pageSize || 6;
    var currentPage = config.currentPage || 1;
    var isInitialLoad = true;

    console.log(window.reviewPaginationConfig);

    function updateReviewListings(reviews) {
        const $reviewListings = $('#review-listings');
        $reviewListings.empty();

        if (reviews && reviews.length > 0) {
            const reviewCards = reviews.map(review => {
                // Generate star rating HTML
                let starsHtml = '';
                for (let i = 5; i >= 1; i--) {
                    starsHtml += `<span class="star ${i <= review.rating ? 'text-warning' : 'text-muted'}"><i class="bi bi-star-fill"></i></span>`;
                }

                return `
                    <div class="col w-50">
                        <div class="card mx-auto shadow-sm">
                            <div class="card-body">
                                <div class="d-flex align-items-center mb-3">
                                    <!-- Company Logo -->
                                    ${review.logo ? `
                                        <img src="${review.logo}" class="me-3" alt="${review.company || 'Company'} Logo" style="height: 80px" />
                                    ` : `
                                        <div class="me-3 bg-light d-flex align-items-center justify-content-center" style="width: 60px; height: 60px;">
                                            <span class="text-muted">No Logo</span>
                                        </div>
                                    `}
                                    <!-- Company Name -->
                                    <h5 class="card-title text-primary mb-0">${review.company || 'Unknown Company'}</h5>
                                </div>

                                <!-- Review Content -->
                                <p class="card-text text-muted">${review.content || 'No content'}</p>

                                <!-- Rating in Stars -->
                                <div class="star-rating mb-3">
                                    ${starsHtml}
                                </div>

                                <!-- Action Buttons -->
                                <div class="d-flex justify-content-end">
                                    <a href="/JobSeeker/CompanyReview/Edit/${review.id}" class="btn btn-primary me-2">
                                        <i class="bi bi-pencil-square me-1"></i>Edit
                                    </a>
                                    <button type="button" class="btn btn-danger delete-review-btn" 
                                            data-bs-toggle="modal" 
                                            data-bs-target="#reviewDeleteModal"
                                            data-review-id="${review.id}"
                                            data-review-name="${review.company || 'this company'}">
                                        <i class="bi bi-trash me-1"></i>Delete
                                    </button>
                                </div>
                            </div>
                            <div class="card-footer text-muted">
                                <small>Posted on ${new Date(review.createdDate).toLocaleDateString('en-US', { month: 'short', day: '2-digit', year: 'numeric' })}</small>
                            </div>
                        </div>
                    </div>
                `;
            });
            $reviewListings.append(reviewCards.join(''));
        } else {
            $reviewListings.html('<div class="text-center"><p class="text-muted">You haven’t submitted any reviews yet.</p></div>');
        }
    }

    function fetchReviews(page) {
        var params = new URLSearchParams();
        params.append('page', page);
        params.append('pageSize', pageSize);

        $('#loading-spinner').removeClass('d-none');
        $('#review-listings').addClass('opacity-50');

        $.ajax({
            url: `/api/CompanyReview/get-reviews?${params.toString()}`,
            method: 'GET',
            dataType: 'json',
            success: function (response) {
                updateReviewListings(response.reviews);
                if (response.totalReviews !== totalReviews) {
                    totalReviews = response.totalReviews;
                    $('#pagination-container').pagination('destroy');
                    initializePagination(response.page, response.totalReviews);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching reviews:', error);
                alert('Error loading reviews. Please try again.');
            },
            complete: function () {
                $('#loading-spinner').addClass('d-none');
                $('#review-listings').removeClass('opacity-50');
            }
        });
    }

    function initializePagination(page, total) {
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
                fetchReviews(pagination.pageNumber);
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

    initializePagination(currentPage, totalReviews);
});