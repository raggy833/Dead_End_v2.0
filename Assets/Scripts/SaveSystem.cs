using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public static class SaveSystem
{
    private static byte[] key = new byte[32] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20 };
    private static byte[] iv = new byte[16] { 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x30 };

    public static void SaveData(int silverAmount, int goldAmount)
    {
        string data = silverAmount.ToString() + "," + goldAmount.ToString();
        byte[] encryptedData = EncryptStringToBytes_Aes(data, key, iv);
        string encryptedString = Convert.ToBase64String(encryptedData);
        PlayerPrefs.SetString("GameData", encryptedString);
        PlayerPrefs.Save();
    }

    public static (int, int) LoadData()
    {
        if (PlayerPrefs.HasKey("GameData"))
        {
            string encryptedString = PlayerPrefs.GetString("GameData");
            byte[] encryptedData = Convert.FromBase64String(encryptedString);
            string decryptedString = DecryptStringFromBytes_Aes(encryptedData, key, iv);
            string[] data = decryptedString.Split(',');
            int silverAmount = int.Parse(data[0]);
            int goldAmount = int.Parse(data[1]);
            return (silverAmount, goldAmount);
        }
        else
        {
            return (0, 0);
        }
    }

    static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
    {
        byte[] encrypted;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        return encrypted;
    }

    static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        string plaintext = null;
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }
}
