import { connection, ipAddress } from "../signalr.js";
import { InformationType } from "./etcc-datamodels.js";
connection.on('getDashBoardInfo', getDashBoardInfo);
let chamber;
let name = document.querySelector('.status-box .name');
let id = document.querySelector('.status-box .id');
let owner = document.querySelector('.status-box .owner');
let description = document.querySelector('.status-box .description');
let level = document.querySelector('.status-box .level');
let temp = document.querySelector('.temp-module .module-data');
let hudimity = document.querySelector('.hudimity-module .module-data');
let illumination = document.querySelector('.illumination-module .module-data');
let carbonDioxide = document.querySelector('.carbonDioxide-module .module-data');
let pH = document.querySelector('.pH-module .module-data');
connection.invoke('ETCC_SendInformation', InformationType.ETCC_DashBoard, connection.connectionId, ipAddress);
export function getDashBoardInfo(json) {
    chamber = JSON.parse(json);
    renderInfo();
}
function renderInfo() {
    name.innerText = chamber.name;
    id.innerText = '#' + chamber.id.toString();
    owner.innerText = chamber.owner;
    description.innerText = chamber.description;
    level.innerText = 'Lv. ' + chamber.level.toString();
    if (chamber.modules.length > 0 && chamber.modules[0] != null) {
        temp.innerText = chamber.modules[0].value.toString();
    }
    else {
        temp.innerText = "Unavaliable";
    }
    if (chamber.modules.length > 1 && chamber.modules[1] != null) {
        hudimity.innerText = chamber.modules[1].value.toString();
    }
    else {
        hudimity.innerText = "Unavaliable";
    }
    if (chamber.modules.length > 2 && chamber.modules[2] != null) {
        illumination.innerText = chamber.modules[2].value.toString();
    }
    else {
        illumination.innerText = "Unavaliable";
    }
    if (chamber.modules.length > 3 && chamber.modules[3] != null) {
        carbonDioxide.innerText = chamber.modules[3].value.toString();
    }
    else {
        carbonDioxide.innerText = "Unavaliable";
    }
    if (chamber.modules.length > 4 && chamber.modules[4] != null) {
        pH.innerText = chamber.modules[4].value.toString();
    }
    else {
        pH.innerText = "Unavaliable";
    }
    onPageSwitch();
}
async function onPageSwitch() {
    await new Promise(resolve => setTimeout(resolve, 400));
    let tabsBox = document.querySelector('.tabs-box');
    let titleBox = document.querySelector('.title-box');
    let moduleContainer = document.querySelector('.module-container');
    let statusBox = document.querySelector('.status-box');
    for (let i = 0; i < 3; i++) {
        tabsBox.style.opacity = "0";
        await new Promise(resolve => setTimeout(resolve, 30));
        titleBox.style.opacity = "0";
        statusBox.style.opacity = "0";
        await new Promise(resolve => setTimeout(resolve, 20));
        tabsBox.style.opacity = "0.7";
        moduleContainer.style.opacity = "0";
        statusBox.style.opacity = "0.7";
        await new Promise(resolve => setTimeout(resolve, 25));
        titleBox.style.opacity = "0.7";
        moduleContainer.style.opacity = "0.7";
        await new Promise(resolve => setTimeout(resolve, 35));
    }
    tabsBox.style.opacity = "1";
    titleBox.style.opacity = "1";
    moduleContainer.style.opacity = "1";
    statusBox.style.opacity = "1";
}