$(document).ready(function() {
	$(".button-collapse").sideNav();
	$(".dropdown-button").dropdown();

	$("#user-dropdown-logoff").click(function() {
		$("#user-dropdown-logoutform").submit();
	});
});
