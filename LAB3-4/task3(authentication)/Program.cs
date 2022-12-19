//Написати програму для обчислення хеш-коду автентифікації 
//повідомлення, а також реалізувати можливість перевірки автентичності повідомлення.

using System.Security.Cryptography;
using System.Text;

class Authentication
{
    public static void Main()
    {
        int MD5Size = 16;       //Hash sizes (will be used for keys generation)
        int SHA1Size = 20;
        int SHA256Size = 32;
        int SHA512Size = 64;

        Console.WriteLine("");
        Console.Write("Enter the message you want to hash: ");
        string input = Console.ReadLine();
        byte[] inputByteArr = Encoding.Unicode.GetBytes(input);
        Console.WriteLine("");

        byte[] keyForMD5 = KeyGenerator(MD5Size);       //Declaring and initializing keys
        byte[] keyForSHA1 = KeyGenerator(SHA1Size);
        byte[] keyForSHA256 = KeyGenerator(SHA256Size);
        byte[] keyForSHA512 = KeyGenerator(SHA512Size);


        //Sender data hashing
        var HMACMD5ForInput = ComputeHMACMD5(inputByteArr, keyForMD5);
        var HMACSHA1ForInput = ComputeHMACSHA1(inputByteArr, keyForSHA1);
        var HMACSHA256ForInput = ComputeHMACSHA256(inputByteArr, keyForSHA256);
        var HMACSHA512ForInput = ComputeHMACSHA512(inputByteArr, keyForSHA512);
        Console.WriteLine("----------------------------------------------------------------------------------------------------------------");
        Console.WriteLine($"HMACMD5 (sender): {Convert.ToBase64String(HMACMD5ForInput)}");
        Console.WriteLine($"HMACSHA1 (sender): {Convert.ToBase64String(HMACSHA1ForInput)}");
        Console.WriteLine($"HMACSHA256 (sender): {Convert.ToBase64String(HMACSHA256ForInput)}");
        Console.WriteLine($"HMACSHA512 (sender): {Convert.ToBase64String(HMACSHA512ForInput)}");
        Console.WriteLine("----------------------------------------------------------------------------------------------------------------");


        //Receiver data hashing (for further comparison)
        var HMACMD5ForInput_2 = ComputeHMACMD5(inputByteArr, keyForMD5);
        var HMACSHA1ForInput_2 = ComputeHMACSHA1(inputByteArr, keyForSHA1);
        var HMACSHA256ForInput_2 = ComputeHMACSHA256(inputByteArr, keyForSHA256);
        var HMACSHA512ForInput_2 = ComputeHMACSHA512(inputByteArr, keyForSHA512);
        Console.WriteLine("----------------------------------------------------------------------------------------------------------------");
        Console.WriteLine($"HMACMD5 (receiver): {Convert.ToBase64String(HMACMD5ForInput_2)}");
        Console.WriteLine($"HMACSHA1 (receiver): {Convert.ToBase64String(HMACSHA1ForInput_2)}");
        Console.WriteLine($"HMACSHA256 (receiver): {Convert.ToBase64String(HMACSHA256ForInput_2)}");
        Console.WriteLine($"HMACSHA512 (receiver): {Convert.ToBase64String(HMACSHA512ForInput_2)}");
        Console.WriteLine("----------------------------------------------------------------------------------------------------------------");


        //Hashes comparison
        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine($"HMAC MD5 comparison result: {CompareHashes(HMACMD5ForInput, HMACMD5ForInput_2)}");
        Console.WriteLine($"HMAC SHA1 comparison result: {CompareHashes(HMACSHA1ForInput, HMACSHA1ForInput_2)}");
        Console.WriteLine($"HMAC SHA256 comparison result: {CompareHashes(HMACSHA256ForInput, HMACSHA256ForInput_2)}");
        Console.WriteLine($"HMAC SHA512 comparison result: {CompareHashes(HMACSHA512ForInput, HMACSHA512ForInput_2)}");
        Console.WriteLine("-------------------------------------------------------");


        static byte[] KeyGenerator(int hashSize)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                var key = new byte[hashSize];
                rng.GetBytes(key);
                return key;
            }
        }

        static byte[] ComputeHMACMD5(byte[] inputByteArr, byte[] keyByteArr)
        {
            using (var hmacmd5 = new HMACMD5(keyByteArr))
            {
                return hmacmd5.ComputeHash(inputByteArr);
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

        static string CompareHashes(byte[] initHash, byte[] finalHash)
        {
            if (Convert.ToBase64String(initHash) == Convert.ToBase64String(finalHash)) return "Hashes match. Excellent!";
            else return "Hashes don't match. Receiver got a modified message or file.";
        }
    }
}