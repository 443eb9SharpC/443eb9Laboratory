using _443eb9Laboratory.DataModels.ETCC;
using _443eb9Laboratory.Utils;

namespace _443eb9Laboratory.Database;

public class CropDatabase
{
    public static List<Crop> crops = IOOperator.ReadJson<List<Crop>>("./Data/Crops.json");

    public static Crop GetCrop(string fruitName)
    {
        return crops.Find(fruit => fruit.name == fruitName);
    }
}
