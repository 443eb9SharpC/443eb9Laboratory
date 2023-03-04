namespace _443eb9Laboratory.DataModels.ETCC;

public class Chunk
{
    public int level;
    public bool isLocked;
    public Crop? cropOn;

    public Chunk(bool isLocked)
    {
        level = 0;
        this.isLocked = isLocked;
        cropOn = null;
    }
}
