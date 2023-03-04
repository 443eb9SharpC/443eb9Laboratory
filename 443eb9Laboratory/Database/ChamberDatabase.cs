using _443eb9Laboratory.DataModels;
using _443eb9Laboratory.DataModels.ETCC;
using _443eb9Laboratory.Utils;

namespace _443eb9Laboratory.Database;

public class ChamberDatabase
{
    private static List<Chamber> database;

    public static void Init()
    {
        Console.WriteLine($"[{DateTime.Now}] ChamberDatabase Init");
        database = IOOperator.ReadJson<List<Chamber>>("Data/ChamberDatabase.json");
    }

    public static void SaveDatabase()
    {
        IOOperator.ToJson("Data/ChamberDatabase.json", database);
    }

    public static void AddChamber(string chamberName, string chamberDescription, string ipAddress)
    {
        database.Add(new Chamber(database.Count, ClientDatabase.GetUsername(ipAddress), chamberName, chamberDescription));
        SaveDatabase();
    }

    public static Chamber GetChamber(string username)
    {
        return database.Find(chamber => chamber.owner == username);
    }

    public static bool IsUserOwnedChamber(string username)
    {
        return database.Find(chamber => chamber.owner == username) != null;
    }
}
