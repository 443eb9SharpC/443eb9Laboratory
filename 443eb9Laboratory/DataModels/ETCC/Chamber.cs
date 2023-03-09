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
    public Dictionary<ConditionType, Module> modules;
    public void SaveChamber()
    {
        IOOperator.ToJson($"./Data/UserData/{owner}/Chamber.json", this);
    }

    public void AddSeedToStorage(Crop seed)
    {
        int index = chamberStorage.seeds.FindIndex(seedInStorage => seedInStorage.name == seed.name);
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

    public void RemoveSeedFromStorage(string seedName)
    {
        int index = chamberStorage.seeds.FindIndex(seedInStorage => seedInStorage.name == seedName);
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

    public void AddFruitToStorage(Crop fruit)
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

    public void RemoveFruitFromStorage(string fruitName)
    {
        int index = chamberStorage.fruits.FindIndex(seedInStorage => seedInStorage.name == fruitName);
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

    public void RemoveFruitFromChamber(string fruitName)
    {
        int index = chamberStorage.fruits.FindIndex(seedInStorage => seedInStorage.name == fruitName);
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
            chamberStorage = new ChamberStorage { seeds = new List<Crop>() },
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
            modulesJS = new List<Module>(),
            modules = new Dictionary<ConditionType, Module>(),
        };
        IOOperator.ToJson($"./Data/UserData/{username}/Chamber.json", newChamber);
    }

    public static bool IsUserOwnedChamber(string username)
    {
        return File.Exists($"./Data/UserData/{username}/Chamber.json");
    }
}
