(function($) {

	"use strict";

	var fullHeight = function() {

		$('.js-fullheight').css('height', $(window).height());
		$(window).resize(function(){
			$('.js-fullheight').css('height', $(window).height());
		});
        console.log('fullHeight() ...');
	};
	fullHeight();

	//$('#sidebarCollapse').on('click', function () {
	//	alert('xxx');
 //     $('#sidebar').toggleClass('active');
	//});
	console.log('Main.js is run...');
})(jQuery);