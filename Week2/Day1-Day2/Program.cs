using LibrarySystem.Models;
using LibrarySystem.Week2;


namespace LibrarySystem
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Day1.Week2Day1();


            Day2.Week2Day2();

            await Day3.Week2Day3();

            Console.WriteLine("\nFinished");
        }
    }
}