using System.Security.Cryptography;
using System.Text;

namespace TodoApi.Utils;
public class EncryptUtils
{
    public static string Encrypt(string strpwd)
    {
        string str = "";
        MD5 mD5 = MD5.Create();
        byte[] bytes = Encoding.Default.GetBytes(strpwd);
        byte[] md5Str = mD5.ComputeHash(bytes);

        mD5.Clear();

        for (int i = 0; i < md5Str.Length - 1; i++)
        {
            str += md5Str[i].ToString("x").PadLeft(2, '0');
        }
        return str;
    }
}