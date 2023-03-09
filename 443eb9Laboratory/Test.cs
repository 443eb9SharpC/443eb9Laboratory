//using _443eb9Laboratory.Database;
//using _443eb9Laboratory.DataModels;
//using _443eb9Laboratory.DataModels.ETCC;
//using _443eb9Laboratory.Utils;

//namespace _443eb9Laboratory;

//public class Test
//{
//    public static void Tst()
//    {
//        Crop crop = new Crop()
//        {
//            id = 0,
//            amount = 1,
//            buyPrice = 100,
//            sellPrice = 100,
//            name = "防风草",
//            latin = "aaa",
//            variantPotential = 0,
//            seedProductionRate = 0,
//            plantTimeJS = 0,
//            growthCycleJS = 0,
//            plantTime = DateTime.MinValue,
//            growthCycle = TimeSpan.Zero,
//            variant = new List<VariantType>(),
//            requiredCondition = new Dictionary<ConditionType, float>()
//        };
//        crop.requiredCondition[ConditionType.Temperature] = 10;
//        crop.requiredCondition[ConditionType.Hudimity] = 10;
//        crop.requiredCondition[ConditionType.Illumination] = 10;
//        crop.requiredCondition[ConditionType.CarbonDioxide] = 10;
//        crop.requiredCondition[ConditionType.PH] = 10;
//        IOOperator.ToJson("./Data/Crops.json", crop);
//    }
//}
