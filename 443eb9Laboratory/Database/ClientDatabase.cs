namespace _443eb9Laboratory.Database;

public class ClientDatabase
{
    private static Dictionary<string, string> ipAddreeToUsername = new Dictionary<string, string>();

    public static void AddKeyValuePair(string ipAddress, string username) =>
        ipAddreeToUsername[ipAddress] = username;

    public static void RemoveKeyValuePair(string ipAddress) =>
        ipAddreeToUsername.Remove(ipAddress);

    public static string GetUsername(string ipAddress)
    {
        return ipAddreeToUsername[ipAddress];
    }
}
