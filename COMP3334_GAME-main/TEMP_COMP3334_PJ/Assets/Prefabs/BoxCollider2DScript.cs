using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
 using System;

public class BoxCollider2DScript : MonoBehaviour
{
    private string logFile = "collisionLog.txt";
    private byte[] key = new byte[32];
    private byte[] iv = new byte[16]; 

    private void Start()
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(key);
            rng.GetBytes(iv);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string message = "transaction successful " + Time.time.ToString() + " userdata";
        string encryptedMessage = EncryptString(message, key, iv);
        WriteToLog(encryptedMessage);
    }

    private void WriteToLog(string message)
    {
        string path = Path.Combine(Application.persistentDataPath, logFile);
        Debug.Log(path);
        using (StreamWriter writer = File.AppendText(path))
        {
            writer.WriteLine(message);
        }
    }

    private string EncryptString(string plainText, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            byte[] encrypted;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    encrypted = ms.ToArray();
                }
            }

            return Convert.ToBase64String(encrypted);
        }
    }
}