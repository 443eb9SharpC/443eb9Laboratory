using _443eb9Laboratory.DataModels.ETCC;
using _443eb9Laboratory.Utils;

namespace _443eb9Laboratory.Database;

public class CropDatabase
{
    public static List<Crop> crops = IOOperator.ReadJson<List<Crop>>("./Data/Crops.json");

    public static Crop GetCrop(string cropName)
    {
        return crops.Find(crop => crop.name == cropName);
    }

    public static Crop GetActualCropGrowthCycle(Crop targetCrop, Dictionary<ConditionType, float> moduleDatas)
    {
        if (targetCrop == null) return null;

        float effect = 0;
        foreach (ConditionType conditionType in moduleDatas.Keys)
        {
            effect += (targetCrop.requiredCondition[conditionType] - moduleDatas[conditionType]) / GetDataRangeInfo(conditionType)[2];
        }

        targetCrop.growthCycle *= Math.Pow(effect, 2);
        return targetCrop;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="conditionType"></param>
    /// <returns>Returns [maxValue, minValue, valueRange]</returns>
    public static float[] GetDataRangeInfo(ConditionType conditionType)
    {
        float maxValue;
        float minValue;

        switch (conditionType)
        {
            case ConditionType.Temperature:
                maxValue = 50;
                minValue = 5;
                break;
            case ConditionType.Hudimity:
                maxValue = 95;
                minValue = 20;
                break;
            case ConditionType.Illumination:
                maxValue = 300000;
                minValue = 0;
                break;
            case ConditionType.CarbonDioxide:
                maxValue = 1000;
                minValue = 100;
                break;
            case ConditionType.PH:
                maxValue = 12;
                minValue = 2;
                break;
            default:
                maxValue = -1;
                minValue = -1;
                break;
        }

        return new float[] { maxValue, minValue, maxValue - minValue };
    }
}
