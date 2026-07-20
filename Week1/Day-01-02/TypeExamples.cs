using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// week1======Day2
namespace HelloBinX
{
    public class TypeExamples
    {

        // Define and print value and reference types variables
        public void ValueVsReferenceTypes()
        {
            // Define value types variables
            double salary = 100.50;
            int age = 20;
            bool isHeWorking = true;
            //print value types variables
            Console.WriteLine("print value types variables");
            Console.WriteLine("First value type:  "+salary.GetType());
            Console.WriteLine("Second value type:  " + age.GetType());
            Console.WriteLine("Third value type:  " + isHeWorking.GetType());
            Console.WriteLine();

            // Define  reference types variables
            string message = "I love this internship";
            int[] numbers = { 1, 2, 3, 4, 5 };
            List<char> names = new List<char> { 'L', 'M', 'N', 'R' };
            //print reference types variables
            Console.WriteLine("print reference types variables");
            Console.WriteLine("First reference type:  " + message.GetType());
            Console.WriteLine("Second reference type:  " + numbers.GetType());
            Console.WriteLine("Third reference type:  " + names.GetType());
        }
        public void ValueVsReferenceCopyBehavior()
        {
            // Value type copy example
            Console.WriteLine("Value type copy example");
            int number1 = 10;
            int number2 = number1; // Copying value type
            Console.WriteLine("Befor any change:  " + "Number1=  " + number1 + "  " + "Number2=  " + number2); 
           
            number2 = 20;
            
            Console.WriteLine("After the copy:  "+"Number1=  " + number1 + "  "+ "Number2=  "+number2); 
            Console.WriteLine();

            // Reference type copy example
            int[] array1 = { 1, 2, 3 };
            int[] array2 = array1; // Copying reference type
            Console.WriteLine("Befor any change:  " + "Array1[0]=  " + array1[0] + "  " + "Array2[0]=  " + array2[0]); 
           
            array2[0] = 10;
          
            Console.WriteLine("After the copy:  " + "Array1[0]=  " + array1[0] + "  " + "Array2[0]=  " +array2[0]);
        }

        //grade-classifier method using a switch expression
        public string Grade(int score)
        {
            if (score < 0 || score > 100) // Check if the score is outside the valid range (0-100)
            {
                return "Invalid score";
            }
            return score switch
            {
            >= 90 => "A", // If score is greater than or equal to 90, return "A"
            >= 80 => "B", // If score is greater than or equal to 80, return "B"
            >= 70 => "C", // If score is greater than or equal to 70, return "C"
            >= 60 => "D", // If score is greater than or equal to 60, return "D"
            _ => "F"      // If score is less than 60, return "F"
              };
        }
        // Nullable values example to handles a possibly-null value
        public void NullableValuesExample()
        {
            // Take user input and if it is not a valid integer, assign null to the age variable else assign the parsed integer value to the age variable
            Console.WriteLine("Enter your age(It should be a positive Number): ");
            int? age = int.TryParse(Console.ReadLine(), out int result) ? result : (int?)null;

            // Check if the age variable has a value and if it is greater than or equal to 0
            if (age.HasValue && age.Value >= 0)
            {
                Console.WriteLine($"The value is: {age.Value}");
            }

            // Check if the age variable has a value and if it is less than 0
            else if (age.HasValue && age.Value < 0)
            {
                Console.WriteLine("The value is negative.");
            }
            // Check if the age variable is null
            else
            {
                Console.WriteLine("The value is null.");
            }
            
        }

    }
}