using _443eb9Laboratory.DataModels.ETCC;
using _443eb9Laboratory.Utils;

namespace _443eb9Laboratory.Database;

public class ChamberDatabase
{
    public static void SaveChamber(string ipAddress, Chamber chamber)
    {
        IOOperator.ToJson($"./Data/Chambers/{ClientDatabase.GetUsername(ipAddress)}/chamber.json", chamber);
    }

    public static void AddChamber(string chamberName, string chamberDescription, string ipAddress)
    {
        Chamber newChamber = new Chamber
        {
            id = Directory.GetDirectories($"./Data/Chambers").Length,
            level = 0,
            assets = 1000,
            cropsTotalPlanted = 0,
            owner = ClientDatabase.GetUsername(ipAddress),
            name = chamberName,
            description = chamberDescription,
            chamberStorage = new ChamberStorage { seeds = new List<Crop>() },
            chunks = new List<Chunk>()
            {
                new Chunk(),
                new Chunk(),
                new Chunk(),
                new Chunk(){ isLocked = true },
                new Chunk(){ isLocked = true },
                new Chunk(){ isLocked = true },
                new Chunk(){ isLocked = true },
                new Chunk(){ isLocked = true },
                new Chunk(){ isLocked = true }
            },
            modules = new List<Module>()
        };
        IOOperator.ToJson($"./Data/Chambers/{ClientDatabase.GetUsername(ipAddress)}/chamber.json", newChamber);
    }


    public static Chamber GetChamber(string ipAddress)
    {
        return IOOperator.ReadJson<Chamber>($"./Data/Chambers/{ClientDatabase.GetUsername(ipAddress)}/chamber.json");
    }

    public static void AddSeedToChamber(string ipAddress, Crop seed)
    {
        Chamber chamber = GetChamber(ipAddress);
        int index = chamber.chamberStorage.seeds.FindIndex(seedInStorage => seedInStorage.name == seed.name);
        if (index == -1)
        {
            chamber.chamberStorage.seeds.Add(seed);
        }
        else
        {
            chamber.chamberStorage.seeds[index].amount++;
        }
        SaveChamber(ipAddress, chamber);
    }

    public static void RemoveSeedFromChamber(string ipAddress, string seedName)
    {
        Chamber chamber = GetChamber(ipAddress);
        int index = chamber.chamberStorage.seeds.FindIndex(seedInStorage => seedInStorage.name == seedName);
        if (chamber.chamberStorage.seeds[index].amount == 1)
        {
            chamber.chamberStorage.seeds.RemoveAt(index);
        }
        else
        {
            chamber.chamberStorage.seeds[index].amount--;
        }
        SaveChamber(ipAddress, chamber);
    }

    public static bool IsUserOwnedChamber(string ipAddress)
    {
        string username = ClientDatabase.GetUsername(ipAddress);
        return Directory.Exists($"./Data/Chambers/{username}");
    }
}
