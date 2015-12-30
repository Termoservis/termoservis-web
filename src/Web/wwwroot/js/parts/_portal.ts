/// <reference path="~/js/typings/jquery.d.ts"/>

module Portal {
	export class PortalMenu {
		private menu: HTMLElement;


		constructor(menu: HTMLElement) {
			this.menu = menu;
		}


		loaded(): void {
			$(".collapsible", $(this.menu)).collapsible({
				accordion: true // A setting that changes the collapsible behavior to expandable instead of the default accordion style
			});
		}
	}
}
