//Написати програму для реєстрації користувача за логіном/паролем та 
//авторизації шляхом співставлення відповідних логінів і паролів.
//Зберігання пароля у відкритому вигляді неприпустиме.

using System.Security.Cryptography;
using System.Text;

class Log
{
    public static void Main()
    {
        int users_am = 0; //Amount of signed up users
        string[] logins = new string[100]; //Array signed up logins
        string[] passwords = new string[100]; //Array of signed up passwords

        while(true)
        {
            Console.WriteLine("");
            Console.WriteLine("Greetings!");
            Console.WriteLine("1 -> Create a new user");
            Console.WriteLine("2 -> Log in");
            Console.WriteLine("3 -> Exit program");
            Console.Write("Please, enter option to continue: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");

            switch (choice)
            {
                case 1:
                    Console.Write("Create login: ");
                    string login = Console.ReadLine();
                    Console.Write("Create password: ");
                    string password = "";
                    while (true)
                    {
                        var key = Console.ReadKey(true); //True is used here not to display our input in console
                        if (key.Key == ConsoleKey.Enter)
                            break;
                        password += key.KeyChar;
                    }
                    byte[] loginByteArr = Encoding.Unicode.GetBytes(login);
                    byte[] passwordByteArr = Encoding.Unicode.GetBytes(password);
                    var HMACSHA512ForLogin = ComputeHMACSHA512(loginByteArr, passwordByteArr);
                    var HMACSHA512ForPas = ComputeHMACSHA512(passwordByteArr, loginByteArr);
                    logins[users_am] = Convert.ToBase64String(HMACSHA512ForLogin); //Recording HMACs in logins and passwords arrays (this will prevent from seeing clear text)
                    passwords[users_am] = Convert.ToBase64String(HMACSHA512ForPas);
                    users_am++;
                    Console.WriteLine($"\n\nThe user {login} was created. Use option 2 to verify this.");
                    Console.WriteLine($"Current amount of signed up users: {users_am}");
                    Console.WriteLine("-------------------------------------");
                    break;

                case 2:
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
                    byte[] realLoginByteArr = Encoding.Unicode.GetBytes(realLogin);
                    byte[] realPasswordByteArr = Encoding.Unicode.GetBytes(realPassword);
                    var HMACSHA512ForRealLogin = ComputeHMACSHA512(realLoginByteArr, realPasswordByteArr);
                    var HMACSHA512ForRealPas = ComputeHMACSHA512(realPasswordByteArr, realLoginByteArr);
                    int indexLogin = Array.IndexOf(logins, Convert.ToBase64String(HMACSHA512ForRealLogin));
                    int indexPas = Array.IndexOf(passwords, Convert.ToBase64String(HMACSHA512ForRealPas));
                    if (logins.Contains(Convert.ToBase64String(HMACSHA512ForRealLogin)) && passwords.Contains(Convert.ToBase64String(HMACSHA512ForRealPas)) && indexLogin == indexPas)
                    {
                        Console.WriteLine("\n\nYou were successfully logged in!");
                        Console.WriteLine("-------------------------------------");
                    }
                    else
                    {
                        Console.WriteLine($"\n\nUser with login {realLogin} can't be found. Please, try again.");
                        Console.WriteLine("-------------------------------------");
                        break;
                    }
                    break;

                case 3:
                    Environment.Exit(0);
                    break;
            }
        }

        static byte[] ComputeHMACSHA512(byte[] loginByteArr, byte[] passwordByteArr)
        {
            using (var hmacsha512 = new HMACSHA512(passwordByteArr))
            {
                return hmacsha512.ComputeHash(loginByteArr);
            }
        }
    }
}