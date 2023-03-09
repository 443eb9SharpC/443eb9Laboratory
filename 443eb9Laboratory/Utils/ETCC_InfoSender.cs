using _443eb9Laboratory.Database;
using _443eb9Laboratory.DataModels.ETCC;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace _443eb9Laboratory.Utils;

public class ETCC_InfoSender
{
    public async static Task SendDashBoardInfo(string username, IClientProxy client)
    {
        Chamber chamber = Chamber.GetChamber(username);

        foreach (ConditionType conditionType in chamber.modules.Keys)
        {
            chamber.modulesJS.Add(new Module
            {
                conditionType = conditionType,
                value = chamber.modules[conditionType].value
            });
        }

        chamber.chunks = null;
        chamber.chamberStorage = null;
        chamber.modules = null;
        await client.SendAsync("getDashBoardInfo", JsonConvert.SerializeObject(chamber));
    }

    public async static Task SendChunksInfo(string username, IClientProxy client)
    {
        List<Chunk> chunks = Chamber.GetChamber(username).chunks;
        await client.SendAsync("getChunksInfo", JsonConvert.SerializeObject(chunks));
    }

    public async static Task SendAssetInfo(string username, IClientProxy client)
    {
        await client.SendAsync("getAssetInfo", Chamber.GetChamber(username).assets);
    }

    public async static Task SendSeedStoreInfo(IClientProxy client)
    {
        await client.SendAsync("getSeedStoreInfo", JsonConvert.SerializeObject(CropDatabase.crops));
    }

    public async static Task SendStorageInfo(string username, IClientProxy client)
    {
        Chamber chamber = Chamber.GetChamber(username);
        await client.SendAsync("getStorageInfo", JsonConvert.SerializeObject(chamber.chamberStorage));
    }

    public async static Task SendMarketInfo(IClientProxy client)
    {
        await client.SendAsync("getFruitMarketInfo", JsonConvert.SerializeObject(CropDatabase.crops));
    }
}