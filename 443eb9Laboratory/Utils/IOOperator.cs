using Newtonsoft.Json;
using System.Diagnostics;

namespace _443eb9Laboratory.Utils;

public class IOOperator
{
    [DebuggerStepThrough]
    public static T ReadJson<T>(string path)
    {
        string jsonData = File.ReadAllText(path);
        T? obj = JsonConvert.DeserializeObject<T>(jsonData);
        if (obj == null)
        {
            throw new Exception("Empty Json File.");
        }
        return obj;
    }

    [DebuggerStepThrough]
    public static byte[] ReadImage(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader binaryReader = new BinaryReader(fileStream);
        byte[] image = binaryReader.ReadBytes((int)fileStream.Length);
        fileStream.Close();
        return image;
    }

    [DebuggerStepThrough]
    public static void ToJson(string path, object? obj)
    {
        string jsonData = JsonConvert.SerializeObject(obj, Formatting.Indented);

        string[] paths = path.Split('/');
        string parent = "";
        for (int i = 0; i < paths.Length - 1; i++)
        {
            parent += paths[i] + '/';
        }

        if (!Directory.Exists(parent))
        {
            Directory.CreateDirectory(parent);
        }
        StreamWriter streamWriter = File.CreateText(path);
        streamWriter.Write(jsonData);
        streamWriter.Close();
    }
}
