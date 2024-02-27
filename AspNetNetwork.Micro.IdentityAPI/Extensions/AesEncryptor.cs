using System.Security.Cryptography;
using System.Text;

namespace AspNetNetwork.Micro.IdentityAPI.Extensions;

/// <summary>
/// Represents the aes encryptor class.
/// </summary>
public sealed class AesEncryptor
{
    /// <summary>
    /// Encrypt some information.
    /// </summary>
    /// <param name="key">The string key.</param>
    /// <param name="content">The content.</param>
    /// <returns>Returns encrypt string.</returns>
    public static string Encrypt(string key, string content)
    {
        var keyBytes = new byte[32];
        byte[] array;  
  
        using (var aes = Aes.Create())  
        {  
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32, '\0'));  
            aes.IV = new byte[aes.BlockSize / 8];
  
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            
            using (var memoryStream = new MemoryStream())  
            {  
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))  
                {  
                    using (var streamWriter = new StreamWriter(cryptoStream))  
                    {  
                        streamWriter.Write(content);  
                    }  
                    
                    array = memoryStream.ToArray();  
                }  
            }  
        }  
  
        return Convert.ToBase64String(array);  
    }
    
    /// <summary>
    /// Decrypt some information.
    /// </summary>
    /// <param name="key">The string key.</param>
    /// <param name="encryptedContent">The encrypted content.</param>
    /// <returns>Returns decrypted string.</returns>
    public static string Decrypt(string key, string encryptedContent)
    {
        var iv = new byte[16];  
        var buffer = Convert.FromBase64String(encryptedContent);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);  
        aes.IV = iv;  
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        return streamReader.ReadToEnd();
    }
}