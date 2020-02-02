/*
 * https://www.w3schools.com/js/js_random.asp
 * @param {number} min
 * @param {number} max
 */
function getRandomInteger(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

/* https://www.w3schools.com/howto/howto_js_fullscreen.asp */
function openFullscreen(obj) {
    if (obj.requestFullscreen) {
        obj.requestFullscreen();
    } else if (obj.mozRequestFullScreen) { /* Firefox */
        obj.mozRequestFullScreen();
    } else if (obj.webkitRequestFullscreen) { /* Chrome, Safari and Opera */
        obj.webkitRequestFullscreen();
    } else if (obj.msRequestFullscreen) { /* IE/Edge */
        obj.msRequestFullscreen();
    }
}

/* https://www.w3schools.com/howto/howto_js_fullscreen.asp */
function closeFullscreen() {
    if (document.exitFullscreen) {
        document.exitFullscreen();
    } else if (document.mozCancelFullScreen) { /* Firefox */
        document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen) { /* Chrome, Safari and Opera */
        document.webkitExitFullscreen();
    } else if (document.msExitFullscreen) { /* IE/Edge */
        document.msExitFullscreen();
    }
}