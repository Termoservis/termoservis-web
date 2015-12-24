/// <reference path="~/js/typings/jquery.d.ts"/>
var Portal;
(function (Portal) {
    var PortalMenu = (function () {
        function PortalMenu(menu) {
            this.menu = menu;
        }
        PortalMenu.prototype.loaded = function () {
            $(".collapsible", $(this.menu)).collapsible({
                accordion: true // A setting that changes the collapsible behavior to expandable instead of the default accordion style
            });
        };
        return PortalMenu;
    })();
    Portal.PortalMenu = PortalMenu;
})(Portal || (Portal = {}));
