using _443eb9Laboratory.Utils;

namespace _443eb9Laboratory.DataModels.ETCC;

public class Module
{
    public float value;
    public ConditionType conditionType;

    public static Dictionary<ConditionType, float[]> valueRange =
        IOOperator.ReadJson<Dictionary<ConditionType, float[]>>("./Data/ModuleDataRange.json");
}
