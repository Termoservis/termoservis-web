/// <reference path="~/js/typings/jquery.d.ts"/>
/// <reference path="~/js/parts/_portal.ts"/>

$(document).ready(() => {
	// Initialize all components
	var portal = new Portal.PortalMenu($("#portal-nav")[0]);

	// Load all components
	portal.loaded();
});
