using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.IO
{
    public static class ConsoleExtensions
    {
        public static string RequestString(this string message)
        {
            string? output = string.Empty;
            Console.Write(message);
            output = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(output))
            {
                Console.Write(message);
                output=Console.ReadLine();
            }

            return output;
        }

        public static int RequestInteger(this string message)
        {
            int output = 0;
            Console.Write(message);
            bool isValidInt = int.TryParse(Console.ReadLine(), out output);

            while(!isValidInt)
            {
                Console.WriteLine(message);
                isValidInt = int.TryParse(Console.ReadLine(),out output);
            }

            return output;
        }
    }
}
