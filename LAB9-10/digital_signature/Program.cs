/*Написати програму, яка виконує підписування повідомлення та 
перевірку ЕЦП з використанням алгоритму асиметричного 
шифрування RSA. Відкритий ключ імпортується із XML-файлу, 
секретний – береться із контейнера.*/

using System.Text;
using System.Security.Cryptography;

class Program
{
    public static void Main()
    {
        string publicKeyPath = @"D:\KNU_FIT\Основи інформаційної безпеки\InfSec_MIT21\LAB9-10\digital_signature\data\public.xml";

        Console.WriteLine();
        Console.WriteLine("Greetings!");
        Console.Write("Enter your message: ");
        string message = Console.ReadLine();
        byte[] messageB = Encoding.UTF8.GetBytes(message);
        byte[] hashOfMessage;

        using(var sha512 = SHA512.Create()) //Generating hash from message entered by user
        {
            hashOfMessage = sha512.ComputeHash(messageB);
        }

        var digitalSignature = new DigitalSignature();
        digitalSignature.AssignNewKey(publicKeyPath);
        var signature = digitalSignature.SignData(hashOfMessage);
        var verified = digitalSignature.VerifySignature(publicKeyPath, hashOfMessage, signature);

        Console.WriteLine();
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine($"Message entered: {message}");
        Console.WriteLine($"Digital signature: {Convert.ToBase64String(signature)}");
        Console.WriteLine(verified ? "The digital signature has been correctly verified." : "The digital signature has NOT been correctly verified.");
        Console.WriteLine("-----------------------------------------------------");
    }
}

public class DigitalSignature
{
    private readonly static string CspContainerName = "RsaContainer";
    public void AssignNewKey(string publicKeyPath) //Generating public key
    {
        var cspParams = new CspParameters
        {
            KeyContainerName = CspContainerName,
            Flags = CspProviderFlags.UseMachineKeyStore
        };
        using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
        {
            rsa.PersistKeyInCsp = true; //Using private key stored in the key container
            File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
        }
    }

    public byte[] SignData(byte[] hashOfMessage)
    {
        var cspParams = new CspParameters
        {
            KeyContainerName = CspContainerName,
            Flags = CspProviderFlags.UseMachineKeyStore,
        };
        using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
        {
            rsa.PersistKeyInCsp = false;
            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa); //RSAPKCS1SignatureFormatter class used to sign the message entered by user (passing rsa object which contains a private key)
            rsaFormatter.SetHashAlgorithm(nameof(SHA512));
            return rsaFormatter.CreateSignature(hashOfMessage);
        }
    }

    public bool VerifySignature(string publicKeyPath, byte[] hashOfMessage, byte[] signature)
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            rsa.PersistKeyInCsp = false;
            rsa.FromXmlString(File.ReadAllText(publicKeyPath)); //Using public key stored in the XML file
            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa); //RSAPKCS1SignatureDeformatter class used to verify the user's signature (passing rsa object which contains a public key)
            rsaDeformatter.SetHashAlgorithm(nameof(SHA512));
            return rsaDeformatter.VerifySignature(hashOfMessage, signature);
        }
    }
}