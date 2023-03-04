namespace _443eb9Laboratory.DataModels.ETCC;

public class Crop
{
    public int id;
    public int name;
    public int buyPrice;
    public int sellingPrice;
    public float requiredTemperature;
    public float requiredHudimity;
    public float requiredIllumination;
    public float requiredCarbonDioxide;
    public float requiredpH;
    public float variantPotential;
    public float seedProductionRate;
    public long plantTime;
    public long growthCycle;
    public List<VariantType> variant;
}
