/*Розробити клас SaltedHash, що реалізує хешування паролів із додаванням 
додаткової ентропії. Продемонструвати роботу класу, обчислюючи хеш для заданого пароля та "солі".*/

using System.Text;
using System.Security.Cryptography;

class Program
{
    public static void Main()
    {
        Console.WriteLine("");
        Console.WriteLine("Greetings!");
        Console.Write("Enter password you want to hash: ");
        string password = Console.ReadLine();
        byte[] pasByteArr = Encoding.Unicode.GetBytes(password);
        byte[] saltByteArr = SaltedHash.GenerateSalt();
        var hashedPassword = SaltedHash.HashPasWithSalt(pasByteArr, saltByteArr);

        Console.WriteLine("");
        Console.WriteLine("-------------------------------------------------------------------------------");
        Console.WriteLine($"Password entered by user: {password}");
        Console.WriteLine($"Salt generated: {Convert.ToBase64String(saltByteArr)}");
        Console.WriteLine($"Hashed password (password + salt): {Convert.ToBase64String(hashedPassword)}");
        Console.WriteLine("-------------------------------------------------------------------------------");
    }
}

public class SaltedHash
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

    private static byte[] CombinePasAndSalt(byte[] pas, byte[] salt)
    {
        var full = new byte[pas.Length + salt.Length];
        Buffer.BlockCopy(pas, 0, full, 0, pas.Length); //Copying password to full array
        Buffer.BlockCopy(salt, 0, full, pas.Length, salt.Length); //Extending full array with salt
        return full;
    }

    public static byte[] HashPasWithSalt(byte[] pas, byte[] salt)
    {
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(CombinePasAndSalt(pas, salt));
        }
    }
}