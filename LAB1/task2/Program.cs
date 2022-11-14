//Generating random numbers using RNGCryptoServiceProvider
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        using (var randNumGen = new RNGCryptoServiceProvider())
        {
            var randNum = new byte[5]; //creating a byte array (5 bytes is the length of our random number)

            Console.WriteLine("Numbers generated using RNGCryptoServiceProvider:");
            Console.WriteLine(" ");

            for (int i = 0; i < 15; i++)
            {
                randNumGen.GetBytes(randNum); //byte array filling with random data
                string num = Convert.ToBase64String(randNum);
                Console.WriteLine(num);
            }

            Console.WriteLine(" ");
            Console.WriteLine("That's it!");
        }
    }
}