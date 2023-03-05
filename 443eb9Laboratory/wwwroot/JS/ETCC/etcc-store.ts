import { connection, ipAddress } from "../signalr.js";
import { Crop, InformationType, cropToIcon, OperationType } from "./etcc-datamodels.js";

connection.on('getSeedStoreInfo', getSeedStoreInfo)

let seedStore: Crop[]
let productsContainer = document.querySelector('.item-container ul') as HTMLUListElement

connection.invoke('ETCC_SendInformation', InformationType.ETCC_SeedStore, connection.connectionId, ipAddress)

export function getSeedStoreInfo(json: string) {
    seedStore = JSON.parse(json)
    renderSeedStoreInfo()
    let seedProduct = document.querySelectorAll('.item-container .seed-product') as NodeListOf<HTMLLIElement>
    for (let i = 0; i < seedProduct.length; i++) {
        seedProduct[i].addEventListener('click', buySeed)
    }
}

function renderSeedStoreInfo() {
    for (let i = 0; i < seedStore.length; i++) {
        let seedProduct =
            `<li class="seed-product clearfix">
            <i class="seed-icon icon">`+ cropToIcon[seedStore[i].name] + `</i>
            <div class="description">
            <h4 class="latin">`+ seedStore[i].latin + `</h4>
            <h3 class="name">`+ seedStore[i].name + `</h3>
            <h3 class="price">`+ seedStore[i].buyPrice + `μ</h3>
            </div>
        </li>`
        productsContainer.insertAdjacentHTML('beforeend', seedProduct)
    }
}

function buySeed(event: MouseEvent) {
    let target = event.target as HTMLElement
    while (!target.classList.contains('seed-product')) {
        target = target.parentElement
    }
    let seedName = target.querySelector('.name') as HTMLTitleElement
    connection.invoke('ETCC_ExecuteOperation', OperationType.ETCC_BuySeed, connection.connectionId, ipAddress, [seedName.innerText])
}