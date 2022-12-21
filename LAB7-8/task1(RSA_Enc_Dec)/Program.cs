/*Написати програму, яка виконує зашифровування та розшифровування 
даних з використанням алгоритмів асиметричного шифрування RSA.
Пара ключів зберігається у пам’яті. 
Реалізувати можливість збереження відкритого ключа у файлі (рядки реалізації цього пункту – закоментовані).*/

using System.Text;
using System.Security.Cryptography;

class Program
{
    public static void Main()
    {
       // string publicKeyPath = @"D:\KNU_FIT\Основи інформаційної безпеки\InfSec_MIT21\LAB7-8\task1(RSA_Enc_Dec)\Public_key\public.xml";

        Console.WriteLine();
        Console.WriteLine("Greetings!");
        Console.Write("Enter your message for encryption and decryption: ");
        string message = Console.ReadLine();
        byte[] messageB = Encoding.UTF8.GetBytes(message);

       // AsymEnc.AssignNewKey(publicKeyPath);
        AsymEnc.AssignNewKey();
        var encMes = AsymEnc.EncryptData(messageB);
        var decMes = AsymEnc.DecryptData(encMes);

        Console.WriteLine($"Message entered by user: {message}");
        Console.WriteLine($"Encrypted message: {Convert.ToBase64String(encMes)}");
        Console.WriteLine($"Decrypted message: {Encoding.UTF8.GetString(decMes)}");
    }
}

public class AsymEnc
{
    private static RSAParameters _publicKey, _privateKey;

    // public static void AssignNewKey(string publicKeyPath)
    public static void AssignNewKey()
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.PersistKeyInCsp = false;
            _publicKey = rsa.ExportParameters(false);
          // File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
            _privateKey = rsa.ExportParameters(true);
        }
    }

    public static byte[] EncryptData(byte[] dataToEncrypt)
    {
        byte[] cipher;
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.PersistKeyInCsp = false;
            rsa.ImportParameters(_publicKey);
            cipher = rsa.Encrypt(dataToEncrypt, true);
        }
        return cipher;
    }

    public static byte[] DecryptData(byte[] dataToDecrypt)
    {
        byte[] plain;
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.PersistKeyInCsp = false;
            rsa.ImportParameters(_privateKey);
            plain = rsa.Decrypt(dataToDecrypt, true);
        }
        return plain;
    }
}