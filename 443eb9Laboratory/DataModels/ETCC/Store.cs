using _443eb9Laboratory.Utils;

namespace _443eb9Laboratory.DataModels.ETCC;

public class Store
{
    public static List<Crop> seeds = IOOperator.ReadJson<List<Crop>>("./Data/Store/Seed.json");

    public static Crop GetSeed(string seedName)
    {
        return seeds.Find(seed => seed.name == seedName);
    }
}
