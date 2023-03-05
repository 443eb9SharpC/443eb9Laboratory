import { connection, ipAddress } from "../signalr.js";
import { chunkPanel } from "./etcc-common.js";
import { InformationType, OperationType, cropToIcon, ChamberStorage } from "./etcc-datamodels.js";

connection.on('getStorageInfo', getStorageInfo)

let chamberStorage: ChamberStorage
let productsContainer = document.querySelector('.seed-container ul') as HTMLUListElement

connection.invoke('ETCC_SendInformation', InformationType.ETCC_Storage, connection.connectionId, ipAddress)

export function getStorageInfo(json: string) {
    chamberStorage = JSON.parse(json)
    renderSeedInfo()
    let seedProduct = document.querySelectorAll('.seed-container .seed-item') as NodeListOf<HTMLLIElement>
    for (let i = 0; i < seedProduct.length; i++) {
        seedProduct[i].removeEventListener('click', openChunkPanel)
        seedProduct[i].addEventListener('click', openChunkPanel)
    }
}

function renderSeedInfo() {
    productsContainer.innerHTML = ''
    for (let i = 0; i < chamberStorage.seeds.length; i++) {
        let seedItem =
            `<li class="seed-item clearfix">
                <i class="seed-icon icon">`+ cropToIcon[chamberStorage.seeds[i].name] + `</i>
                <div class="description">
                    <h4 class="latin">`+ chamberStorage.seeds[i].latin + `</h4>
                    <h3 class="name">`+ chamberStorage.seeds[i].name + `种子</h3>
                    <h3 class="amount">`+ chamberStorage.seeds[i].amount + `</h3>
                </div>
            </li>`
        productsContainer.insertAdjacentHTML('beforeend', seedItem)
    }
}

let seedNameToPlant: string
function openChunkPanel(event: MouseEvent) {
    chunkPanel.style.display = 'inline-block'
    chunkPanel.style.transform = `translate(${event.clientX}px, ${event.clientY}px)`

    let target = event.target as HTMLElement
    while (!target.classList.contains('seed-item')) {
        target = target.parentElement
    }

    let seedNameElement = target.querySelector('.name') as HTMLTitleElement
    seedNameToPlant = seedNameElement.innerText
}

export function plantSeed(chunkId: string) {
    chunkPanel.style.display = 'none'
    connection.invoke('ETCC_ExecuteOperation', OperationType.ETCC_PlantSeed, connection.connectionId, ipAddress, [seedNameToPlant, chunkId])
}