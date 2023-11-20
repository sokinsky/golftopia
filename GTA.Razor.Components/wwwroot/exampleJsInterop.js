// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function showPrompt(message) {
 const result = prompt(message, 'Type anything here');
 console.log(result);
 return result;
}
export function logConsole(message) {
 return showPrompt(message);
}
export function alert(message) {
 alert(message);
}
