using _443eb9Laboratory.Database;
using _443eb9Laboratory.DataModels.ETCC;
using Microsoft.AspNetCore.SignalR;

namespace _443eb9Laboratory.Utils;

public class ETCC_OperationIndexer
{
    public async static Task ExecuteBuySeed(string ipAddress, IClientProxy client, string seedName)
    {
        Chamber chamber = Chamber.GetChamber(ipAddress);
        Crop seed = CropDatabase.GetCrop(seedName.Substring(0, seedName.Length - 2));
        if (chamber.assets < seed.buyPrice)
        {
            await client.SendAsync("createErrorMessage", "购买失败", $"你的总资产小于{seed.buyPrice}");
        }
        else
        {
            chamber.assets -= seed.buyPrice;
            chamber.AddSeedToStorage(seed);
            chamber.SaveChamber();
            await ETCC_InformationIndexer.SendAssetInfo(ipAddress, client);
            await ETCC_InformationIndexer.SendStorageInfo(ipAddress, client);
            await client.SendAsync("createMessage", "购买成功", $"成功购买了{seedName}");
        }
    }

    public async static Task ExecutePlantSeed(string ipAddress, IClientProxy client, string seedName, string chunkIdStr)
    {
        Chamber chamber = Chamber.GetChamber(ipAddress);
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

        chamber.chunks[chunkId].cropOn = CropDatabase.GetCrop(seedName);
        chamber.chunks[chunkId].cropOn.plantTime = DateTime.Now.ToUniversalTime();

        chamber.chunks[chunkId].cropOn.id = chamber.cropsTotalPlanted;
        chamber.chunks[chunkId].cropOn.plantTimeJS = MathExt.ConvertToJSTime(chamber.chunks[chunkId].cropOn.plantTime);
        chamber.chunks[chunkId].cropOn.growthCycleJS = MathExt.ConvertToJSTime(chamber.chunks[chunkId].cropOn.growthCycle);
        chamber.cropsTotalPlanted++;

        chamber.RemoveSeedFromStorage(seedName);
        await ETCC_InformationIndexer.SendStorageInfo(ipAddress, client);
        await ETCC_InformationIndexer.SendChunksInfo(ipAddress, client);
        await client.SendAsync("createMessage", "种植成功", $"成功将{seedName}种子种植在区块{chunkId}上");
    }

    public async static Task ExecuteHarvest(string ipAddress, IClientProxy client, string chunkIdStr)
    {
        int chunkId = Convert.ToInt32(chunkIdStr);
        Chamber chamber = Chamber.GetChamber(ipAddress);
        Crop targetCrop = chamber.chunks[chunkId].cropOn;
        if (DateTime.Now.ToUniversalTime().Subtract(targetCrop.plantTime).TotalMilliseconds < targetCrop.growthCycle.TotalMilliseconds) return;

        chamber.AddFruitToStorage(chamber.chunks[chunkId].cropOn);
        await client.SendAsync("createMessage", "收获成功", $"成功将种植在区块{chunkId}上的{chamber.chunks[chunkId].cropOn.name}收获");
        chamber.chunks[chunkId].cropOn = null;
        chamber.SaveChamber();
        await ETCC_InformationIndexer.SendChunksInfo(ipAddress, client);
        await ETCC_InformationIndexer.SendStorageInfo(ipAddress, client);
    }

    public async static Task ExecuteSellFruit(string ipAddress, IClientProxy client, string fruitName)
    {
        Chamber chamber = Chamber.GetChamber(ipAddress);
        if (chamber.chamberStorage.fruits.FindIndex(fruit => fruit.name == fruitName) == -1)
        {
            await client.SendAsync("createErrorMessage", "售出失败", $"仓库中不存在{fruitName}");
            return;
        }

        Crop fruit = CropDatabase.GetCrop(fruitName);
        chamber.assets += fruit.sellPrice;
        chamber.RemoveFruitFromChamber(fruitName);
        chamber.SaveChamber();
        await ETCC_InformationIndexer.SendAssetInfo(ipAddress, client);
        await ETCC_InformationIndexer.SendStorageInfo(ipAddress, client);
        await client.SendAsync("createMessage", "售出成功", $"成功将{fruitName}售出");
    }
}
