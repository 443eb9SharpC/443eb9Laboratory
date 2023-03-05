import { connection, ipAddress } from "../signalr.js";
import { Crop, cropToIcon, InformationType, OperationType } from "./etcc-datamodels.js";

connection.on('getFruitMarketInfo', getFruitMarketInfo)

let fruitMarket: Crop[]
let fruitContainer = document.querySelector('.market .item-container ul') as HTMLElement

connection.invoke('ETCC_SendInformation', InformationType.ETCC_SeedMarket, connection.connectionId, ipAddress)

export function getFruitMarketInfo(json: string) {
    fruitMarket = JSON.parse(json)
    renderFruitMarketInfo()
    let fruitProduct = document.querySelectorAll('.market .item-container .fruit-product') as NodeListOf<HTMLElement>
    for (let i = 0; i < fruitMarket.length; i++) {
        fruitProduct[i].addEventListener('click', sellFruit)
    }
}

function renderFruitMarketInfo() {
    for (let i = 0; i < fruitMarket.length; i++) {
        let fruitProduct =
            `<li class="fruit-product product clearfix">
                <i class="fruit-icon icon">`+ cropToIcon[fruitMarket[i].name] + `</i>
                <div class="description">
                    <h4 class="latin">`+ fruitMarket[i].latin + `</h4>
                    <h3 class="name">`+ fruitMarket[i].name + `</h3>
                    <h3 class="price">`+ fruitMarket[i].sellPrice + `μ</h3>
                </div>
            </li>`
        fruitContainer.insertAdjacentHTML('beforeend', fruitProduct)
    }
}

function sellFruit(event: MouseEvent) {
    let target = event.target as HTMLElement
    while (!target.classList.contains('product')) {
        target = target.parentElement
    }
    let fruitName = target.querySelector('.name') as HTMLTitleElement
    connection.invoke('ETCC_ExecuteOperation', OperationType.ETCC_SellFruit, connection.connectionId, ipAddress, [fruitName.innerText])
}