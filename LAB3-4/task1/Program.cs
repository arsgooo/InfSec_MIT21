//Написати програму, яка обчислює хеш-коди за всіма відомими
//алгоритмами для заданих даних. Порівняти розміри хеш-кодів та
//значення для однакових та різних даних.

using System.Security.Cryptography;
using System.Text;

class Hashing {
    public static void Main()
    {
        Console.WriteLine("");
        Console.Write("Enter the message you want to hash: ");
        string input = Console.ReadLine();
        Console.Write("Enter the secret key (for HMAC hashing): ");
        string key = Console.ReadLine();
        byte[] inputByteArr = Encoding.UTF8.GetBytes(input);
        byte[] keyByteArr = Encoding.UTF8.GetBytes(key);
        Console.WriteLine("");
        Console.WriteLine("----------------------------------------------------------------------------------------------------------");

        var MD5ForInput = ComputeHashMD5(inputByteArr);
        Guid guidMD5ForInput = new Guid(MD5ForInput);
        var SHA1ForInput = ComputeHashSHA1(inputByteArr);
        var SHA256ForInput = ComputeHashSHA256(inputByteArr);
        var SHA384ForInput = ComputeHashSHA384(inputByteArr);
        var SHA512ForInput = ComputeHashSHA512(inputByteArr);
        var HMACSHA1ForInput = ComputeHMACSHA1(inputByteArr, keyByteArr);
        var HMACSHA256ForInput = ComputeHMACSHA256(inputByteArr, keyByteArr);
        var HMACSHA512ForInput = ComputeHMACSHA512(inputByteArr, keyByteArr);
        var HMACMD5ForInput = ComputeHMACMD5(inputByteArr, keyByteArr);

        Console.WriteLine($"Hash MD5: {Convert.ToBase64String(MD5ForInput)}");
        Console.WriteLine($"Guid: {guidMD5ForInput}");
        Console.WriteLine($"Hash SHA1: {Convert.ToBase64String(SHA1ForInput)}");
        Console.WriteLine($"Hash SHA256: {Convert.ToBase64String(SHA256ForInput)}");
        Console.WriteLine($"Hash SHA384: {Convert.ToBase64String(SHA384ForInput)}");
        Console.WriteLine($"Hash SHA512: {Convert.ToBase64String(SHA512ForInput)}");
        Console.WriteLine($"Hash HMACSHA1: {Convert.ToBase64String(HMACSHA1ForInput)}");
        Console.WriteLine($"Hash HMACSHA256: {Convert.ToBase64String(HMACSHA256ForInput)}");
        Console.WriteLine($"Hash HMACSHA512: {Convert.ToBase64String(HMACSHA512ForInput)}");
        Console.WriteLine($"Hash HMACMD5: {Convert.ToBase64String(HMACMD5ForInput)}");
        Console.WriteLine("----------------------------------------------------------------------------------------------------------");

        static byte[] ComputeHashMD5(byte[] inputByteArr)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(inputByteArr);
            }
        }

        static byte[] ComputeHashSHA1(byte[] inputByteArr)
        {
            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(inputByteArr);
            }
        }

        static byte[] ComputeHashSHA256(byte[] inputByteArr)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(inputByteArr);
            }
        }

        static byte[] ComputeHashSHA384(byte[] inputByteArr)
        {
            using (var sha384 = SHA384.Create())
            {
                return sha384.ComputeHash(inputByteArr);
            }
        }

        static byte[] ComputeHashSHA512(byte[] inputByteArr)
        {
            using (var sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(inputByteArr);
            }
        }

        static byte[] ComputeHMACSHA1(byte[] inputByteArr, byte[] keyByteArr)
        {
            using (var hmacsha1 = new HMACSHA1(keyByteArr))
            {
                return hmacsha1.ComputeHash(inputByteArr);
            }
        }

        static byte[] ComputeHMACSHA256(byte[] inputByteArr, byte[] keyByteArr)
        {
            using (var hmacsha256 = new HMACSHA256(keyByteArr))
            {
                return hmacsha256.ComputeHash(inputByteArr);
            }
        }

        static byte[] ComputeHMACSHA512(byte[] inputByteArr, byte[] keyByteArr)
        {
            using (var hmacsha512 = new HMACSHA512(keyByteArr))
            {
                return hmacsha512.ComputeHash(inputByteArr);
            }
        }

        static byte[] ComputeHMACMD5(byte[] inputByteArr, byte[] keyByteArr)
        {
            using (var hmacmd5 = new HMACMD5(keyByteArr))
            {
                return hmacmd5.ComputeHash(inputByteArr);
            }
        }
    }
}