using _443eb9Laboratory.Database;
using _443eb9Laboratory.DataModels.ETCC;
using Microsoft.AspNetCore.SignalR;

namespace _443eb9Laboratory.Utils;

public class ETCC_OperExecuter
{
    public async static Task ExecuteBuySeed(string username, IClientProxy client, string seedName)
    {
        Chamber chamber = Chamber.GetChamber(username);
        Crop seed = CropDatabase.GetCrop(seedName.Substring(0, seedName.Length - 2));
        if (seed == null) return;

        if (chamber.assets < seed.buyPrice)
        {
            await ClientManager.SendErrorMessage(client, "购买失败", $"你的总资产小于{seed.buyPrice}");
        }
        else
        {
            seed.amount = 1;
            chamber.assets -= seed.buyPrice;
            chamber.AddSeedToStorage(seed);
            chamber.SaveChamber();
            await ETCC_InfoSender.SendAssetInfo(username, client);
            await ETCC_InfoSender.SendStorageInfo(username, client);
            await ClientManager.SendMessage(client, "购买成功", $"成功购买了{seedName}");
        }
    }

    public async static Task ExecutePlantSeed(string username, IClientProxy client, string seedName, string chunkIdStr)
    {
        Chamber chamber = Chamber.GetChamber(username);
        int chunkId = Convert.ToInt32(chunkIdStr);

        if (chamber.chunks[chunkId].cropOn != null)
        {
            await ClientManager.SendErrorMessage(client, "种植失败", $"区块{chunkId}上已存在作物");
            return;
        }
        if (chamber.chunks[chunkId].isLocked)
        {
            await ClientManager.SendErrorMessage(client, "种植失败", $"区块{chunkId}未解锁");
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
        await ETCC_InfoSender.SendStorageInfo(username, client);
        await ETCC_InfoSender.SendChunksInfo(username, client);
        await ClientManager.SendMessage(client, "种植成功", $"成功将{seedName}种子种植在区块{chunkId}上");
    }

    public async static Task ExecuteHarvest(string username, IClientProxy client, string chunkIdStr)
    {
        int chunkId = Convert.ToInt32(chunkIdStr);
        Chamber chamber = Chamber.GetChamber(username);
        Crop targetCrop = chamber.chunks[chunkId].cropOn;
        if (DateTime.Now.ToUniversalTime().Subtract(targetCrop.plantTime).TotalMilliseconds < targetCrop.growthCycle.TotalMilliseconds) return;

        chamber.AddFruitToStorage(chamber.chunks[chunkId].cropOn);
        await ClientManager.SendMessage(client, "收获成功", $"成功将种植在区块{chunkId}上的{chamber.chunks[chunkId].cropOn.name}收获");
        chamber.chunks[chunkId].cropOn = null;
        chamber.SaveChamber();
        await ETCC_InfoSender.SendChunksInfo(username, client);
        await ETCC_InfoSender.SendStorageInfo(username, client);
    }

    public async static Task ExecuteSellFruit(string username, IClientProxy client, string fruitName)
    {
        Chamber chamber = Chamber.GetChamber(username);
        if (chamber.chamberStorage.fruits.FindIndex(fruit => fruit.name == fruitName) == -1)
        {
            await ClientManager.SendErrorMessage(client, "售出失败", $"仓库中不存在{fruitName}");
            return;
        }

        Crop fruit = CropDatabase.GetCrop(fruitName);
        chamber.assets += fruit.sellPrice;
        chamber.RemoveFruitFromChamber(fruitName);
        chamber.SaveChamber();
        await ETCC_InfoSender.SendAssetInfo(username, client);
        await ETCC_InfoSender.SendStorageInfo(username, client);
        await ClientManager.SendMessage(client, "售出成功", $"成功将{fruitName}售出");
    }

    public async static Task ExecuteBuyChunk(string username, IClientProxy client, string chunkIdStr)
    {
        int chunkId = Convert.ToInt32(chunkIdStr);
        if (chunkId < 3 || chunkId > 8) return;

        Chamber chamber = Chamber.GetChamber(username);
        int chunkPrice = 4000 + (chunkId - 2) * 2000;
        if (chamber.assets < chunkPrice)
        {
            await ClientManager.SendErrorMessage(client, "购买失败", $"你的总资产小于{chunkPrice}");
            return;
        }
        if (!chamber.chunks[chunkId].isLocked)
        {
            await ClientManager.SendErrorMessage(client, "购买失败", $"区块{chunkId}已解锁");
            return;
        }

        chamber.assets -= chunkPrice;
        chamber.chunks[chunkId].isLocked = false;
        chamber.SaveChamber();
        await ETCC_InfoSender.SendChunksInfo(username, client);
        await ETCC_InfoSender.SendAssetInfo(username, client);
        await client.SendAsync("createMessage", "购买成功", $"已成功解锁区块{chunkId}");
    }

    public static async Task ExecuteBuyModule(string username, IClientProxy client, string moduleIdStr)
    {
        int moduleId = Convert.ToInt32(moduleIdStr);
        if (moduleId < 0 || moduleId > 4) return;

        Chamber chamber = Chamber.GetChamber(username);
        if (chamber.assets < 5000)
        {
            await ClientManager.SendErrorMessage(client, "购买失败", $"你的总资产小于5000");
            return;
        }

        if (chamber.modules.ContainsKey((ConditionType)moduleId))
        {
            await ClientManager.SendErrorMessage(client, "购买失败", $"模块已解锁");
            return;
        }

        Module module = new Module()
        {
            value = -1,
            conditionType = (ConditionType)moduleId,
        };

        chamber.modules[module.conditionType] = module;
        chamber.SaveChamber();
        await ClientManager.SendMessage(client, "购买成功", $"已成功解锁模块");
        await ETCC_InfoSender.SendDashBoardInfo(username, client);
    }

    public async static Task ExecuteChangeModuleData(string username, IClientProxy client, string moduleDataStr, string moduleTypeStr)
    {
        ConditionType moduleType = (ConditionType)Convert.ToInt32(moduleTypeStr);
        float maxValue = CropDatabase.GetDataRangeInfo(moduleType)[0];
        float minValue = CropDatabase.GetDataRangeInfo(moduleType)[1];

        if (!float.TryParse(moduleDataStr, out float moduleData)) return;
        if (moduleData > maxValue || moduleData < minValue) return;

        Chamber chamber = Chamber.GetChamber(username);
        chamber.modules[(ConditionType)Convert.ToInt32(moduleType)].value = moduleData;

        Dictionary<ConditionType, float> moduleDatas = new Dictionary<ConditionType, float>();
        foreach (ConditionType conditionType in chamber.modules.Keys)
        {
            moduleDatas[conditionType] = chamber.modules[conditionType].value;
        }

        for (int i = 0; i < chamber.chunks.Count; i++)
        {
            chamber.chunks[i].cropOn = CropDatabase.GetActualCropGrowthCycle(chamber.chunks[i].cropOn, moduleDatas);
        }

        chamber.SaveChamber();
        await ETCC_InfoSender.SendChunksInfo(username, client);
        await ClientManager.SendMessage(client, "修改成功", $"已成功将数据改为{moduleData}");
    }
}
