using _443eb9Laboratory.DataModels.ETCC.SubModels;
using _443eb9Laboratory.Utils;

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
    public long actualGrowthCycleJS;
    public DateTime plantTime;
    public TimeSpan growthCycle;
    public TimeSpan actualGrowthCycle;
    public List<VariantType> variant;
    public Dictionary<ConditionType, float> requiredCondition;

    public static List<Crop> crops = IOOperator.ReadJson<List<Crop>>("./Data/Crops.json");

    public static Seed GetSeed(string cropName)
    {
        Crop crop = crops.Find(crop => crop.name == cropName);
        return new Seed()
        {
            amount = crop.amount,
            buyPrice = crop.buyPrice,
            name = crop.name,
            latin = crop.latin
        };
    }

    public static Fruit GetFruit(string fruitName)
    {
        Crop crop = crops.Find(crop => crop.name == fruitName);
        return new Fruit()
        {
            amount = crop.amount,
            sellPrice = crop.sellPrice,
            name = crop.name,
            latin = crop.latin
        };
    }

    public static Plant GetPlant(string plantName)
    {
        Crop crop = crops.Find(crop => crop.name == plantName);
        return new Plant()
        {
            id = crop.id,
            name = crop.name,
            latin = crop.latin,
            variantPotential = crop.variantPotential,
            seedProductionRate = crop.seedProductionRate,
            plantTimeJS = crop.plantTimeJS,
            growthCycleJS = crop.growthCycleJS,
            plantTime = crop.plantTime,
            growthCycle = crop.growthCycle,
            variant = crop.variant,
            requiredCondition = crop.requiredCondition
        };
    }

    public static Plant GetActualGrowthCycle(Plant targetPlant, Dictionary<ConditionType, float> moduleDatas)
    {
        if (targetPlant == null) return null;

        float effect = 0;
        foreach (ConditionType conditionType in moduleDatas.Keys)
        {
            effect += Math.Abs((targetPlant.requiredCondition[conditionType] - moduleDatas[conditionType]) /
                (Module.valueRange[conditionType][1] - Module.valueRange[conditionType][0]) * 2);
        }

        targetPlant.actualGrowthCycle = targetPlant.growthCycle * (1 + Math.Pow(effect * 0.5, 2));
        targetPlant.actualGrowthCycleJS = (long)Math.Floor(targetPlant.growthCycleJS * (1 + Math.Pow(effect * 0.5, 2)));
        return targetPlant;
    }
}
