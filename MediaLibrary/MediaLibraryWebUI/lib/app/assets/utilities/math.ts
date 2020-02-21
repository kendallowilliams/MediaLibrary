/*
 * https://www.w3schools.com/js/js_random.asp
 * @param {number} min
 * @param {number} max
 */
export function getRandomInteger(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}