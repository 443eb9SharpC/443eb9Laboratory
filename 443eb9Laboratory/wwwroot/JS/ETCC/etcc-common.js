import { connection, ipAddress } from "../signalr.js";
import { InformationType } from "./etcc-datamodels.js";
import { plantSeed } from "./etcc-storage.js";
let dashBoardButton = document.querySelector('.dashboard .tab');
let chunksButton = document.querySelector('.chunks .tab');
let storeButton = document.querySelector('.store .tab');
let marketButton = document.querySelector('.market .tab');
let storageButton = document.querySelector('.storage .tab');
let tabs = document.querySelectorAll('.box-container');
var Tab;
(function (Tab) {
    Tab[Tab["DashBoard"] = 0] = "DashBoard";
    Tab[Tab["Chunks"] = 1] = "Chunks";
    Tab[Tab["Store"] = 2] = "Store";
    Tab[Tab["Market"] = 3] = "Market";
    Tab[Tab["Storage"] = 4] = "Storage";
})(Tab || (Tab = {}));
let currentTab = Tab.DashBoard;
dashBoardButton.addEventListener('click', function () {
    if (currentTab != Tab.DashBoard) {
        closeTab();
        tabs[0].style.display = 'block';
        currentTab = Tab.DashBoard;
    }
});
chunksButton.addEventListener('click', function () {
    if (currentTab != Tab.Chunks) {
        closeTab();
        tabs[1].style.display = 'block';
        currentTab = Tab.Chunks;
    }
});
storeButton.addEventListener('click', function () {
    if (currentTab != Tab.Store) {
        closeTab();
        tabs[2].style.display = 'block';
        currentTab = Tab.Store;
    }
});
marketButton.addEventListener('click', function () {
    if (currentTab != Tab.Market) {
        closeTab();
        tabs[3].style.display = 'block';
        currentTab = Tab.Market;
    }
});
storageButton.addEventListener('click', function () {
    if (currentTab != Tab.Storage) {
        closeTab();
        tabs[4].style.display = 'block';
        currentTab = Tab.Storage;
    }
});
function closeTab() {
    switch (currentTab) {
        case Tab.DashBoard:
            tabs[0].style.display = 'none';
            break;
        case Tab.Chunks:
            tabs[1].style.display = 'none';
            break;
        case Tab.Store:
            tabs[2].style.display = 'none';
            break;
        case Tab.Market:
            tabs[3].style.display = 'none';
            break;
        case Tab.Storage:
            tabs[4].style.display = 'none';
            chunkPanel.style.display = 'none';
            break;
    }
}
connection.invoke('ETCC_SendInformation', InformationType.ETCC_Asset, connection.connectionId, ipAddress);
connection.on('getAssetInfo', getAssetInfo);
let assets = document.querySelectorAll('.assets');
export function getAssetInfo(totalAsset) {
    assets.forEach(element => {
        element.innerText = totalAsset + 'μ';
    });
}
let chunkPanel = document.querySelector('.chunk-select-panel');
let chunkButtons = document.querySelectorAll('.chunk-select-panel td');
chunkButtons.forEach(element => {
    element.addEventListener('click', selectChunk);
});
function selectChunk(event) {
    let target = event.target;
    switch (currentTab) {
        case Tab.Storage:
            plantSeed(target.innerText);
            break;
    }
}
export { chunkPanel };
