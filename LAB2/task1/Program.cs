//File encryption and decryption

using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    class XOR_Algorithm
    {
        private byte[] KeyGen(byte[] password, byte[] data) //generating a secret key
        {
            byte[] key = new byte[data.Length]; //creating an array where the secret key will be stored
            for(int i = 0; i < key.Length; i++)
            {
                key[i] = password[i % password.Length];
            }
            return key;
        }

        public byte[] XOR(byte[] password, byte[] data) //XOR operation
        {
            byte[] secretKey = KeyGen(password, data); //getting a secret key from KeyGen function
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ secretKey[i]); //XOR
            }
            return data;
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Greetings!");
        Console.WriteLine("\n1 - encrypt the file and write this data into .dat file");
        Console.WriteLine("2 - decrypt an encrypted file");
        Console.WriteLine("3 - clear the .dat file");
        Console.Write("Please, choose one of the options given: ");
        int choice = Convert.ToInt32(Console.ReadLine());

        string path = @"D:\KNU_FIT\Основи інформаційної безпеки\InfSec_MIT21\LAB2\";
        byte[] file_data = File.ReadAllBytes(path + "init_file.txt").ToArray(); //reading file content and putting it into byte array

        switch(choice)
        {
            case 1:
                var forEnc = new XOR_Algorithm();
                Console.Write("Write the password for encryption: ");
                string encPas = Console.ReadLine();
                if (String.IsNullOrEmpty(encPas)) 
                {
                    Console.WriteLine("\nUnfortunately, the encryption cannot be done. Try again please.");
                    break;
                }
                byte[] encPasInBytes = Encoding.UTF8.GetBytes(encPas); //converting the password entered by user into bytes
                var encData = forEnc.XOR(encPasInBytes, file_data); //getting encrypted data with XOR operation
                File.WriteAllBytes(path + "encr_file.dat", encData); //writing encrypted data into .dat file
                Console.WriteLine("\nThe encryption was done! Check the .dat file.");
                break;
            
            case 2:
                if(new FileInfo(path + "encr_file.dat").Length == 0)
                {
                    Console.WriteLine("\nThe encrypted file is empty so the decryption cannot be done. Encrypt the data first.");
                    break;
                }

                byte[] encFileData = File.ReadAllBytes(path + "encr_file.dat").ToArray(); //reading file encrypted content and putting it into byte array
                var forDec = new XOR_Algorithm();
                Console.WriteLine("\nATTENTION! You will get a correct decryption only if the passwords match!");
                Console.Write("Write the password that you previously used to decrypt the file: ");
                string decPas = Console.ReadLine();
                if (String.IsNullOrEmpty(decPas))
                {
                    Console.WriteLine("\nUnfortunately, the decryption cannot be done. Try again please.");
                    break;
                }
                byte[] decPasInBytes = Encoding.UTF8.GetBytes(decPas);
                var decData = forDec.XOR(decPasInBytes, encFileData); //getting decrypted data with XOR operation
                File.WriteAllBytes(path + "encr_file.dat", decData); //writing decrypted data into .dat file
                Console.WriteLine("\nThe decryption was done! Check the .dat file.");
                Console.WriteLine(Encoding.UTF8.GetString(decData)); //converting decrypted data into text
                break;

            case 3:
                File.WriteAllText(path + "encr_file.dat", string.Empty); //making file empty
                break;
        }
    }
}