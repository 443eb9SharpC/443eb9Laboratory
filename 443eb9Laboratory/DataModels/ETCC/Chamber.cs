namespace _443eb9Laboratory.DataModels.ETCC;

public class Chamber
{
    public int id;
    public int level;
    public int assets;
    public int cropsTotalPlanted;
    public string owner;
    public string name;
    public string description;
    public ChamberStorage chamberStorage;
    public List<Chunk> chunks;
    public List<Module> modules;
}
