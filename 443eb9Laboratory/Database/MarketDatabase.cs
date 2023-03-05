using _443eb9Laboratory.DataModels.ETCC;
using _443eb9Laboratory.Utils;

namespace _443eb9Laboratory.Database;

public class MarketDatabase
{
    public static List<Crop> fruits = IOOperator.ReadJson<List<Crop>>("./Data/Market/Fruit.json");

    public static Crop GetFruit(string fruitName)
    {
        return fruits.Find(fruit => fruit.name == fruitName);
    }
}
