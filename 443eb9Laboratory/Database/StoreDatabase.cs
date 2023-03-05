using _443eb9Laboratory.DataModels.ETCC;
using _443eb9Laboratory.Utils;

namespace _443eb9Laboratory.Database;

public class StoreDatabase
{
    public static List<Crop> seeds = IOOperator.ReadJson<List<Crop>>("./Data/Store/Seed.json");

    public static Crop GetSeed(string seedName)
    {
        return seeds.Find(seed => seed.name == seedName);
    }
}
