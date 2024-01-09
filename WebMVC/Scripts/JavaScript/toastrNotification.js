function showToastrNotification(message, type) {
    toastr.options = {
        "positionClass": "toast-bottom-right",
        "progressBar": true,
        "preventDuplicates": true,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "toastClass": "toast-" + type
    };

    toastr[type](message);
}
