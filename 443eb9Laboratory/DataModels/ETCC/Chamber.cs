using _443eb9Laboratory.DataModels.ETCC.SubModels;
using _443eb9Laboratory.Utils;

namespace _443eb9Laboratory.DataModels.ETCC;

public class Chamber
{
    public int id;
    public int level;
    public int assets;
    public int cropsTotalPlanted;
    public string owner;
    public string name;
    public string description;
    public ChamberStorage chamberStorage;
    public List<Chunk> chunks;
    public List<Module> modulesJS;
    public List<ConditionType> unlockedModuleTypes;
    public Dictionary<ConditionType, Module> modules;

    public void SaveChamber()
    {
        IOOperator.ToJson($"./Data/UserData/{owner}/Chamber.json", this);
    }

    public void AddSeedToStorage(Seed seed)
    {
        int index;
        if (seed.variant.Count != 0)
        {
            index = chamberStorage.seeds.FindIndex(seedInStorage =>
            {
                int variantEquals = 0;
                foreach (VariantType vt in seed.variant)
                {
                    if (seedInStorage.variant.Contains(vt)) variantEquals++;
                }
                return variantEquals == seed.variant.Count;
            });
        }
        else
        {
            index = chamberStorage.seeds.FindIndex(seedInStorage => seedInStorage.name == seed.name && seedInStorage.variant.Count == 0);
        }

        if (index == -1)
        {
            chamberStorage.seeds.Add(seed);
        }
        else
        {
            chamberStorage.seeds[index].amount++;
        }
        SaveChamber();
    }

    public void RemoveSeedFromStorage(Seed seed)
    {
        if (seed.variant.Count != 0)
        {
            chamberStorage.seeds.Remove(seed);
            return;
        }

        int index = chamberStorage.seeds.FindIndex(seedInStorage => seedInStorage.name == seed.name);
        if (chamberStorage.seeds[index].amount == 1)
        {
            chamberStorage.seeds.RemoveAt(index);
        }
        else
        {
            chamberStorage.seeds[index].amount--;
        }
        SaveChamber();
    }

    public void AddFruitToStorage(Fruit fruit)
    {
        int index = chamberStorage.fruits.FindIndex(fruitInStorage => fruitInStorage.name == fruit.name);
        if (index == -1)
        {
            chamberStorage.fruits.Add(fruit);
        }
        else
        {
            chamberStorage.fruits[index].amount++;
        }

        SaveChamber();
    }

    public void RemoveFruitFromStorage(Fruit fruit)
    {
        int index = chamberStorage.fruits.FindIndex(seedInStorage => seedInStorage.name == fruit.name);
        if (chamberStorage.fruits[index].amount == 1)
        {
            chamberStorage.fruits.RemoveAt(index);
        }
        else
        {
            chamberStorage.fruits[index].amount--;
        }
        SaveChamber();
    }

    public static Chamber GetChamber(string username)
    {
        return IOOperator.ReadJson<Chamber>($"./Data/UserData/{username}/Chamber.json");
    }

    public static void AddChamber(string chamberName, string chamberDescription, string username)
    {
        Chamber newChamber = new Chamber
        {
            id = Directory.GetDirectories($"./Data/UserData").Length,
            level = 0,
            assets = 1000,
            cropsTotalPlanted = 0,
            owner = username,
            name = chamberName,
            description = chamberDescription,
            chamberStorage = new ChamberStorage
            {
                seeds = new List<Seed>(),
                fruits = new List<Fruit>()
            },
            chunks = new List<Chunk>()
            {
                new Chunk(),
                new Chunk(),
                new Chunk(),
                new Chunk() { isLocked = true },
                new Chunk() { isLocked = true },
                new Chunk() { isLocked = true },
                new Chunk() { isLocked = true },
                new Chunk() { isLocked = true },
                new Chunk() { isLocked = true }
            },
            unlockedModuleTypes = new List<ConditionType>(),
            modulesJS = new List<Module>(),
            modules = new Dictionary<ConditionType, Module>()
        };

        newChamber.modules[ConditionType.Temperature] = new Module() { conditionType = ConditionType.Temperature, value = 25 };
        newChamber.modules[ConditionType.Hudimity] = new Module { conditionType = ConditionType.Hudimity, value = 70 };
        newChamber.modules[ConditionType.Illumination] = new Module { conditionType = ConditionType.Illumination, value = 15000 };
        newChamber.modules[ConditionType.CarbonDioxide] = new Module { conditionType = ConditionType.CarbonDioxide, value = 350 }; ;
        newChamber.modules[ConditionType.PH] = new Module { conditionType = ConditionType.PH, value = 7 };

        IOOperator.ToJson($"./Data/UserData/{username}/Chamber.json", newChamber);
    }

    public static bool IsUserOwnedChamber(string username)
    {
        return File.Exists($"./Data/UserData/{username}/Chamber.json");
    }
}
