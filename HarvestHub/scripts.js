document.addEventListener("DOMContentLoaded", function () {
    const navbar = document.querySelector('.navbar');
    let lastScrollTop = 0;
    const hideThreshold = 100; // scroll 100px before hiding

    window.addEventListener('scroll', function () {
        let scrollTop = window.pageYOffset || document.documentElement.scrollTop;

        if (scrollTop > lastScrollTop && scrollTop > hideThreshold) {
            // Scrolling down after 100px
            navbar.classList.add('navbar-hidden');
        } else {
            // Scrolling up
            navbar.classList.remove('navbar-hidden');
        }
        lastScrollTop = scrollTop <= 0 ? 0 : scrollTop;
    });
});

