export enum ModuleType {
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
    ETCC_Storage
}

export enum OperationType {
    ETCC_BuySeed,
    ETCC_PlantSeed
}

export interface Chamber {
    id: number
    level: number
    assets: number
    cropsTotalPlanted: number
    owner: string
    name: string
    description: string
    modules: Module[]
}

export interface Module {
    value: number
    moduleType: ModuleType
}

export interface Crop {
    id: number
    latin: string
    name: string
    amount: number
    buyPrice: number
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
}

const cropToIcon: { [key: string]: string } = {}
cropToIcon['防风草'] = '&#xe602;'
cropToIcon['草莓'] = '&#xe6f8;'
cropToIcon['番茄'] = '&#xe603;'
cropToIcon['马铃薯'] = '&#xe62c;'
cropToIcon['西瓜'] = '&#xf850;'
cropToIcon['蓝莓'] = '&#xe624;'
export { cropToIcon }