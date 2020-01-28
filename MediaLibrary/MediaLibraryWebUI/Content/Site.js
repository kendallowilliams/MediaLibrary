/**
 * https://www.w3schools.com/js/js_random.asp
 * @param {any} min
 * @param {any} max
 */
function getRandomInteger(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}