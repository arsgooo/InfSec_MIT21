/*Для програми з п.1 реалізувати можливість задання секретного ключа 
та вектора ініціалізації за допомогою псевдовипадкової послідовності 
із використанням пароля. «Сіль» генерувати як випадкову 
послідовність байтів. Число ітерацій = номер варіанта * 10'000.
Підказка: використовувати клас Rfc2898DeriveBytes.*/

using System.Text;
using System.Security.Cryptography;

class Program
{
    public static void Main()
    {
        while (true)
        {
            int numOfIter = 90000;

            Console.WriteLine();
            Console.WriteLine("Greetings!");
            Console.WriteLine("Choose the encryption algorithm:");
            Console.WriteLine("1 - AES");
            Console.WriteLine("2 - DES");
            Console.WriteLine("3 - Triple DES");
            Console.Write("Enter the digit to proceed: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            Console.Write("Enter the message you want to encrypt and decrypt: ");
            string message = Console.ReadLine();
            Console.Write("Enter password for key and IV generation: ");
            string password = Console.ReadLine();
            byte[] messageB = Encoding.UTF8.GetBytes(message);
            byte[] passwordB = Encoding.UTF8.GetBytes(password);
            byte[] salt = GenerateRandomNumber(32);
            Console.WriteLine();

            switch (choice)
            {
                case 1:
                    var aes_key = EncAndDec.AES_KeyGen(passwordB, salt, numOfIter);
                    var aes_iv = EncAndDec.AES_VectorGen(passwordB, salt, numOfIter);
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
                    var des_key = EncAndDec.DES_KeyVecGen(passwordB, salt, numOfIter);
                    var des_iv = EncAndDec.DES_KeyVecGen(passwordB, salt, numOfIter);
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
                    var tdes_key = EncAndDec.TDES_KeyGen(passwordB, salt, numOfIter);
                    var tdes_iv = EncAndDec.TDES_VectorGen(passwordB, salt, numOfIter);
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

    public static byte[] GenerateRandomNumber(int length) //Used for salt random generation
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
    //AES
    public static byte[] AES_KeyGen(byte[] password, byte[] salt, int numOfIter)
    {
        using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, numOfIter, HashAlgorithmName.SHA256))
        {
            return rfc2898.GetBytes(32);
        }
    }

    public static byte[] AES_VectorGen(byte[] password, byte[] salt, int numOfIter)
    {
        using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, numOfIter, HashAlgorithmName.SHA256))
        {
            return rfc2898.GetBytes(16);
        }
    }

    public static byte[] AES_Encryption(byte[] dataToEncrypt, byte[] key, byte[] iv)
    {
        using (var aes = new AesCryptoServiceProvider())
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            using (var memoryStream = new MemoryStream())
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

    //DES
    public static byte[] DES_KeyVecGen(byte[] password, byte[] salt, int numOfIter)
    {
        using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, numOfIter, HashAlgorithmName.SHA256))
        {
            return rfc2898.GetBytes(8);
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

    //TDES
    public static byte[] TDES_KeyGen(byte[] password, byte[] salt, int numOfIter)
    {
        using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, numOfIter, HashAlgorithmName.SHA256))
        {
            return rfc2898.GetBytes(16);
        }
    }

    public static byte[] TDES_VectorGen(byte[] password, byte[] salt, int numOfIter)
    {
        using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, numOfIter, HashAlgorithmName.SHA256))
        {
            return rfc2898.GetBytes(8);
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