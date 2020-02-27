using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
	
public class Program
{
	 private static byte[] Generate256BitsOfRandomEntropy()
        {
            byte[] array = new byte[32];
            using (RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngcryptoServiceProvider.GetBytes(array);
            }
            return array;
        }
	public static void Main()
	{
		    byte[] array = Generate256BitsOfRandomEntropy();
            byte[] array2 = Generate256BitsOfRandomEntropy();
            byte[] bytes = Encoding.UTF8.GetBytes("{\"Us\":ID_OF_YOUR_BITMEX_ACCOUNT,\"M\":0}");
            string result;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("469B9413227measu5541DAE9C7ring", array, 1000))
            {
                byte[] bytes2 = rfc2898DeriveBytes.GetBytes(32);
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.BlockSize = 256;
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(bytes2, array2))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(bytes, 0, bytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] inArray = array.Concat(array2).ToArray<byte>().Concat(memoryStream.ToArray()).ToArray<byte>();
                                memoryStream.Close();
                                cryptoStream.Close();
                                result = Convert.ToBase64String(inArray);
								Console.WriteLine(result);
                            }
                        }
                    }
                }
            }
	}
}
