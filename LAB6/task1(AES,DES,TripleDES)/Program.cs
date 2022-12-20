/*Написати програму, яка виконує зашифровування та розшифровування 
даних з використанням алгоритмів симетричного шифрування DES,
Triple-DES, AES. Секретний ключ та вектор ініціалізації генерується випадковим чином.*/

using System.Text;
using System.Security.Cryptography;

class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Greetings!");
            Console.WriteLine("Choose the encryption algorithm:");
            Console.WriteLine("1 - AES");
            Console.WriteLine("2 - DES");
            Console.WriteLine("3 - Triple DES");
            Console.Write("Enter the digit to proceed: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the message you want to encrypt and decrypt: ");
            string message = Console.ReadLine();
            byte[] messageB = Encoding.UTF8.GetBytes(message);
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    var aes_key = GenerateRandomNumber(32);
                    var aes_iv = GenerateRandomNumber(16);
                    var aes_enc_message = EncAndDec.AES_Encryption(messageB, aes_key, aes_iv);
                    var aes_dec_message = EncAndDec.AES_Decryption(aes_enc_message, aes_key, aes_iv);
                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine("AES ENCRYPTION");
                    Console.WriteLine();
                    Console.WriteLine($"Message entered: {message}");
                    Console.WriteLine($"Encrypted message: {Convert.ToBase64String(aes_enc_message)}");
                    Console.WriteLine($"Decrypted message: {Encoding.UTF8.GetString(aes_dec_message)}");
                    Console.WriteLine("-------------------------------------------------");
                    break;

                case 2:
                    var des_key = GenerateRandomNumber(8);
                    var des_iv = GenerateRandomNumber(8);
                    var des_enc_message = EncAndDec.DES_Encryption(messageB, des_key, des_iv);
                    var des_dec_message = EncAndDec.DES_Decryption(des_enc_message, des_key, des_iv);
                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine("DES ENCRYPTION");
                    Console.WriteLine();
                    Console.WriteLine($"Message entered: {message}");
                    Console.WriteLine($"Encrypted message: {Convert.ToBase64String(des_enc_message)}");
                    Console.WriteLine($"Decrypted message: {Encoding.UTF8.GetString(des_dec_message)}");
                    Console.WriteLine("-------------------------------------------------");
                    break;

                case 3:
                    var tdes_key = GenerateRandomNumber(16);
                    var tdes_iv = GenerateRandomNumber(8);
                    var tdes_enc_message = EncAndDec.TDES_Encryption(messageB, tdes_key, tdes_iv);
                    var tdes_dec_message = EncAndDec.TDES_Decryption(tdes_enc_message, tdes_key, tdes_iv);
                    Console.WriteLine("-------------------------------------------------");
                    Console.WriteLine("TDES ENCRYPTION");
                    Console.WriteLine();
                    Console.WriteLine($"Message entered: {message}");
                    Console.WriteLine($"Encrypted message: {Convert.ToBase64String(tdes_enc_message)}");
                    Console.WriteLine($"Decrypted message: {Encoding.UTF8.GetString(tdes_dec_message)}");
                    Console.WriteLine("-------------------------------------------------");
                    break;
            }
        }
    }

    public static byte[] GenerateRandomNumber(int length) //Used for key and initialization vector random generation
    {
        using (var randNumGen = new RNGCryptoServiceProvider())
        {
            var randomNumber = new byte[length];
            randNumGen.GetBytes(randomNumber);
            return randomNumber;
        }
    }
}

public class EncAndDec
{
    public static byte[] AES_Encryption(byte[] dataToEncrypt, byte[] key, byte[] iv)
    {
        using(var aes = new AesCryptoServiceProvider())
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            using(var memoryStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }
    }

    public static byte[] AES_Decryption(byte[] dataToDecrypt, byte[] key, byte[] iv)
    {
        using (var aes = new AesCryptoServiceProvider())
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            using (var memoryStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }
    }

    public static byte[] DES_Encryption(byte[] dataToEncrypt, byte[] key, byte[] iv)
    {
        using (var des = new DESCryptoServiceProvider())
        {
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            des.Key = key;
            des.IV = iv;

            using (var memoryStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }
    }

    public static byte[] DES_Decryption(byte[] dataToDecrypt, byte[] key, byte[] iv)
    {
        using (var des = new DESCryptoServiceProvider())
        {
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            des.Key = key;
            des.IV = iv;

            using (var memoryStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }
    }

    public static byte[] TDES_Encryption(byte[] dataToEncrypt, byte[] key, byte[] iv)
    {
        using (var tdes = new TripleDESCryptoServiceProvider())
        {
            tdes.Mode = CipherMode.CBC;
            tdes.Padding = PaddingMode.PKCS7;
            tdes.Key = key;
            tdes.IV = iv;

            using (var memoryStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(memoryStream, tdes.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }
    }

    public static byte[] TDES_Decryption(byte[] dataToDecrypt, byte[] key, byte[] iv)
    {
        using (var tdes = new TripleDESCryptoServiceProvider())
        {
            tdes.Mode = CipherMode.CBC;
            tdes.Padding = PaddingMode.PKCS7;
            tdes.Key = key;
            tdes.IV = iv;

            using (var memoryStream = new MemoryStream())
            {
                var cryptoStream = new CryptoStream(memoryStream, tdes.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }
    }
}