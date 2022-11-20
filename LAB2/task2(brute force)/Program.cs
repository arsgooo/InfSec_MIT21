//File decryption using brute force method

using System;
using System.Text;

class Program
{
    class XOR_Algorithm
    {
        private byte[] KeyGen(byte[] password, byte[] data) //generating a secret key
        {
            byte[] key = new byte[data.Length]; //creating an array where the secret key will be stored
            for (int i = 0; i < key.Length; i++)
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

    static void Main()
    {
        string path = @"D:\KNU_FIT\Основи інформаційної безпеки\InfSec_MIT21\LAB2\task2(brute force)\";
        string result = path + "result.txt";
        string phrase = "Mit21";

        byte[] phraseB = Encoding.UTF8.GetBytes(phrase); //converting the phrase into bytes and putting it into byte array
        byte[] encFileData = File.ReadAllBytes(path + "encfile5.dat").ToArray(); //reading encrypted file content and putting it into byte array
        byte[] key = new byte[5]; //creating 5-byte array
        var forDec = new XOR_Algorithm();

        for (int i = 0; i < encFileData.Length - phrase.Length; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                key[j] = encFileData[j + i];
            }

            var passB = forDec.XOR(phraseB, key);
            var textFinalB = forDec.XOR(passB, encFileData); //decryption of .dat file
            string textFinal = Encoding.UTF8.GetString(textFinalB); //converting the final text into string

            if (textFinal.Contains(" Mit21 "))
            {
                File.AppendAllText(result, textFinal);
                Console.WriteLine("The encryption was done! Check the .txt file.");
            }
        }
    }
}