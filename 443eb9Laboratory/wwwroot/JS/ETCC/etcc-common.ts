import { connection, ipAddress } from "../signalr.js";

let dashBoardButton = document.querySelector('.dashboard .tab') as HTMLButtonElement
let chunksButton = document.querySelector('.chunks .tab') as HTMLButtonElement
let storeButton = document.querySelector('.store .tab') as HTMLButtonElement
let marketButton = document.querySelector('.market .tab') as HTMLButtonElement

let tabs = document.querySelectorAll('.box-container') as NodeListOf<HTMLDivElement>

enum Tab {
    DashBoard,
    Chunks,
    Store,
    Market
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
    }
}