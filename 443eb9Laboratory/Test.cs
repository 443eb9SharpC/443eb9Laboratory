//using _443eb9Laboratory.Database;
//using _443eb9Laboratory.DataModels;
//using _443eb9Laboratory.DataModels.ETCC;
//using _443eb9Laboratory.Utils;

//namespace _443eb9Laboratory;

//public class Test
//{
//    public static void Tst()
//    {
//        Dictionary<ConditionType, float[]> dic = new Dictionary<ConditionType, float[]>();

//        dic[ConditionType.Temperature] = new float[] { 50, 5 };
//        dic[ConditionType.Hudimity] = new float[] { 95, 20 };
//        dic[ConditionType.Illumination] = new float[] { 50000, 0 };
//        dic[ConditionType.CarbonDioxide] = new float[] { 1000, 100 };
//        dic[ConditionType.PH] = new float[] { 12, 2 };

//        IOOperator.ToJson("./Data/ModuleDataRange.json", dic);
//    }
//}
