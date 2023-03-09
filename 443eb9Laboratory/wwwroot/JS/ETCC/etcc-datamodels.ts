export enum ConditionType {
    Temperature,
    Hudimity,
    Illumination,
    CarbonDioxide,
    PH
}

export enum InformationType {
    ETCC_DashBoard,
    ETCC_Chunks,
    ETCC_Asset,
    ETCC_SeedStore,
    ETCC_Storage,
    ETCC_SeedMarket
}

export enum OperationType {
    ETCC_BuySeed,
    ETCC_PlantSeed,
    ETCC_Harvest,
    ETCC_SellFruit,
    ETCC_BuyChunk,
    ETCC_BuyModule,
    ETCC_ChangeModuleData
}

export interface Chamber {
    id: number
    level: number
    assets: number
    cropsTotalPlanted: number
    owner: string
    name: string
    description: string
    modulesJS: Module[]
}

export interface Module {
    value: number
    conditionType: ConditionType
}

export interface Crop {
    id: number
    latin: string
    name: string
    amount: number
    buyPrice: number
    sellPrice: number
    plantTimeJS: number
    growthCycleJS: number
}

export interface Chunk {
    level: number
    isLocked: boolean
    cropOn: Crop
}

export interface ChamberStorage {
    seeds: Crop[]
    fruits: Crop[]
}

const cropToIcon: { [key: string]: string } = {}
cropToIcon['防风草'] = '&#xe602;'
cropToIcon['草莓'] = '&#xe6f8;'
cropToIcon['番茄'] = '&#xe603;'
cropToIcon['马铃薯'] = '&#xe62c;'
cropToIcon['西瓜'] = '&#xf850;'
cropToIcon['蓝莓'] = '&#xe624;'
export { cropToIcon }

const greekToNumber: { [key: string]: number } = {}
greekToNumber['alpha'] = 0
greekToNumber['beta'] = 1
greekToNumber['gamma'] = 2
greekToNumber['delta'] = 3
greekToNumber['epsilon'] = 4
greekToNumber['zeta'] = 5
greekToNumber['eta'] = 6
greekToNumber['theta'] = 7
greekToNumber['iota'] = 8
export { greekToNumber }