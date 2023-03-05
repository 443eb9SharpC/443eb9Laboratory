using _443eb9Laboratory.Database;
using _443eb9Laboratory.DataModels.ETCC;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace _443eb9Laboratory.Utils;

public class ETCC_InformationIndexer
{
    public async static Task SendDashBoardInfo(string ipAddress, IClientProxy client)
    {
        Chamber chamber = ChamberDatabase.GetChamber(ipAddress);
        await client.SendAsync("getDashBoardInfo", JsonConvert.SerializeObject(chamber));
    }

    public async static Task SendChunksInfo(string ipAddress, IClientProxy client)
    {
        List<Chunk> chunks = ChamberDatabase.GetChamber(ipAddress).chunks;
        await client.SendAsync("getChunksInfo", JsonConvert.SerializeObject(chunks));
    }

    public async static Task SendAssetInfo(string ipAddress, IClientProxy client)
    {
        await client.SendAsync("getAssetInfo", ChamberDatabase.GetChamber(ipAddress).assets);
    }

    public async static Task SendSeedStoreInfo(IClientProxy client)
    {
        List<Crop> seedStore = IOOperator.ReadJson<List<Crop>>("./Data/Store/Seed.json");
        await client.SendAsync("getSeedStoreInfo", JsonConvert.SerializeObject(seedStore));
    }

    public async static Task SendStorageInfo(string ipAddress, IClientProxy client)
    {
        Chamber chamber = ChamberDatabase.GetChamber(ipAddress);
        await client.SendAsync("getStorageInfo", JsonConvert.SerializeObject(chamber.chamberStorage));
    }
}