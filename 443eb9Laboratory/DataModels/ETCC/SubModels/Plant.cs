namespace _443eb9Laboratory.DataModels.ETCC.SubModels;

public class Plant
{
    public int id;
    public string name;
    public string latin;
    public float seedProductionRate;
    public long plantTimeJS;
    public long growthCycleJS;
    public long actualGrowthCycleJS;
    public DateTime plantTime;
    public TimeSpan growthCycle;
    public TimeSpan actualGrowthCycle;
    public List<VariantType> variant;
    public Dictionary<ConditionType, float> requiredCondition;
}
