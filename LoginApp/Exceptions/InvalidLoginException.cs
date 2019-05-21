using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginApp.Enums;
using System.IO;

namespace LoginApp.Exceptions
{   
    public class InvalidLoginException : Exception
    {
        private string _logs;
        private string _usersLogPath = @"C:\Users\marko\Desktop\MyApps\VisualStudio\AdvancedC#\LoginApp\Data\Log.txt";

        private ErrorType Type { get; set; }

        public InvalidLoginException(ErrorType type)
        {
            Type = type;
        }

        public void PrintError()
        {
            if (Type == ErrorType.IncorrectPassword) Console.WriteLine("The password you entered is incorrect!");
            else if (Type == ErrorType.UsernameDoesntExist) Console.WriteLine("The username you entered does not exist");

            using (StreamReader sr = new StreamReader(_usersLogPath)) _logs = sr.ReadToEnd();
            using (StreamWriter sw = new StreamWriter(_usersLogPath)) sw.WriteLine($@"{_logs}
Log Report: Failed log in
Time: {DateTime.Now.ToLocalTime()}
Error Type: {Type}");
        }
    }
}