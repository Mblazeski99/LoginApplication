using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LoginApp.Classes;
using LoginApp.Exceptions;

namespace LoginApp
{
    class Program
    {
        static void Main(string[] args)
        {           
            var ui = new UserInterface();
            string answer;
            bool stop = false;
            bool errorPoped = false;

            while (!stop)
            {
                errorPoped = false;
                try
                {
                    ui.ChooseOptions();
                }
                catch (InvalidChoiceInputException ex)
                {
                    errorPoped = true;
                    ex.PrintError();
                }
                catch (InvalidLoginException ex)
                {
                    errorPoped = true;
                    ex.PrintError();
                }
                catch (InvalidRegistrationInput ex)
                {
                    errorPoped = true;
                    ex.PrintError();
                }
                finally
                {
                    if(errorPoped)
                    {
                        Console.WriteLine("Try again? (Y/N)");
                        answer = Console.ReadLine();
                        if (answer.ToLower() == "y") stop = false;
                        else stop = true;
                    }                   
                }
            }
        }
    }    
}