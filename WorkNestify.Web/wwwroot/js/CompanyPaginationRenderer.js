$(document).ready(function () {
    var config = window.companyFilterConfig || {};
    var totalCompanies = config.totalCompanies || 0;
    var pageSize = config.pageSize || 12;
    var currentPage = config.currentPage || 1;
    var search = config.search || '';
    var isInitialLoad = true;

    console.log(window.companyFilterConfig);

    function updateCompanyListings(companies) {
        const $companyListings = $('#company-listings');
        $companyListings.empty();
        
        if (companies && companies.length > 0 && companies.length < 3) {
            $companyListings.addClass('justify-content-center');
        } else {
            $companyListings.removeClass('justify-content-center');
        }

        if (companies && companies.length > 0) {
            const companyCards = companies.map(company => {
                // Construct the location string
                const location = company.streetAddress
                    ? `${company.streetAddress}, ${company.ward || ''}, ${company.district || ''}, ${company.province || ''}`
                    : 'N/A';

                return `
                    <div class="col">
                        <a href="/JobSeeker/Company/Details/${company.id}" class="text-decoration-none">
                            <div class="card h-100 shadow-sm">
                                <!-- Company Logo -->
                                ${company.logo ? `
                                    <img src="${company.logo}" class="card-img-top" alt="${company.name || 'Company'} Logo" style="height: 150px; object-fit: contain; padding: 10px;">
                                ` : `
                                    <div class="card-img-top bg-light d-flex align-items-center justify-content-center" style="height: 150px;">
                                        <span class="text-muted">No Logo</span>
                                    </div>
                                `}

                                <div class="card-body">
                                    <h5 class="card-title text-primary">${company.name || 'Untitled'}</h5>
                                    <p class="card-text text-muted">
                                        <i class="bi bi-geo-alt-fill me-2"></i>
                                        ${location}
                                    </p>
                                    <p class="card-text text-muted">
                                        <i class="bi bi-building me-2"></i>
                                        Industry: ${company.industry || 'N/A'}
                                    </p>
                                    <p class="card-text text-muted">
                                        <i class="bi bi-people-fill me-2"></i>
                                        Size: ${company.size || 'N/A'}
                                    </p>
                                </div>
                            </div>
                        </a>
                    </div>
                `;
            });
            $companyListings.append(companyCards.join(''));
        } else {
            $companyListings.html('<div class="text-center"><p class="text-muted">No companies available at the moment.</p></div>');
        }
    }

    function fetchCompanies(page) {
        var params = new URLSearchParams();
        if (search) params.append('search', search);
        params.append('page', page);
        params.append('pageSize', pageSize);

        $('#loading-spinner').removeClass('d-none');
        $('#company-listings').addClass('opacity-50');

        $.ajax({
            url: `/api/Company/filter-companies?${params.toString()}`,
            method: 'GET',
            dataType: 'json',
            success: function (response) {
                updateCompanyListings(response.companies);
                if (response.totalCompanies !== totalCompanies) {
                    totalCompanies = response.totalCompanies;
                    $('#pagination-container').pagination('destroy');
                    initializePagination(page, totalCompanies);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching companies:', error);
                alert('Error loading companies. Please try again.');
            },
            complete: function () {
                $('#loading-spinner').addClass('d-none');
                $('#company-listings').removeClass('opacity-50');
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
                fetchCompanies(pagination.pageNumber);
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

    initializePagination(currentPage, totalCompanies);

    // Handle search form submission
    $('#search-form').on('submit', function (e) {
        e.preventDefault(); // Prevent default form submission
        search = $(this).find('input[name="search"]').val().trim();
        currentPage = 1; // Reset to first page on new search
        fetchCompanies(currentPage);
        // Update pagination
        $('#pagination-container').pagination('go', 1);
    });
});