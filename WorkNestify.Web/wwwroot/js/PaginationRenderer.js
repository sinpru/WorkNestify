$(document).ready(function () {
    // Get the current page from the URL
    var urlParams = new URLSearchParams(window.location.search);
    var currentPageFromUrl = parseInt(urlParams.get('page')) || 1;
    var isInitialLoad = true; // Flag to track initial load

    $('#pagination-container').pagination({
        dataSource: Array(totalJobs).fill({}),
        pageSize: pageSize,
        pageNumber: currentPage,
        totalNumber: totalJobs,
        className: 'paginationjs',
        alias: {
            pageNumber: 'page'
        },
        prevText: '«', // Previous arrow
        nextText: '»', // Next arrow
        showPrevious: true,
        showNext: true,
        pageRange: 2, // Controls how many pages are shown around the current page
        callback: function (data, pagination) {
            // Skip callback on initial load
            if (isInitialLoad) {
                isInitialLoad = false;
                return;
            }

            // Only navigate if the page has actually changed
            if (pagination.pageNumber !== currentPageFromUrl) {
                // Base URL without query parameters
                var baseUrl = baseUrlFromRazor;
                
                // Get current query parameters
                var search = searchFromRazor;
                var category = categoryFromRazor;
                var location = locationFromRazor;
                
                // Build query string using URLSearchParams
                var params = new URLSearchParams();
                if (search) params.append('search', search);
                if (category) params.append('category', category);
                if (location) params.append('location', location);
                params.append('page', pagination.pageNumber);
                
                // Construct the final URL
                var newUrl = baseUrl + '?' + params.toString();
                window.location.href = newUrl;
            }
        },
        afterRender: function () {
            // Apply Bootstrap classes
            $('#pagination-container .paginationjs-pages ul').addClass('pagination');
            $('#pagination-container .paginationjs-pages li').addClass('page-item');
            $('#pagination-container .paginationjs-pages li a').addClass('page-link');
            $('#pagination-container .paginationjs-pages li.active').addClass('active');
            $('#pagination-container .paginationjs-pages li.disabled').addClass('disabled');
        }
    });
});