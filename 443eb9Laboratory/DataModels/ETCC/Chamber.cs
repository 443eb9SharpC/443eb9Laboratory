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
    public FarmStorage farmStorage;
    public List<Chunk> chunks;
    public List<Module> modules;

    public Chamber(int id, string owner, string name, string description)
    {
        this.id = id;
        this.owner = owner;
        this.name = name;
        this.description = description;
        level = 0;
        assets = 1000;
        cropsTotalPlanted = 0;
        farmStorage = new FarmStorage();
        chunks = new List<Chunk>()
        {
            new Chunk(false),
            new Chunk(false),
            new Chunk(false),
            new Chunk(true),
            new Chunk(true),
            new Chunk(true),
            new Chunk(true),
            new Chunk(true),
            new Chunk(true)
        };
        modules = new List<Module>(5);
    }
}
