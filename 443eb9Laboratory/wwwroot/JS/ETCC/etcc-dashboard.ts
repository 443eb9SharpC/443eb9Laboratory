import { connection, ipAddress } from "../signalr.js";
import { ConditionType, InformationType, OperationType } from "./etcc-datamodels.js";
import { Chamber } from "./etcc-datamodels.js";

connection.on('getDashBoardInfo', getDashBoardInfo)
connection.on('getModuleValueRangeInfo', getModuleValueRangeInfo)

let isFirstLoad = true

let chamber: Chamber

let name = document.querySelector('.dashboard .status-box .name') as HTMLTitleElement
let id = document.querySelector('.dashboard .status-box .id') as HTMLTitleElement
let owner = document.querySelector('.dashboard .status-box .owner') as HTMLTitleElement
let description = document.querySelector('.dashboard .status-box .description') as HTMLTitleElement
let level = document.querySelector('.dashboard .status-box .level') as HTMLTitleElement

let moduleValueRange: { [key: string]: number[] } = {}

let moduleData = document.querySelectorAll('.dashboard .module-box .module-data') as NodeListOf<HTMLElement>

connection.invoke('ETCC_SendInformation', InformationType.ETCC_DashBoard, connection.connectionId, ipAddress)

export function getModuleValueRangeInfo(json: string) {
    moduleValueRange = JSON.parse(json)
}

export function getDashBoardInfo(json: string) {
    chamber = JSON.parse(json)
    renderInfo()
}

function renderInfo() {
    name.innerText = chamber.name
    id.innerText = '#' + chamber.id.toString()
    owner.innerText = chamber.owner
    description.innerText = chamber.description
    level.innerText = 'Lv. ' + chamber.level.toString()

    for (let i = 0; i < moduleData.length; i++) {
        if (chamber.modulesJS.length <= i) {
            moduleData[i].innerText = 'Unavaliable'
            continue
        }
        if (chamber.modulesJS[i] == null)
            moduleData[i].innerText = 'Unavaliable'
        if (chamber.modulesJS[i].value == -1)
            moduleData[i].innerText = 'NaN'
        else
            moduleData[i].innerText = chamber.modulesJS[i].value.toString()
    }

    if (isFirstLoad)
        onPageSwitch()
}

async function onPageSwitch() {
    await new Promise(resolve => setTimeout(resolve, 400))
    let tabsBox = document.querySelector('.tabs-box') as HTMLDivElement
    let titleBox = document.querySelector('.title-box') as HTMLDivElement
    let moduleContainer = document.querySelector('.module-container') as HTMLDivElement
    let statusBox = document.querySelector('.status-box') as HTMLDivElement

    for (let i = 0; i < 3; i++) {
        tabsBox.style.opacity = "0"
        await new Promise(resolve => setTimeout(resolve, 30))
        titleBox.style.opacity = "0"
        statusBox.style.opacity = "0"
        await new Promise(resolve => setTimeout(resolve, 20))
        tabsBox.style.opacity = "0.7"
        moduleContainer.style.opacity = "0"
        statusBox.style.opacity = "0.7"
        await new Promise(resolve => setTimeout(resolve, 25))
        titleBox.style.opacity = "0.7"
        moduleContainer.style.opacity = "0.7"
        await new Promise(resolve => setTimeout(resolve, 35))
    }
    tabsBox.style.opacity = "1"
    titleBox.style.opacity = "1"
    moduleContainer.style.opacity = "1"
    statusBox.style.opacity = "1"

    isFirstLoad = false
}

let moduleBoxes = document.querySelectorAll('.dashboard .module-box') as NodeListOf<HTMLDivElement>
let valueSlider = document.querySelector('.slider') as HTMLElement

let originalValue = NaN
moduleBoxes.forEach(box => {
    box.addEventListener('click', openSlider)
});

let selectedModuleId: number
let selectedConditionType: ConditionType
let selectedModuleRect: DOMRect
let sliderTrackRect: DOMRect

function openSlider(event: MouseEvent) {
    let target = event.target as HTMLElement
    while (!target.classList.contains('module-box')) {
        target = target.parentElement
    }

    switch (target.classList[0]) {
        case 'temp-module':
            selectedModuleId = 0
            selectedConditionType = ConditionType.Temperature
            break
        case 'hudimity-module':
            selectedModuleId = 1
            selectedConditionType = ConditionType.Hudimity
            break
        case 'illumination-module':
            selectedModuleId = 2
            selectedConditionType = ConditionType.Illumination
            break
        case 'carbonDioxide-module':
            selectedModuleId = 3
            selectedConditionType = ConditionType.CarbonDioxide
            break
        case 'pH-module':
            selectedModuleId = 4
            selectedConditionType = ConditionType.PH
            break
    }

    if (!chamber.unlockedModuleTypes.includes(selectedModuleId)) {
        valueSlider.style.display = 'none'
        onDrag = false
        return
    }

    if (valueSlider.style.display == 'none')
        valueSlider.style.display = 'block'
    else {
        valueSlider.style.display = 'none'
        if (selectedModuleId != null)
            moduleData[selectedModuleId].innerText = originalValue.toString()
        return
    }

    switch (selectedConditionType) {
        case ConditionType.Temperature:
            minValue = moduleValueRange['Temperature'][0]
            maxValue = moduleValueRange['Temperature'][1]
            break
        case ConditionType.Hudimity:
            minValue = moduleValueRange['Hudimity'][0]
            maxValue = moduleValueRange['Hudimity'][1]
            break
        case ConditionType.Illumination:
            minValue = moduleValueRange['Illumination'][0]
            maxValue = moduleValueRange['Illumination'][1]
            break
        case ConditionType.CarbonDioxide:
            minValue = moduleValueRange['CarbonDioxide'][0]
            maxValue = moduleValueRange['CarbonDioxide'][1]
            break
        case ConditionType.PH:
            minValue = moduleValueRange['PH'][0]
            maxValue = moduleValueRange['PH'][1]
            break
    }

    originalValue = parseFloat(moduleData[selectedModuleId].innerText)

    selectedModuleRect = moduleBoxes[selectedModuleId].getBoundingClientRect()
    let posX = selectedModuleRect.x
    let posY = selectedModuleRect.y + moduleBoxes[selectedModuleId].clientHeight
    let width = moduleBoxes[selectedModuleId].clientWidth

    valueSlider.style.transform = `translate(${posX}px, ${posY}px)`
    valueSlider.style.width = width.toString()

    posX = parseFloat(moduleData[selectedModuleId].innerText) / (maxValue - minValue) * sliderTrack.clientWidth - sliderTrack.clientWidth * 0.11
    if (Number.isNaN(posX))
        posX = 0
    sliderBlock.style.transform = `translate(${posX}px, 0)`

    sliderTrackRect = sliderTrack.getBoundingClientRect()
}

let sliderBlock = valueSlider.querySelector('.block') as HTMLElement
let sliderTrack = valueSlider.querySelector('.track') as HTMLElement
let silderTick = valueSlider.querySelector('.tick') as HTMLElement
sliderBlock.addEventListener('mousemove', changeSliderValue)
sliderBlock.addEventListener('mousedown', () => { onDrag = true })
sliderBlock.addEventListener('mouseup', () => { onDrag = false })
silderTick.addEventListener('click', submitSliderValue)

let onDrag = false

let maxValue: number
let minValue: number

function changeSliderValue(event: MouseEvent) {
    if (!onDrag) return

    let posX = event.clientX - sliderTrackRect.x
    if (event.clientX < sliderTrackRect.x || event.clientX > sliderTrackRect.right)
        return
    sliderBlock.style.transform = `translate(${posX}px, 0)`

    let value = posX / sliderTrack.offsetWidth
    moduleData[selectedModuleId].innerText = (minValue + (maxValue - minValue) * value).toFixed(2).toString()
}

function submitSliderValue() {
    valueSlider.style.display = 'none'
    connection.invoke('ETCC_ExecuteOperation', OperationType.ETCC_ChangeModuleData, connection.connectionId, ipAddress, [moduleData[selectedModuleId].innerText, selectedConditionType.toString()])
}