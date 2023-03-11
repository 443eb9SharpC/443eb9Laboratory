using _443eb9Laboratory.Database;
using _443eb9Laboratory.DataModels.ETCC;
using _443eb9Laboratory.DataModels.ETCC.SubModels;
using Microsoft.AspNetCore.SignalR;

namespace _443eb9Laboratory.Utils;

public class ETCC_OperExecuter
{
    public static Random random = new Random();

    public async static Task ExecuteBuySeed(string username, IClientProxy client, string seedName)
    {
        Chamber chamber = Chamber.GetChamber(username);
        Seed seed = Crop.GetSeed(seedName.Substring(0, seedName.Length - 2));
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

    public async static Task ExecutePlantSeed(string username, IClientProxy client, string seedIdStr, string chunkIdStr)
    {
        Chamber chamber = Chamber.GetChamber(username);
        int chunkId = Convert.ToInt32(chunkIdStr);
        int seedId = Convert.ToInt32(seedIdStr);

        if (seedId > chamber.chamberStorage.seeds.Count - 1 || seedId < 0) return;
        Seed targetSeed = chamber.chamberStorage.seeds[seedId];
        chamber.RemoveSeedFromStorage(targetSeed);

        if (chamber.chunks[chunkId].plantOn != null)
        {
            await ClientManager.SendErrorMessage(client, "种植失败", $"区块{chunkId}上已存在作物");
            return;
        }
        if (chamber.chunks[chunkId].isLocked)
        {
            await ClientManager.SendErrorMessage(client, "种植失败", $"区块{chunkId}未解锁");
            return;
        }

        chamber.chunks[chunkId].plantOn = Crop.GetPlant(targetSeed.name);
        chamber.chunks[chunkId].plantOn.plantTime = DateTime.Now.ToUniversalTime();

        chamber.chunks[chunkId].plantOn.id = chamber.cropsTotalPlanted;

        Dictionary<ConditionType, float> moduleData = new Dictionary<ConditionType, float>();
        foreach (ConditionType types in chamber.modules.Keys)
        {
            moduleData[types] = chamber.modules[types].value;
        }

        chamber.chunks[chunkId].plantOn.plantTimeJS = MathExt.ConvertToJSTime(chamber.chunks[chunkId].plantOn.plantTime);
        chamber.chunks[chunkId].plantOn.growthCycleJS = MathExt.ConvertToJSTime(chamber.chunks[chunkId].plantOn.growthCycle);

        chamber.chunks[chunkId].plantOn.seedProductionRate = random.Next(0, 50);
        chamber.chunks[chunkId].plantOn.variant = new List<VariantType>(targetSeed.variant);
        if (random.Next(0, 100) < 10)
        {
            chamber.chunks[chunkId].plantOn.variant.Add(Crop.GenerateVariant(chamber.chunks[chunkId].plantOn.variant));
        }
        Crop.ApplyVariant(ref chamber.chunks[chunkId].plantOn);

        chamber.chunks[chunkId].plantOn = Crop.GetActualGrowthCycle(chamber.chunks[chunkId].plantOn, moduleData);
        chamber.chunks[chunkId].plantOn.actualGrowthCycleJS = MathExt.ConvertToJSTime(chamber.chunks[chunkId].plantOn.actualGrowthCycle);
        chamber.cropsTotalPlanted++;

        chamber.SaveChamber();
        await ETCC_InfoSender.SendStorageInfo(username, client);
        await ETCC_InfoSender.SendChunksInfo(username, client);
        await ClientManager.SendMessage(client, "种植成功", $"成功将{targetSeed.name}种子种植在区块{chunkId}上");
    }

    public async static Task ExecuteHarvest(string username, IClientProxy client, string chunkIdStr)
    {
        int chunkId = Convert.ToInt32(chunkIdStr);
        Chamber chamber = Chamber.GetChamber(username);
        Plant targetPlant = chamber.chunks[chunkId].plantOn;
        if (DateTime.Now.ToUniversalTime().Subtract(targetPlant.plantTime).TotalMilliseconds < targetPlant.actualGrowthCycle.TotalMilliseconds) return;

        if (random.Next(0, 100) < 100)
        {
            Seed targetSeed = Crop.GetSeed(targetPlant.name);
            targetSeed.amount = 1;
            targetSeed.variant = new List<VariantType>(targetPlant.variant);
            chamber.chamberStorage.seeds.Add(targetSeed);
            await ClientManager.SendMessage(client, "额外产物", "这株作物产出了一个单位的种子");
        }

        chamber.AddFruitToStorage(Crop.GetFruit(chamber.chunks[chunkId].plantOn.name));
        await ClientManager.SendMessage(client, "收获成功", $"成功将种植在区块{chunkId}上的{chamber.chunks[chunkId].plantOn.name}收获");
        chamber.chunks[chunkId].plantOn = null;
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

        Fruit fruit = Crop.GetFruit(fruitName);
        chamber.assets += fruit.sellPrice;
        chamber.RemoveFruitFromStorage(fruit);
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

        if (chamber.unlockedModuleTypes.Contains((ConditionType)moduleId))
        {
            await ClientManager.SendErrorMessage(client, "购买失败", $"模块已解锁");
            return;
        }

        Module module = new Module()
        {
            value = -1,
            conditionType = (ConditionType)moduleId,
        };

        chamber.unlockedModuleTypes.Add(module.conditionType);
        chamber.assets -= 5000;
        chamber.modules[module.conditionType] = module;
        chamber.SaveChamber();
        await ClientManager.SendMessage(client, "购买成功", $"已成功解锁模块");
        await ETCC_InfoSender.SendDashBoardInfo(username, client);
        await ETCC_InfoSender.SendAssetInfo(username, client);
    }

    public async static Task ExecuteChangeModuleData(string username, IClientProxy client, string moduleDataStr, string moduleTypeStr)
    {
        Chamber chamber = Chamber.GetChamber(username);
        ConditionType moduleType = (ConditionType)Convert.ToInt32(moduleTypeStr);
        float maxValue = Module.valueRange[moduleType][1];
        float minValue = Module.valueRange[moduleType][0];

        if (!float.TryParse(moduleDataStr, out float moduleData)) return;
        if (moduleData > maxValue || moduleData < minValue) return;
        if (!chamber.unlockedModuleTypes.Contains(moduleType)) return;

        chamber.modules[(ConditionType)Convert.ToInt32(moduleType)].value = moduleData;

        Dictionary<ConditionType, float> moduleDatas = new Dictionary<ConditionType, float>();
        foreach (ConditionType conditionType in chamber.modules.Keys)
        {
            moduleDatas[conditionType] = chamber.modules[conditionType].value;
        }

        for (int i = 0; i < chamber.chunks.Count; i++)
        {
            chamber.chunks[i].plantOn = Crop.GetActualGrowthCycle(chamber.chunks[i].plantOn, moduleDatas);
        }

        chamber.SaveChamber();
        await ETCC_InfoSender.SendChunksInfo(username, client);
        await ClientManager.SendMessage(client, "修改成功", $"已成功将数据改为{moduleData}");
    }
}
