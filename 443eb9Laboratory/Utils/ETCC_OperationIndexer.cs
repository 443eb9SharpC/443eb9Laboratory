using _443eb9Laboratory.Database;
using _443eb9Laboratory.DataModels.ETCC;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace _443eb9Laboratory.Utils;

public class ETCC_OperationIndexer
{
    public async static Task ExecuteBuySeed(string ipAddress, IClientProxy client, string seedName)
    {
        Chamber chamber = ChamberDatabase.GetChamber(ipAddress);
        Crop seed = Store.GetSeed(seedName);
        if (chamber.assets < seed.buyPrice)
        {
            await client.SendAsync("createErrorMessage", "购买失败", $"你的总资产小于{seed.buyPrice}");
        }
        else
        {
            chamber.assets -= seed.buyPrice;
            ChamberDatabase.SaveChamber(ipAddress, chamber);
            ChamberDatabase.AddSeedToChamber(ipAddress, seed);
            await client.SendAsync("getAssetInfo", chamber.assets);
            await ETCC_InformationIndexer.SendStorageInfo(ipAddress, client);
            await client.SendAsync("createMessage", "购买成功", $"成功购买了{seedName}");
        }
    }

    public async static Task ExecutePlantSeed(string ipAddress, IClientProxy client, string seedName, string chunkIdStr)
    {
        Chamber chamber = ChamberDatabase.GetChamber(ipAddress);
        int chunkId = Convert.ToInt32(chunkIdStr);

        if (chamber.chunks[chunkId].cropOn != null)
        {
            await client.SendAsync("createErrorMessage", "种植失败", $"区块{chunkId}上已存在作物");
            return;
        }
        if (chamber.chunks[chunkId].isLocked)
        {
            await client.SendAsync("createErrorMessage", "种植失败", $"区块{chunkId}未解锁");
            return;
        }

        seedName = seedName.Substring(0, seedName.Length - 2);

        chamber.chunks[chunkId].cropOn = Store.GetSeed(seedName);
        chamber.chunks[chunkId].cropOn.plantTime = DateTime.Now.ToUniversalTime();

        chamber.chunks[chunkId].cropOn.id = chamber.cropsTotalPlanted;
        chamber.chunks[chunkId].cropOn.plantTimeJS = MathExt.ConvertToJSTime(chamber.chunks[chunkId].cropOn.plantTime);
        chamber.chunks[chunkId].cropOn.growthCycleJS = MathExt.ConvertToJSTime(chamber.chunks[chunkId].cropOn.growthCycle);
        chamber.cropsTotalPlanted++;

        ChamberDatabase.SaveChamber(ipAddress, chamber);
        ChamberDatabase.RemoveSeedFromChamber(ipAddress, seedName);
        await ETCC_InformationIndexer.SendStorageInfo(ipAddress, client);
        await ETCC_InformationIndexer.SendChunksInfo(ipAddress, client);
    }
}
