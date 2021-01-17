var isBodyToggle = false;

window.Benriya = {
    toggleClass: className => {
        $('body').toggleClass(className);
        isBodyToggle = $('body').hasClass(className);
    },
    isBodyToggleSidebar: (className = null) => {
        return $('#layoutSidenav_nav').position().left < -100;
    },
    closeModal: id => {
        $(id).modal('hide');
    }
    
}
