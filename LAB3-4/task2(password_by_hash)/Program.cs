/*Відомо, що користувач використав для пароля довжиною 8 символів 
лише цифри. Шляхом пасивного прослуховування мережі отримано 
MD5 хеш-код цього пароля:
{564c8da6 - 0440 - 88ec - d453 - 0bbad57c6036}
та po1MVkAE7IjUUwu61XxgNg==
Відновити пароль користувача та зробити висновки про надійність такого пароля.*/

using System.Text;
using System.Security.Cryptography;

class Password
{
    public static void Main()
    {
        string givenHash = "po1MVkAE7IjUUwu61XxgNg==";
        string foundHash = "";
        int password = 10000000; //initial value (by default)

        while(!foundHash.Equals(givenHash))
        {
            byte[] passwordByteArr = Encoding.Unicode.GetBytes(password.ToString());
            byte[] passwordByteArrHash = ComputeHashMD5(passwordByteArr);
            foundHash = Convert.ToBase64String(passwordByteArrHash);
            Console.WriteLine(foundHash);
            password++;
        }

        Console.WriteLine($"Password found from MD5 hash is: {password}"); //password = 20192021

        static byte[] ComputeHashMD5(byte[] inputByteArr)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(inputByteArr);
            }
        }
    }
}