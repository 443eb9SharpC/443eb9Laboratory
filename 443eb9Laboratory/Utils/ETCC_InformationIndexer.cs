using _443eb9Laboratory.Database;
using _443eb9Laboratory.DataModels.ETCC;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace _443eb9Laboratory.Utils;

public class ETCC_InformationIndexer
{
    public async static void SendDashBoardInfo(string connectionId, string ipAddress, IClientProxy client)
    {
        Chamber chamber = ChamberDatabase.GetChamber(ClientDatabase.GetUsername(ipAddress));
        await client.SendAsync("getDashBoardInfo", JsonConvert.SerializeObject(chamber));
    }

    public async static void SendChunksInfo(string connectionId, string ipAddress, IClientProxy client)
    {
        List<Chunk> chunks = ChamberDatabase.GetChamber(ClientDatabase.GetUsername(ipAddress)).chunks;
        await client.SendAsync("getChunksInfo", JsonConvert.SerializeObject(chunks));
    }
}