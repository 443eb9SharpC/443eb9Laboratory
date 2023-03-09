import { connection, ipAddress } from "../signalr.js";
import { Crop, InformationType, cropToIcon, OperationType } from "./etcc-datamodels.js";

connection.on('getSeedStoreInfo', getSeedStoreInfo)

connection.invoke('ETCC_SendInformation', InformationType.ETCC_SeedStore, connection.connectionId, ipAddress)

enum Selection {
    Seed,
    Chunk,
    Module
}

let currentSelection = Selection.Seed
let selectionSwitchers = document.querySelectorAll('.store .selection') as NodeListOf<HTMLElement>

let seedPage = document.querySelector('.store .seed-products-container') as HTMLElement
let chunkPage = document.querySelector('.store .chunk-products-container') as HTMLElement
let modulePage = document.querySelector('.store .module-products-container') as HTMLElement

chunkPage.style.display = 'none'
modulePage.style.display = 'none'

selectionSwitchers.forEach(switcher => {
    switcher.addEventListener('click', switchPage)
})

function switchPage(event: MouseEvent) {
    let target = event.target as HTMLElement

    switch (currentSelection) {
        case Selection.Seed:
            seedPage.style.display = 'none'
            break
        case Selection.Chunk:
            chunkPage.style.display = 'none'
            break
        case Selection.Module:
            modulePage.style.display = 'none'
            break
    }

    switch (target.innerText) {
        case '种子':
            seedPage.style.display = 'block'
            currentSelection = Selection.Seed
            break
        case '区块':
            chunkPage.style.display = 'block'
            currentSelection = Selection.Chunk
            break
        case '模块':
            modulePage.style.display = 'block'
            currentSelection = Selection.Module
            break
    }
}

let seedStore: Crop[]
let seedContainer = document.querySelector('.store .seed-products-container ul') as HTMLUListElement

export function getSeedStoreInfo(json: string) {
    seedStore = JSON.parse(json)
    renderSeedStoreInfo()
    let seedProduct = document.querySelectorAll('.store .seed-products-container .seed-product') as NodeListOf<HTMLLIElement>
    for (let i = 0; i < seedProduct.length; i++) {
        seedProduct[i].addEventListener('click', buySeed)
    }
}

function renderSeedStoreInfo() {
    for (let i = 0; i < seedStore.length; i++) {
        let seedProduct =
            `<li class="seed-product product clearfix">
            <i class="seed-icon icon">`+ cropToIcon[seedStore[i].name] + `</i>
            <div class="description">
                <h4 class="latin">`+ seedStore[i].latin + `</h4>
                <h3 class="name">`+ seedStore[i].name + `种子</h3>
                <h3 class="price">`+ seedStore[i].buyPrice + `μ</h3>
            </div>
        </li>`
        seedContainer.insertAdjacentHTML('beforeend', seedProduct)
    }
}

function buySeed(event: MouseEvent) {
    let target = event.target as HTMLElement
    while (!target.classList.contains('product')) {
        target = target.parentElement
    }
    let seedName = target.querySelector('.name') as HTMLTitleElement
    connection.invoke('ETCC_ExecuteOperation', OperationType.ETCC_BuySeed, connection.connectionId, ipAddress, [seedName.innerText])
}

let chunkProduct = document.querySelectorAll('.store .chunk-products-container .chunk-product') as NodeListOf<HTMLElement>

chunkProduct.forEach(product => {
    product.addEventListener('click', buyChunk)
});

function buyChunk(event: MouseEvent) {
    let target = event.target as HTMLElement
    while (!target.classList.contains('product')) {
        target = target.parentElement
    }
    let chunkId = target.querySelector('.product-id') as HTMLElement
    connection.invoke('ETCC_ExecuteOperation', OperationType.ETCC_BuyChunk, connection.connectionId, ipAddress, [chunkId.innerText])
}

let moduleProduct = document.querySelectorAll('.store .module-products-container .module-products-container') as NodeListOf<HTMLElement>
moduleProduct.forEach(product => {
    product.addEventListener('click', buyModule)
});

function buyModule(event: MouseEvent) {
    let target = event.target as HTMLElement
    while (!target.classList.contains('product')) {
        target = target.parentElement
    }
    let moduleName = target.querySelector('.product-id') as HTMLElement
    connection.invoke('ETCC_ExecuteOperation', OperationType.ETCC_BuyModule, connection.connectionId, ipAddress, [moduleName.innerText])
}