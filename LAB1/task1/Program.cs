//Generating random numbers
using System;

public class Generator
{
    static void Main(string[] args)
    {
        Console.WriteLine("15 numbers were generated for you:");
        Console.WriteLine(" ");

        Random rand = new Random();

        for (int i = 0; i < 15; i++)
        {
            Console.WriteLine("{0, 3}", rand.Next(-1000, 1000));
        }

        Console.WriteLine(" ");
        Console.WriteLine("That's it!");
    }
}