import { connection, ipAddress } from "../signalr.js";
import { chunkPanel } from "./etcc-common.js";
import { InformationType, OperationType, cropToIcon, ChamberStorage } from "./etcc-datamodels.js";

connection.on('getStorageInfo', getStorageInfo)

let chamberStorage: ChamberStorage
let seedContainer = document.querySelector('.seed-container ul') as HTMLUListElement
let fruitContainer = document.querySelector('.fruit-container ul') as HTMLUListElement

let seedPage = document.querySelector('.seed-container') as HTMLUListElement
let fruitPage = document.querySelector('.fruit-container') as HTMLUListElement

fruitPage.style.display = 'none'

connection.invoke('ETCC_SendInformation', InformationType.ETCC_Storage, connection.connectionId, ipAddress)

export function getStorageInfo(json: string) {
    chamberStorage = JSON.parse(json)
    renderSeedInfo()
    renderFruitInfo()
    let seedProduct = document.querySelectorAll('.seed-container .seed-product') as NodeListOf<HTMLLIElement>
    for (let i = 0; i < seedProduct.length; i++) {
        seedProduct[i].removeEventListener('click', openChunkPanel)
        seedProduct[i].addEventListener('click', openChunkPanel)
    }
}

function renderSeedInfo() {
    seedContainer.innerHTML = ''
    for (let i = 0; i < chamberStorage.seeds.length; i++) {
        let seedItem =
            `<li class="seed-product product clearfix">
                <i class="icon">`+ cropToIcon[chamberStorage.seeds[i].name] + `</i>
                <div class="description">
                    <h4 class="latin">`+ chamberStorage.seeds[i].latin + `</h4>
                    <h3 class="name">`+ chamberStorage.seeds[i].name + `种子</h3>
                    <h3 class="amount">`+ chamberStorage.seeds[i].amount + `</h3>
                </div>
            </li>`
        seedContainer.insertAdjacentHTML('beforeend', seedItem)
    }
}

function renderFruitInfo() {
    fruitContainer.innerHTML = ''
    for (let i = 0; i < chamberStorage.fruits.length; i++) {
        let fruitItem =
            `<li class="fruit-product product clearfix">
                <i class="icon">`+ cropToIcon[chamberStorage.fruits[i].name] + `</i>
                <div class="description">
                    <h4 class="latin">`+ chamberStorage.fruits[i].latin + `</h4>
                    <h3 class="name">`+ chamberStorage.fruits[i].name + `</h3>
                    <h3 class="amount">`+ chamberStorage.fruits[i].amount + `</h3>
                </div>
            </li>`
        fruitContainer.insertAdjacentHTML('beforeend', fruitItem)
    }
}

let seedNameToPlant: string
function openChunkPanel(event: MouseEvent) {
    chunkPanel.style.display = 'inline-block'
    chunkPanel.style.transform = `translate(${event.clientX}px, ${event.clientY}px)`

    let target = event.target as HTMLElement
    while (!target.classList.contains('seed-product')) {
        target = target.parentElement
    }

    let seedNameElement = target.querySelector('.name') as HTMLTitleElement
    seedNameToPlant = seedNameElement.innerText
}

export function plantSeed(chunkId: string) {
    chunkPanel.style.display = 'none'
    connection.invoke('ETCC_ExecuteOperation', OperationType.ETCC_PlantSeed, connection.connectionId, ipAddress, [seedNameToPlant, chunkId])
}

enum Selection {
    Seed,
    Fruit
}

let currentSelection = Selection.Seed
let selectionSwitchers = document.querySelectorAll('.storage .selection') as NodeListOf<HTMLElement>

selectionSwitchers.forEach(switcher => {
    switcher.addEventListener('click', switchPage)
})

function switchPage(event: MouseEvent) {
    let target = event.target as HTMLElement

    switch (currentSelection) {
        case Selection.Seed:
            seedPage.style.display = 'none'
            break
        case Selection.Fruit:
            fruitPage.style.display = 'none'
            break
    }

    switch (target.innerText) {
        case '种子':
            seedPage.style.display = 'block'
            currentSelection = Selection.Seed
            break
        case '作物':
            fruitPage.style.display = 'block'
            currentSelection = Selection.Fruit
    }
}