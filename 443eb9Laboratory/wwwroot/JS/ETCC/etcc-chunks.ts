import { connection, ipAddress } from "../signalr.js";
import { Chunk, InformationType, cropToIcon, greekToNumber, OperationType } from "./etcc-datamodels.js";

connection.on('getChunksInfo', getChunksInfo)

let chunks: Chunk[]

let chunkIcons = document.querySelectorAll('.chunk .icon') as NodeListOf<HTMLElement>
let chunkIds = document.querySelectorAll('.chunk .id') as NodeListOf<HTMLElement>
let chunkNames = document.querySelectorAll('.chunk .name') as NodeListOf<HTMLElement>
let timeRemainTitles = document.querySelectorAll('.chunk .title') as NodeListOf<HTMLTitleElement>
let timeRemains = document.querySelectorAll('.chunk .time-remain') as NodeListOf<HTMLElement>

connection.invoke('ETCC_SendInformation', InformationType.ETCC_Chunks, connection.connectionId, ipAddress)

export function getChunksInfo(json: string) {
    chunks = JSON.parse(json)
    renderInfo()
}

function renderInfo() {
    for (let i = 0; i < chunks.length; i++) {
        if (chunks[i].isLocked) {
            chunkIcons[i].innerHTML = '&#xe7c9;'
            chunkIds[i].innerText = ''
            chunkNames[i].innerText = ''
            timeRemainTitles[i].innerText = '拒绝访问'
            timeRemains[i].innerText = ''
            continue
        }
        if (chunks[i].plantOn == null) {
            chunkIcons[i].innerHTML = '&#xe70a;'
            chunkIds[i].innerText = ''
            chunkNames[i].innerText = ''
            timeRemainTitles[i].innerText = '未种植任何作物'
            timeRemains[i].innerText = ''
            continue
        }
        chunkIcons[i].innerHTML = cropToIcon[chunks[i].plantOn.name]
        chunkIds[i].innerText = '#' + chunks[i].plantOn.id
        chunkNames[i].innerText = chunks[i].plantOn.name
        timeRemainTitles[i].innerText = '剩余成熟时间'
    }
    updateTime()
}

async function updateTime() {
    while (true) {
        for (let i = 0; i < chunks.length; i++) {
            if (chunks[i].isLocked) continue
            if (chunks[i].plantOn == null) continue

            const remainingTimeMs = chunks[i].plantOn.actualGrowthCycleJS - (Date.now() - chunks[i].plantOn.plantTimeJS);
            const remainingTime = new Date(remainingTimeMs);
            const remainingDays = Math.floor(remainingTimeMs / (1000 * 60 * 60 * 24));
            const remainingHours = remainingTime.getUTCHours();
            const remainingMinutes = remainingTime.getUTCMinutes();
            const remainingSeconds = remainingTime.getUTCSeconds();
            const formattedTime = `${remainingDays}D ${remainingHours}:${remainingMinutes}:${remainingSeconds}`;

            if (remainingTimeMs > 0) {
                timeRemains[i].innerText = formattedTime
            }
            else {
                timeRemainTitles[i].innerText = '作物已成熟'
                timeRemains[i].innerText = ''
            }
        }
        await new Promise(resolver => setTimeout(resolver, 1000))
    }
}

chunkIcons.forEach((icon) => {
    icon.addEventListener('click', harvest)
})

function harvest(event: MouseEvent) {
    let target = event.target as HTMLElement
    target = target.parentElement
    let chunkIndex = greekToNumber[target.classList[0]]
    if (chunks[chunkIndex].isLocked) return
    if (chunks[chunkIndex].plantOn == null) return
    
    connection.send('ETCC_ExecuteOperation', OperationType.ETCC_Harvest, connection.connectionId, ipAddress, [chunkIndex.toString()])
}