/*Написати програму, що реалізує хешування введеного пароля під час реєстрації 
користувача та зберігає логін, пароль та "сіль" у пам'яті. Реалізувати можливість 
автентифікації за логіном і паролем. Число ітерацій = номер варіанта * 10'000.*/

using System.Text;
using System.Security.Cryptography;

class Program
{
    public static void Main()
    {
        int users_am = 0;
        string salt = "24ikpn5dm9";
        string[] logins = new string[10];
        string[] passwords = new string[10];
        string[] salts = new string[10];

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Greetings!");
            Console.WriteLine("1 - Create a user");
            Console.WriteLine("2 - Log in");
            Console.WriteLine("3 - See all users available");
            Console.WriteLine("4 - Exit");
            Console.Write("Please, enter the option: ");
            int option = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            switch (option)
            {
                case 1:
                    Console.WriteLine("-------------------------------------------------------");
                    Console.Write("Create login: ");
                    string login = Console.ReadLine();
                    if(logins.Contains(login))
                    {
                        Console.WriteLine($"User with login {login} already exists. Please, enter another name.");
                        break;
                    }
                    Console.Write("Create password: ");
                    string password = "";
                    while (true)
                    {
                        var key = Console.ReadKey(true); //True is used here not to display our input in console
                        if (key.Key == ConsoleKey.Enter)
                            break;
                        password += key.KeyChar;
                    }
                    byte[] passwordByteArr = Encoding.Unicode.GetBytes(password);
                    logins[users_am] = login;
                    passwords[users_am] = Convert.ToBase64String(HashPassword(passwordByteArr, salt, 90000));
                    // salts[users_am] = Convert.ToBase64String(PBKDF2.GenerateSalt());
                    salts[users_am] = salt;
                    users_am++;
                    Console.WriteLine($"\n\nThe user {login} was created. Use option 2 to verify this.");
                    Console.WriteLine($"Current amount of signed up users: {users_am}");
                    Console.WriteLine("-------------------------------------------------------");
                    break;

                case 2:
                    Console.WriteLine("-------------------------------------------------------");
                    Console.Write("Enter login: ");
                    string realLogin = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string realPassword = "";
                    while (true)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                            break;
                        realPassword += key.KeyChar;
                    }
                    byte[] realPasswordByteArr = Encoding.Unicode.GetBytes(realPassword);
                    int indexLogin = Array.IndexOf(logins, realLogin);
                    int indexPas = Array.IndexOf(passwords, Convert.ToBase64String(HashPassword(realPasswordByteArr, salt, 90000)));
                    if (logins.Contains(realLogin) && passwords.Contains(Convert.ToBase64String(HashPassword(realPasswordByteArr, salt, 90000))) && indexLogin == indexPas)
                    {
                        Console.WriteLine("\n\nYou were successfully logged in!");
                        Console.WriteLine("-------------------------------------------------------");
                    }
                    else
                    {
                        Console.WriteLine($"\n\nIncorrect login or password. Please, try again.");
                        Console.WriteLine("-------------------------------------------------------");
                    }
                    break;

                case 3:
                    Console.WriteLine("-------------------------------------------------------");
                    if (users_am == 0)
                    {
                        Console.WriteLine("Nothing to show. Add at least 1 user.");
                        Console.WriteLine("-------------------------------------------------------");
                        break;
                    }
                    for (int i = 0; i < users_am; i++)
                    {
                        Console.WriteLine(logins[i] + "  " + passwords[i] + "  " + salts[i]);
                    }
                    Console.WriteLine("-------------------------------------------------------");
                    break;

                case 4:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private static byte[] HashPassword(byte[] pasByteArr, string salt, int numofIter)
    {
        byte[] saltByteArr = Encoding.Unicode.GetBytes(salt);
        //var hashedPassword = PBKDF2.HashPassword(pasByteArr, PBKDF2.GenerateSalt(), numofIter);
        var hashedPassword = PBKDF2.HashPassword(pasByteArr, saltByteArr, numofIter);
        return hashedPassword;
    }
}

public class PBKDF2
{
    /*public static byte[] GenerateSalt()
    {
        const int saltLength = 32;
        using (var randNumGen = new RNGCryptoServiceProvider())
        {
            var randomNumber = new byte[saltLength];
            randNumGen.GetBytes(randomNumber);
            return randomNumber;
        }
    }*/

    public static byte[] HashPassword(byte[] pas, byte[] salt, int numOfIter)
    {
        using (var rfc2898 = new Rfc2898DeriveBytes(pas, salt, numOfIter))
        {
            return rfc2898.GetBytes(20);
        }
    }
}