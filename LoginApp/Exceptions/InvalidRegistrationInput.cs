using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginApp.Enums;
using System.IO;

namespace LoginApp.Exceptions
{
    public class InvalidRegistrationInput : Exception
    {
        private string _logs;
        private string _usersLogPath = @"C:\Users\marko\Desktop\MyApps\VisualStudio\AdvancedC#\LoginApp\Data\Log.txt";

        private ErrorType Type { get; set; }

        public InvalidRegistrationInput(ErrorType type)
        {
            Type = type;
        }

        public void PrintError()
        {
           switch(Type)
            {
                case ErrorType.UsernameTaken:
                    Console.WriteLine("The username you entered is already taken!");
                    break;
                case ErrorType.PasswordTaken:
                    Console.WriteLine("The password you enterred is already taken!");
                    break;
                case ErrorType.NonViableAge:
                    Console.WriteLine("Age must be a number and cannot be lower than 0!");
                    break;
                case ErrorType.NonViableUsername:
                    Console.WriteLine("The username must be at least 3 characters long and cannot start with a number!");
                    break;
                case ErrorType.NonViablePassword:
                    Console.WriteLine("The password must be at least 6 characters long");
                    break;
                case ErrorType.NonViableEmail:
                    Console.WriteLine("The email you entered is not a valid email try again. EXAMPLE: James322@gmail.com");
                    break; 
            }

            using(StreamReader sr = new StreamReader(_usersLogPath)) _logs = sr.ReadToEnd();
            using (StreamWriter sw = new StreamWriter(_usersLogPath)) sw.WriteLine($@"{_logs} 
Log Report: Failed Registration
Time: {DateTime.Now.ToLocalTime()}
Error Type: {Type}");
        }        
    }
}