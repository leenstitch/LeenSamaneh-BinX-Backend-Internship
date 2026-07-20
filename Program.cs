// See https://aka.ms/new-console-template for more information
using HelloBinX;

//Console.WriteLine("Leen Samaneh");
//Console.WriteLine("Sun,Jul 19");
class BinX
{
    static void Main(string[] args)
    {
        //======= WEEK1 - DAY1 - TASKES =======
        Console.WriteLine("Leen Samaneh");
        Console.WriteLine("Sun,Jul 19");


        //======= WEEK1 - DAY1 - TASKES =======

        TypeExamples example = new TypeExamples();

        // Method 1: Value types and Reference types
        example.ValueVsReferenceTypes();
        Console.WriteLine();
        // Method 2: Value vs Reference copy behavior
        example.ValueVsReferenceCopyBehavior();
        Console.WriteLine();
        // Method 3 Grade classifier
        Console.Write("Enter your score (It should be between 0 and 100): ");
        int score = int.Parse(Console.ReadLine());
        Console.WriteLine("Your Grade is: "+example.Grade(score));
        Console.WriteLine();
        // Method 4: Nullable values
        example.NullableValuesExample();

        //======= WEEK1 - DAY2 - TASKES =======
    }
}

