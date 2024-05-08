using Cryptography.Services.Interfaces;
using Cryptography.Services.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

public class CryptographyService : ICryptographyService
{
    private readonly string _encryptionKey;
    private readonly string _initializationVector; 

    public CryptographyService(IOptions<CryptographySettings> cryptographySettings)
    {
        _encryptionKey = cryptographySettings.Value.EncryptionKey;
        _initializationVector = cryptographySettings.Value.InitializationVector;
    }
    public string Decrypt(string cipherText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(_encryptionKey);
            aesAlg.IV = Encoding.UTF8.GetBytes(_initializationVector);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    public string Encrypt(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(_encryptionKey);
            aesAlg.IV = Encoding.UTF8.GetBytes(_initializationVector);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }
}
