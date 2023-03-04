let dashBoardButton = document.querySelector('.dashboard .tab');
let chunksButton = document.querySelector('.chunks .tab');
let storeButton = document.querySelector('.store .tab');
let marketButton = document.querySelector('.market .tab');
let tabs = document.querySelectorAll('.box-container');
var Tab;
(function (Tab) {
    Tab[Tab["DashBoard"] = 0] = "DashBoard";
    Tab[Tab["Chunks"] = 1] = "Chunks";
    Tab[Tab["Store"] = 2] = "Store";
    Tab[Tab["Market"] = 3] = "Market";
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
    }
}
export {};
