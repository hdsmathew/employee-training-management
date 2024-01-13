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

window.onload = (event) => {
    hideOverlay(500);
};