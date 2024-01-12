function showOverlay(delay) {
    setTimeout(function () {
        $("#overlay").show();
        $("#spinner-btn").show();
    }, delay);
}

function hideOverlay(delay) {
    setTimeout(function () {
        $("#overlay").hide();
        $("#spinner-btn").hide();
    }, delay);
}

showOverlay(0);
hideOverlay(1000);