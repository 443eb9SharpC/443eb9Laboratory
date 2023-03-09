import { connection, ipAddress } from "../signalr.js";
import { InformationType, cropToIcon, OperationType } from "./etcc-datamodels.js";
connection.on('getSeedStoreInfo', getSeedStoreInfo);
connection.invoke('ETCC_SendInformation', InformationType.ETCC_SeedStore, connection.connectionId, ipAddress);
var Selection;
(function (Selection) {
    Selection[Selection["Seed"] = 0] = "Seed";
    Selection[Selection["Chunk"] = 1] = "Chunk";
    Selection[Selection["Module"] = 2] = "Module";
})(Selection || (Selection = {}));
let currentSelection = Selection.Seed;
let selectionSwitchers = document.querySelectorAll('.store .selection');
let seedPage = document.querySelector('.store .seed-products-container');
let chunkPage = document.querySelector('.store .chunk-products-container');
let modulePage = document.querySelector('.store .module-products-container');
chunkPage.style.display = 'none';
modulePage.style.display = 'none';
selectionSwitchers.forEach(switcher => {
    switcher.addEventListener('click', switchPage);
});
function switchPage(event) {
    let target = event.target;
    switch (currentSelection) {
        case Selection.Seed:
            seedPage.style.display = 'none';
            break;
        case Selection.Chunk:
            chunkPage.style.display = 'none';
            break;
        case Selection.Module:
            modulePage.style.display = 'none';
            break;
    }
    switch (target.innerText) {
        case '种子':
            seedPage.style.display = 'block';
            currentSelection = Selection.Seed;
            break;
        case '区块':
            chunkPage.style.display = 'block';
            currentSelection = Selection.Chunk;
            break;
        case '模块':
            modulePage.style.display = 'block';
            currentSelection = Selection.Module;
            break;
    }
}
let seedStore;
let seedContainer = document.querySelector('.store .seed-products-container ul');
export function getSeedStoreInfo(json) {
    seedStore = JSON.parse(json);
    renderSeedStoreInfo();
    let seedProduct = document.querySelectorAll('.store .seed-products-container .seed-product');
    for (let i = 0; i < seedProduct.length; i++) {
        seedProduct[i].addEventListener('click', buySeed);
    }
}
function renderSeedStoreInfo() {
    for (let i = 0; i < seedStore.length; i++) {
        let seedProduct = `<li class="seed-product product clearfix">
            <i class="seed-icon icon">` + cropToIcon[seedStore[i].name] + `</i>
            <div class="description">
                <h4 class="latin">` + seedStore[i].latin + `</h4>
                <h3 class="name">` + seedStore[i].name + `种子</h3>
                <h3 class="price">` + seedStore[i].buyPrice + `μ</h3>
            </div>
        </li>`;
        seedContainer.insertAdjacentHTML('beforeend', seedProduct);
    }
}
function buySeed(event) {
    let target = event.target;
    while (!target.classList.contains('product')) {
        target = target.parentElement;
    }
    let seedName = target.querySelector('.name');
    connection.invoke('ETCC_ExecuteOperation', OperationType.ETCC_BuySeed, connection.connectionId, ipAddress, [seedName.innerText]);
}
let chunkProduct = document.querySelectorAll('.store .chunk-products-container .chunk-product');
chunkProduct.forEach(product => {
    product.addEventListener('click', buyChunk);
});
function buyChunk(event) {
    let target = event.target;
    while (!target.classList.contains('product')) {
        target = target.parentElement;
    }
    let chunkId = target.querySelector('.product-id');
    connection.invoke('ETCC_ExecuteOperation', OperationType.ETCC_BuyChunk, connection.connectionId, ipAddress, [chunkId.innerText]);
}
let moduleProduct = document.querySelectorAll('.store .module-products-container .module-products-container');
moduleProduct.forEach(product => {
    product.addEventListener('click', buyModule);
});
function buyModule(event) {
    let target = event.target;
    while (!target.classList.contains('product')) {
        target = target.parentElement;
    }
    let moduleName = target.querySelector('.product-id');
    connection.invoke('ETCC_ExecuteOperation', OperationType.ETCC_BuyModule, connection.connectionId, ipAddress, [moduleName.innerText]);
}
