namespace _443eb9Laboratory.DataModels.ETCC;

public class Crop
{
    public int id;
    public int amount;
    public int buyPrice;
    public int sellingPrice;
    public string name;
    public string latin;
    public float requiredTemperature;
    public float requiredHudimity;
    public float requiredIllumination;
    public float requiredCarbonDioxide;
    public float requiredpH;
    public float variantPotential;
    public float seedProductionRate;
    public long plantTimeJS;
    public long growthCycleJS;
    public DateTime plantTime;
    public TimeSpan growthCycle;
    public List<VariantType> variant;
}
