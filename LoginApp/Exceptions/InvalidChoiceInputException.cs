using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginApp.Exceptions
{
    public class InvalidChoiceInputException : Exception
    {
        public void PrintError()
        {
            Console.WriteLine("INVALID ANSWER!!! you need to choose option 1, 2 or 3. Try again");
        }
    }
}
