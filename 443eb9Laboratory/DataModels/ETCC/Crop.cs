namespace _443eb9Laboratory.DataModels.ETCC;

public class Crop
{
    public int id;
    public int amount;
    public int buyPrice;
    public int sellPrice;
    public string name;
    public string latin;
    public float variantPotential;
    public float seedProductionRate;
    public long plantTimeJS;
    public long growthCycleJS;
    public DateTime plantTime;
    public TimeSpan growthCycle;
    public List<VariantType> variant;
    public Dictionary<ConditionType, float> requiredCondition;
}
