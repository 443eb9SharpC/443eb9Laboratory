import { connection, ipAddress } from "../signalr.js";
import { InformationType } from "./etcc-datamodels.js";
import { plantSeed } from "./etcc-storage.js";

let dashBoardButton = document.querySelector('.dashboard .tab') as HTMLButtonElement
let chunksButton = document.querySelector('.chunks .tab') as HTMLButtonElement
let storeButton = document.querySelector('.store .tab') as HTMLButtonElement
let marketButton = document.querySelector('.market .tab') as HTMLButtonElement
let storageButton = document.querySelector('.storage .tab') as HTMLButtonElement

let tabs = document.querySelectorAll('.box-container') as NodeListOf<HTMLDivElement>

enum Tab {
    DashBoard,
    Chunks,
    Store,
    Market,
    Storage
}

let currentTab = Tab.DashBoard

dashBoardButton.addEventListener('click', function () {
    if (currentTab != Tab.DashBoard) {
        closeTab()
        tabs[0].style.display = 'block'
        currentTab = Tab.DashBoard
    }
})

chunksButton.addEventListener('click', function () {
    if (currentTab != Tab.Chunks) {
        closeTab()
        tabs[1].style.display = 'block'
        currentTab = Tab.Chunks
    }
})

storeButton.addEventListener('click', function () {
    if (currentTab != Tab.Store) {
        closeTab()
        tabs[2].style.display = 'block'
        currentTab = Tab.Store
    }
})

marketButton.addEventListener('click', function () {
    if (currentTab != Tab.Market) {
        closeTab()
        tabs[3].style.display = 'block'
        currentTab = Tab.Market
    }
})

storageButton.addEventListener('click', function () {
    if (currentTab != Tab.Storage) {
        closeTab()
        tabs[4].style.display = 'block'
        currentTab = Tab.Storage
    }
})

function closeTab() {
    switch (currentTab) {
        case Tab.DashBoard:
            tabs[0].style.display = 'none'
            break
        case Tab.Chunks:
            tabs[1].style.display = 'none'
            break
        case Tab.Store:
            tabs[2].style.display = 'none'
            break
        case Tab.Market:
            tabs[3].style.display = 'none'
            break
        case Tab.Storage:
            tabs[4].style.display = 'none'
            chunkPanel.style.display = 'none'
            break
    }
}

connection.invoke('ETCC_SendInformation', InformationType.ETCC_Asset, connection.connectionId, ipAddress)

connection.on('getAssetInfo', getAssetInfo)

let assets = document.querySelectorAll('.assets') as NodeListOf<HTMLElement>

export function getAssetInfo(totalAsset: string) {
    assets.forEach(element => {
        element.innerText = totalAsset + 'μ'
    });
}

let chunkPanel = document.querySelector('.chunk-select-panel') as HTMLDivElement
let chunkButtons = document.querySelectorAll('.chunk-select-panel td') as NodeListOf<HTMLTableCellElement>
chunkButtons.forEach(element => {
    element.addEventListener('click', selectChunk)
});

function selectChunk(event: MouseEvent) {
    let target = event.target as HTMLElement
    switch (currentTab) {
        case Tab.Storage:
            plantSeed(target.innerText)
            break
    }
}

export { chunkPanel }