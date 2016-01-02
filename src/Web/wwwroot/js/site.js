$(document).ready(function () {
	$(".button-collapse").sideNav();

	$(".owl-carousel").owlCarousel({
		items: 1,
		loop: true,
		autoplay: true,
		autoplayTimeout: 5000,
		autoplayHoverPause: true
	});
});