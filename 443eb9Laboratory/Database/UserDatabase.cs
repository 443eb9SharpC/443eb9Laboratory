using _443eb9Laboratory.DataModels;
using _443eb9Laboratory.Utils;

namespace _443eb9Laboratory.Database;

public class UserDatabase
{
    private static List<User> database;

    public static void Init() =>
        database = IOOperator.ReadJson<List<User>>("Data/UserDatabase.json");

    public static void SaveDatabase() =>
        IOOperator.ToJson("Data/UserDatabase.json", database);

    public static void AddUser(User user) =>
        database.Add(user);

    public static void AddUser(string username, string password, string email) =>
        database.Add(new User(username, password, email));

    public static bool HasUser(string username)
    {
        return database.Find(user => user.username == username) != null;
    }

    public static User GetUser(string username)
    {
        return database.Find(user => user.username == username);
    }
}
