import { connection, ipAddress } from "../signalr.js";
import { InformationType, cropToIcon } from "./etcc-datamodels.js";
connection.on('getChunksInfo', getChunksInfo);
let chunks;
let chunkIcons = document.querySelectorAll('.chunk .icon');
let chunkIds = document.querySelectorAll('.chunk .id');
let chunkNames = document.querySelectorAll('.chunk .name');
let timeRemainTitles = document.querySelectorAll('.chunk .title');
let timeRemains = document.querySelectorAll('.chunk .time-remain');
connection.invoke('ETCC_SendInformation', InformationType.ETCC_Chunks, connection.connectionId, ipAddress);
export function getChunksInfo(json) {
    chunks = JSON.parse(json);
    renderInfo();
}
function renderInfo() {
    for (let i = 0; i < chunks.length; i++) {
        if (chunks[i].isLocked) {
            chunkIcons[i].innerHTML = '&#xe7c9;';
            chunkIds[i].innerText = '';
            chunkNames[i].innerText = '';
            timeRemainTitles[i].innerText = '拒绝访问';
            timeRemains[i].innerText = '';
            continue;
        }
        if (chunks[i].cropOn == null) {
            chunkIcons[i].innerHTML = '&#xe70a;';
            chunkIds[i].innerText = '';
            chunkNames[i].innerText = '';
            timeRemainTitles[i].innerText = '未种植任何作物';
            timeRemains[i].innerText = '';
            continue;
        }
        chunkIcons[i].innerHTML = cropToIcon[chunks[i].cropOn.name];
        chunkIds[i].innerText = '#' + chunks[i].cropOn.id;
        chunkNames[i].innerText = chunks[i].cropOn.name;
    }
    updateTime();
}
async function updateTime() {
    while (true) {
        for (let i = 0; i < chunks.length; i++) {
            if (chunks[i].isLocked)
                continue;
            if (chunks[i].cropOn == null)
                continue;
            let remain = new Date(Date.now() - chunks[i].cropOn.plantTime - chunks[i].cropOn.growthCycle);
            timeRemains[i].innerText = `${remain.getDay()}D ${remain.getHours()}:${remain.getMinutes()}:${remain.getSeconds}`;
        }
        await new Promise(resolver => setTimeout(resolver, 500));
    }
}
