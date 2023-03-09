using _443eb9Laboratory.Utils;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace _443eb9Laboratory.Database;

public class ClientManager
{
    private static Dictionary<string, string> ipAddreeToUsername;

    [DebuggerStepThrough]
    public static void AddKeyValuePair(string ipAddress, string username)
    {
        if (ipAddreeToUsername == null) ReadData();
        if (!ipAddreeToUsername.ContainsKey(ipAddress) && ipAddreeToUsername.ContainsValue(username))
        {
            foreach (string key in ipAddreeToUsername.Keys)
            {
                if (ipAddreeToUsername[key] == username)
                {
                    ipAddreeToUsername.Remove(key);
                    break;
                }
            }
        }
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

    public async static Task SendMessage(IClientProxy client, string title, string content)
    {
        await client.SendAsync("createMessage", title, content);
    }

    public async static Task SendErrorMessage(IClientProxy client, string title, string content)
    {
        await client.SendAsync("createErrorMessage", title, content);
    }
}
