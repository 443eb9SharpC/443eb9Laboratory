using _443eb9Laboratory.Utils;
using System.Diagnostics;

namespace _443eb9Laboratory.Database;

public class ClientDatabase
{
    private static Dictionary<string, string> ipAddreeToUsername;

    [DebuggerStepThrough]
    public static void AddKeyValuePair(string ipAddress, string username)
    {
        if (ipAddreeToUsername == null) ReadData();
        ipAddreeToUsername[ipAddress] = username;
        SaveData();
    }

    [DebuggerStepThrough]
    public static void RemoveKeyValuePair(string ipAddress)
    {
        if (ipAddreeToUsername == null) ReadData();
        ipAddreeToUsername.Remove(ipAddress);
        SaveData();
    }

    [DebuggerStepThrough]
    public static void ReadData() =>
        ipAddreeToUsername = IOOperator.ReadJson<Dictionary<string, string>>("./Data/ClientDatabase.json");

    public static void SaveData() =>
        IOOperator.ToJson("./Data/ClientDatabase.json", ipAddreeToUsername);


    [DebuggerStepThrough]
    public static string GetUsername(string ipAddress)
    {
        if (ipAddreeToUsername == null) ReadData();
        return ipAddreeToUsername[ipAddress];
    }
}
