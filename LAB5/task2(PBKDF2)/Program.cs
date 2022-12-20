/*Розробити клас PBKDF2, що має наступну функціональність: генерує "сіль", задає
алгоритм хешування (MD5, SHA1, SHA256, SHA384, SHA512) та обчислює хеш для 
заданого числа ітерацій. Створити програму, що обчислює час, витрачений на 
обчислення хешу для різного числа ітерацій (10 значень із кроком 50'000; перше 
значення = номер варіанта * 10'000).*/

using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;

class Program
{
    public static void Main()
    {
        Console.WriteLine("");
        Console.WriteLine("Greetings!");
        Console.Write("Enter password you want to hash: ");
        string password = Console.ReadLine();
        byte[] pasByteArr = Encoding.Unicode.GetBytes(password);

        HashPassword(pasByteArr, 90000, HashAlgorithmName.MD5);
        HashPassword(pasByteArr, 140000, HashAlgorithmName.MD5);
        HashPassword(pasByteArr, 190000, HashAlgorithmName.SHA1);
        HashPassword(pasByteArr, 240000, HashAlgorithmName.SHA1);
        HashPassword(pasByteArr, 290000, HashAlgorithmName.SHA256);
        HashPassword(pasByteArr, 340000, HashAlgorithmName.SHA256);
        HashPassword(pasByteArr, 390000, HashAlgorithmName.SHA384);
        HashPassword(pasByteArr, 440000, HashAlgorithmName.SHA384);
        HashPassword(pasByteArr, 490000, HashAlgorithmName.SHA512);
        HashPassword(pasByteArr, 540000, HashAlgorithmName.SHA512);
    }

    private static void HashPassword(byte[] pasByteArr, int numofIter, HashAlgorithmName hashAlgorithm)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var hashedPassword = PBKDF2.HashPassword(pasByteArr, PBKDF2.GenerateSalt(), numofIter, hashAlgorithm);
        stopwatch.Stop();

        Console.WriteLine("");
        Console.WriteLine($"Hashed password: {Convert.ToBase64String(hashedPassword)}");
        Console.WriteLine($"<Hash algorithm used>: {hashAlgorithm}");
        Console.WriteLine($"<Number of iterations performed>: {numofIter}");
        Console.WriteLine($"<Elapsed time>: {stopwatch.ElapsedMilliseconds} ms");
    }
}

public class PBKDF2
{
    public static byte[] GenerateSalt()
    {
        const int saltLength = 32;
        using (var randNumGen = new RNGCryptoServiceProvider())
        {
            var randomNumber = new byte[saltLength];
            randNumGen.GetBytes(randomNumber);
            return randomNumber;
        }
    }

    public static byte[] HashPassword(byte[] pas, byte[] salt, int numOfIter, HashAlgorithmName hashAlgorithm)
    {
        using (var rfc2898 = new Rfc2898DeriveBytes(pas, salt, numOfIter))
        {
            int hashSize = 0;
            if (hashAlgorithm == HashAlgorithmName.MD5) hashSize = 16;
            else if (hashAlgorithm == HashAlgorithmName.SHA1) hashSize = 20;
            else if (hashAlgorithm == HashAlgorithmName.SHA256) hashSize = 32;
            else if (hashAlgorithm == HashAlgorithmName.SHA384) hashSize = 48;
            else if (hashAlgorithm == HashAlgorithmName.SHA512) hashSize = 64;
            return rfc2898.GetBytes(hashSize);
        }
    }
}