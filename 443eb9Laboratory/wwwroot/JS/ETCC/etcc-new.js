import { connection, ipAddress } from "../signalr.js";
connection.on('onChamberCreated', onChamberCreated);
let name = document.querySelector('.chamber-name');
let description = document.querySelector('.description');
let mask = document.querySelector('.mask');
let submitButton = document.querySelector('.button');
submitButton.addEventListener('click', newChamber);
function newChamber() {
    if (name.value == "" || description.value == "")
        return;
    connection.invoke('NewChamber', name.value, description.value, connection.connectionId, ipAddress);
}
export async function onChamberCreated() {
    mask.classList.add('.mask-enter');
    await new Promise(resolver => setTimeout(resolver, 500));
    location.href = './ETCC.html';
}
