/*Згенерувати відкритий ключ RSA 2048bit та зберегти у файл з 
іменем, що відповідає транслітерації прізвища, та розширенням xml.*/

using System.Text;
using System.Security.Cryptography;

class Program
{
    public static void Main()
    {
        string keyFolderPath = @"D:\KNU_FIT\Основи інформаційної безпеки\InfSec_MIT21\LAB7-8\task2(KeyGen)\Public_key\";
        string encMesFolderPath = @"D:\KNU_FIT\Основи інформаційної безпеки\InfSec_MIT21\LAB7-8\task2(KeyGen)\Encrypted_message\";
        string keyFileExt = ".xml";
        string encFileExt = ".txt";
        string surname;
        byte[] decMes;
        string keyFilePath;
        string encMesFilePath;

        while(true)
        {
            Console.WriteLine();
            Console.WriteLine("Greetings!");
            Console.WriteLine("1 - Generate new key and save it to XML file");
            Console.WriteLine("2 - Delete keys");
            Console.WriteLine("3 - Encrypt the message");
            Console.WriteLine("4 - Decrypt the message");
            Console.WriteLine("5 - Exit");
            Console.Write("Please, enter the option to continue: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            switch(choice)
            {
                case 1:
                    Console.WriteLine();
                    Console.Write("Enter your surname: ");
                    surname = Console.ReadLine();
                    if(String.IsNullOrEmpty(surname))
                    {
                        Console.WriteLine("You haven't specified a surname. Try again, please.");
                        break;
                    }
                    keyFilePath = keyFolderPath + surname + keyFileExt;
                    AsymEnc.GenerateOwnKeys(keyFilePath);
                    Console.WriteLine("The key was generated. Check the file if needed.");
                    break;

                case 2:
                    Console.WriteLine();
                    Console.Write("Enter your surname: ");
                    surname = Console.ReadLine();
                    if (String.IsNullOrEmpty(surname))
                    {
                        Console.WriteLine("You haven't specified a surname. Try again, please.");
                        break;
                    }
                    keyFilePath = keyFolderPath + surname + keyFileExt;
                    AsymEnc.DeleteKey(keyFilePath);
                    var files = Directory.GetFiles(encMesFolderPath);
                    for (int i = 0; i < files.Length; i++)
                    {
                        File.Delete(files[i]);
                    }
                    Console.WriteLine($"The key was successfully deleted!");
                    break;

                case 3:
                    Console.WriteLine();
                    var key_files = Directory.GetFiles(keyFolderPath);
                    if (key_files.Length != 0)
                    {
                        Console.WriteLine("Public keys:");
                        for (int i = 0; i < key_files.Length; i++)
                        {
                            Console.WriteLine((i + 1) + ". " + key_files[i]);
                        }
                        Console.Write("Enter public key number: ");
                        int num = Convert.ToInt32(Console.ReadLine());
                        keyFilePath = key_files[num - 1];
                        Console.Write("Enter your message: ");
                        string message = Console.ReadLine();
                        Console.Write("Enter file name (without extension): ");
                        string encMesFileName = Console.ReadLine();
                        encMesFilePath = encMesFolderPath + encMesFileName + encFileExt;
                        AsymEnc.EncryptData(keyFilePath, Encoding.UTF8.GetBytes(message), encMesFilePath);
                        Console.WriteLine($"The encryption was done!");
                    }
                    else Console.WriteLine("There are no public keys to encrypt data. Generate a key and try again.");
                    break;

                case 4:
                    Console.WriteLine();
                    var enc_files = Directory.GetFiles(encMesFolderPath);
                    if (enc_files.Length != 0)
                    {
                        Console.WriteLine("Encrypted messages:");
                        for (int i = 0; i < enc_files.Length; i++)
                        {
                            Console.WriteLine((i + 1) + ". " + enc_files[i]);
                        }
                        Console.WriteLine();
                        Console.Write("Enter message number to decrypt: ");
                        int num = Convert.ToInt32(Console.ReadLine());
                        encMesFilePath = enc_files[num - 1];
                        decMes = AsymEnc.DecryptData(encMesFilePath);
                        Console.WriteLine($"Decrypted message: {Encoding.UTF8.GetString(decMes)}");
                    }
                    else Console.WriteLine("There are no files to decrypt. Please, encrypt one and try again.");
                    break;

                case 5:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}

public class AsymEnc
{
    const string CspContainerName = "MyContainer";
    public static void GenerateOwnKeys(string keyFilePath)
    {
        CspParameters cspParameters = new CspParameters(1)
        {
            KeyContainerName = CspContainerName,
            Flags = CspProviderFlags.UseMachineKeyStore,
            ProviderName = "Microsoft Strong Cryptographic Provider",
        };
        using (var rsa = new RSACryptoServiceProvider(2048, cspParameters))
        {
            rsa.PersistKeyInCsp = true;
            File.WriteAllText(keyFilePath, rsa.ToXmlString(false));
        }
    }

    public static void DeleteKey(string keyFilePath)
    {
        CspParameters cspParameters = new CspParameters
        {
            KeyContainerName = CspContainerName,
            Flags = CspProviderFlags.UseMachineKeyStore,
        };
        var rsa = new RSACryptoServiceProvider(cspParameters)
        {
            PersistKeyInCsp = false
        };
        File.Delete(keyFilePath);
        rsa.Clear();
    }

    public static void EncryptData(string keyFilePath, byte[] dataToEncrypt, string encMesFilePath)
    {
        byte[] chipherBytes;
        var cspParams = new CspParameters
        {
            KeyContainerName = CspContainerName,
            Flags = CspProviderFlags.UseMachineKeyStore
        };
        using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
        {
            rsa.PersistKeyInCsp = false;
            rsa.FromXmlString(File.ReadAllText(keyFilePath));
            chipherBytes = rsa.Encrypt(dataToEncrypt, true);
        }
        File.WriteAllBytes(encMesFilePath, chipherBytes);
    }

    public static byte[] DecryptData(string encMesFilePath)
    {
        byte[] chipherBytes = File.ReadAllBytes(encMesFilePath);
        byte[] plainTextBytes;
        var cspParams = new CspParameters
        {
            KeyContainerName = CspContainerName,
            Flags = CspProviderFlags.UseMachineKeyStore
        };
        using (var rsa = new RSACryptoServiceProvider(2048, cspParams))
        {
            rsa.PersistKeyInCsp = true;
            plainTextBytes = rsa.Decrypt(chipherBytes, true);
        }
        return plainTextBytes;
    }
}