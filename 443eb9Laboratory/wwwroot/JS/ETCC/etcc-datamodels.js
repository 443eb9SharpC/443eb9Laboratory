export var ModuleType;
(function (ModuleType) {
    ModuleType[ModuleType["Temperature"] = 0] = "Temperature";
    ModuleType[ModuleType["Hudimity"] = 1] = "Hudimity";
    ModuleType[ModuleType["Illumination"] = 2] = "Illumination";
    ModuleType[ModuleType["CarbonDioxide"] = 3] = "CarbonDioxide";
    ModuleType[ModuleType["PH"] = 4] = "PH";
})(ModuleType || (ModuleType = {}));
export var InformationType;
(function (InformationType) {
    InformationType[InformationType["ETCC_DashBoard"] = 0] = "ETCC_DashBoard";
    InformationType[InformationType["ETCC_Chunks"] = 1] = "ETCC_Chunks";
})(InformationType || (InformationType = {}));
const cropToIcon = {};
cropToIcon['防风草'] = '&#xe602;';
cropToIcon['草莓'] = '&#xe6f8;';
cropToIcon['番茄'] = '&#xe603;';
cropToIcon['马铃薯'] = '&#xe62c;';
cropToIcon['西瓜'] = '&#xf850;';
cropToIcon['蓝莓'] = '&#xe624;';
export { cropToIcon };