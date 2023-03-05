using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace _443eb9Laboratory.Utils;

public class MathExt
{
    public static Random random = new Random();

    [DebuggerStepThrough]
    public static void Shuffle<T>(List<T> list)
    {
        int index;
        T temp;
        for (int i = 0; i < list.Count; i++)
        {
            index = random.Next(0, list.Count - 1);
            if (index != i)
            {
                temp = list[i];
                list[i] = list[index];
                list[index] = temp;
            }
        }
    }

    [DebuggerStepThrough]
    public static string MD5Encrypt64(string password)
    {
        string cl = password;
        //string pwd = "";
        MD5 md5 = MD5.Create(); //实例化一个md5对像
                                // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        return Convert.ToBase64String(s);
    }

    [DebuggerStepThrough]
    public static long ConvertToJSTime(DateTime dateTime)
    {
        return (long)dateTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

    [DebuggerStepThrough]
    public static long ConvertToJSTime(TimeSpan timeSpan)
    {
        return (long)timeSpan.TotalMilliseconds;
    }
}
