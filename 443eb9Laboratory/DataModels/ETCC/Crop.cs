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
    public float seedProductionRate;
    public long plantTimeJS;
    public long growthCycleJS;
    public long actualGrowthCycleJS;
    public DateTime plantTime;
    public TimeSpan growthCycle;
    public TimeSpan actualGrowthCycle;
    public List<VariantType> variant;
    public Dictionary<ConditionType, float> requiredCondition;

    public static Random random = new Random();
    public static List<Crop> crops = IOOperator.ReadJson<List<Crop>>("./Data/Crops.json");

    public static Seed GetSeed(string seedName)
    {
        Crop crop = crops.Find(crop => crop.name == seedName);
        return new Seed()
        {
            amount = 1,
            buyPrice = crop.buyPrice,
            name = crop.name,
            latin = crop.latin,
            variant = crop.variant
        };
    }

    public static Fruit GetFruit(string fruitName)
    {
        Crop crop = crops.Find(crop => crop.name == fruitName);
        return new Fruit()
        {
            amount = 1,
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

    public static VariantType GenerateVariant(List<VariantType> variantType)
    {
        VariantType type;
        int typeInt;
        do
        {
            type = (VariantType)random.Next(0, 10);
            typeInt = (int)type;

            if (variantType.Count == 0) break;
            if (typeInt % 2 == 0 && variantType.Contains(type + 1)) continue;
            if (typeInt % 2 == 1 && variantType.Contains(type - 1)) continue;
        }
        while (variantType.Contains(type) && variantType.Count != 0);
        return type;
    }

    public static void ApplyVariant(ref Plant plant)
    {
        ConditionType conditionType;

        foreach (VariantType variantType in plant.variant)
        {
            int vtInt = (int)variantType;
            if (vtInt == 0 || vtInt == 1) conditionType = ConditionType.Temperature;
            else if (vtInt == 2 || vtInt == 3) conditionType = ConditionType.Hudimity;
            else if (vtInt == 4 || vtInt == 5) conditionType = ConditionType.Illumination;
            else if (vtInt == 6 || vtInt == 7) conditionType = ConditionType.CarbonDioxide;
            else if (vtInt == 8 || vtInt == 9) conditionType = ConditionType.PH;
            else return;

            if ((int)conditionType % 2 == 0)
            {
                plant.requiredCondition[conditionType] +=
                    (Module.valueRange[conditionType][1] - plant.requiredCondition[conditionType]) * random.Next(5, 20) / 100;
            }
            else
            {
                plant.requiredCondition[conditionType] -=
                    (plant.requiredCondition[conditionType] - Module.valueRange[conditionType][0]) * random.Next(5, 20) / 100;
            }
        }
    }
}
