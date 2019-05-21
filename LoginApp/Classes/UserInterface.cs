using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoginApp.Enums;
using LoginApp.Exceptions;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace LoginApp.Classes
{
    public class UserInterface
    {
        #region Data Setup
        private string _usersPath = @"C:\Users\marko\Desktop\MyApps\VisualStudio\AdvancedC#\LoginApplication\Data\Users.json";
        private string _usersLogPath = @"C:\Users\marko\Desktop\MyApps\VisualStudio\AdvancedC#\LoginApplication\Data\Log.txt";
        private string _IdCountPath = @"C:\Users\marko\Desktop\MyApps\VisualStudio\AdvancedC#\LoginApplication\Data\IdCount.txt";
        private string _newsFeedPath = @"C:\Users\marko\Desktop\MyApps\VisualStudio\AdvancedC#\LoginApplication\Data\NewsFeed.txt";
        private string _newsFeed;
        private string _postContent;
        private string _logs;
        private string _serializedUsers;
        private int _answer;
        private int _idCount;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _username;
        private string _password;
        private int _age;
        private int _changedAge;
        private string _changedEmail;
        private string _changedUsername;
        private string _changedPassword;
        private bool _alreadyExists = false;
        private List<User> _users;

        public UserInterface()
        {
           
            if (!File.Exists(_usersPath)) File.Create(_usersPath).Close();
            if (!File.Exists(_usersLogPath)) File.Create(_usersLogPath).Close();
            if (!File.Exists(_newsFeedPath))
            {
                File.Create(_newsFeedPath).Close();
                using (StreamWriter sw = new StreamWriter(_newsFeedPath)) sw.WriteLine("----------------------------------------------------------");
            }
            if (!File.Exists(_IdCountPath))
            {
                File.Create(_IdCountPath).Close();
                using (StreamWriter sw = new StreamWriter(_IdCountPath)) sw.WriteLine("1");
            }
            using (StreamReader sr = new StreamReader(_IdCountPath)) _idCount = int.Parse(sr.ReadToEnd());
            using (StreamReader sr = new StreamReader(_usersPath)) _serializedUsers = sr.ReadToEnd();
            using (StreamReader sr = new StreamReader(_usersLogPath)) _logs = sr.ReadToEnd();
            using (StreamReader sr = new StreamReader(_newsFeedPath)) _newsFeed = sr.ReadToEnd();

            _users = JsonConvert.DeserializeObject<List<User>>(_serializedUsers);
        }

        private void AddUser(string username, string firstName, string lastName, string email, string password, int age)
        {
            if (_users == null) _users = new List<User>();
            else
            {
                foreach (User user in _users)
                {
                    if (user.Username == username) throw new InvalidRegistrationInput(ErrorType.UsernameTaken);
                    if (user.Password == password) throw new InvalidRegistrationInput(ErrorType.PasswordTaken);                    
                }
            }
            _users.Add(new User(_idCount, firstName, lastName, email, username, password, age, Role.User));
            _idCount++;
            _serializedUsers = JsonConvert.SerializeObject(_users);
            using (StreamWriter sw = new StreamWriter(_usersPath)) sw.WriteLine(_serializedUsers);
            using (StreamWriter sw = new StreamWriter(_IdCountPath)) sw.WriteLine(_idCount.ToString());
            using (StreamWriter sw = new StreamWriter(_usersLogPath))
            {
                sw.WriteLine($@"{_logs} 
Log Report: New User Registered
Username: {username}
Time: {DateTime.Now.ToLocalTime()}");
            }
        }
        #endregion

        #region Change Settings
        private void ChangeSettings()
        {
            _alreadyExists = false;
            Console.Clear();
            Console.WriteLine($@"What would you like to change?
1) First name: {_firstName}
2) Last name: {_lastName}
3) Age: {_age}
4) E-Mail: {_email}
5) Username: {_username}
6) Password: {_password}

7) Exit");
            if (!int.TryParse(Console.ReadLine(), out _answer) || _answer < 1 || _answer > 7)
            {
                Console.WriteLine("Please enter one of the give options above!");
                Console.ReadKey();
                ChangeSettings();
            }
            else
            {
              switch(_answer)
              {
                    case 1:
                        Console.WriteLine($"Current first name: {_firstName}");
                        Console.WriteLine("Enter new first name:");
                        _firstName = Console.ReadLine().ToString();                       
                        Console.WriteLine($"Done, your new first name is {_firstName}");
                        foreach (User user in _users)
                        {
                            if (user.Username == _username) user.FirstName = _firstName;
                        }                        
                        _serializedUsers = JsonConvert.SerializeObject(_users);
                        using (StreamWriter sw = new StreamWriter(_usersPath)) sw.WriteLine(_serializedUsers);
                        using (StreamWriter sw = new StreamWriter(_usersLogPath))
                        {
                            sw.WriteLine($@"{_logs} 
Log Report: User Changed First Name
Username: {_username}
Time: {DateTime.Now.ToLocalTime()}");
                        }
                        Console.ReadLine();
                        break;
                    case 2:
                        Console.WriteLine($"Current last name: {_lastName}");
                        Console.WriteLine("Enter new last name:");
                        _lastName = Console.ReadLine().ToString();
                        Console.WriteLine($"Done, your new last name is {_lastName}");
                        foreach (User user in _users)
                        {
                            if (user.Username == _username) user.LastName = _lastName;
                        }
                        _serializedUsers = JsonConvert.SerializeObject(_users);
                        using (StreamWriter sw = new StreamWriter(_usersPath)) sw.WriteLine(_serializedUsers);
                        using (StreamWriter sw = new StreamWriter(_usersLogPath))
                        {
                            sw.WriteLine($@"{_logs} 
Log Report: User Changed Last Name
Username: {_username}
Time: {DateTime.Now.ToLocalTime()}");
                        }
                        Console.ReadLine();
                        break;
                    case 3:
                        Console.WriteLine($"Current age: {_age}");
                        Console.WriteLine("Enter new age:");
                        if (int.TryParse(Console.ReadLine(), out _changedAge) && _changedAge >= 0)
                        {
                            Console.WriteLine($"Done, your new age is {_changedAge}");
                            foreach (User user in _users)
                            {
                                if (user.Username == _username) user.Age = _changedAge;
                            }
                            _age = _changedAge;
                            _serializedUsers = JsonConvert.SerializeObject(_users);
                            using (StreamWriter sw = new StreamWriter(_usersPath)) sw.WriteLine(_serializedUsers);
                            using (StreamWriter sw = new StreamWriter(_usersLogPath))
                            {
                                sw.WriteLine($@"{_logs} 
Log Report: User Changed Age
Username: {_username}
Time: {DateTime.Now.ToLocalTime()}");
                            }
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Thats not a viable age!");
                            Console.ReadKey();
                            ChangeSettings();
                        }
                        break;
                    case 4:
                        Console.WriteLine($"Current Email Address: {_email}");
                        Console.WriteLine("Enter new Email Address:");
                        _changedEmail = Console.ReadLine();
                        if (_changedEmail.ViableEmail())
                        {
                            if (_email == _changedEmail)
                            {
                                Console.WriteLine("You already have this email address");
                                Console.ReadKey();
                                ChangeSettings();
                            }

                            foreach (User user in _users)
                            {
                                if (user.Email == _changedEmail) _alreadyExists = true;
                            }

                            if (_alreadyExists)
                            {
                                Console.WriteLine("Sorry, that email address is already taken");
                                Console.ReadKey();
                                ChangeSettings();
                            }
                            else
                            {
                                Console.WriteLine($"Done, your new email address is {_changedEmail}");
                                foreach (User _user in _users)
                                {
                                    if (_user.Username == _username) _user.Email = _changedEmail;
                                }
                                _email = _changedEmail;
                                _serializedUsers = JsonConvert.SerializeObject(_users);
                                using (StreamWriter sw = new StreamWriter(_usersPath)) sw.WriteLine(_serializedUsers);
                                using (StreamWriter sw = new StreamWriter(_usersLogPath))
                                {
                                    sw.WriteLine($@"{_logs} 
Log Report: User Changed Email Address
Username: {_username}
Time: {DateTime.Now.ToLocalTime()}");
                                }
                                Console.ReadLine();
                                _alreadyExists = false;                               
                            }
                        }
                        else
                        {
                            Console.WriteLine("Thats not a valid email address");
                            Console.ReadKey();
                            ChangeSettings();
                        }                                              
                        break;
                    case 5:
                        Console.WriteLine($"Current username: {_username}");
                        Console.WriteLine("Enter new username:");
                        _changedUsername = Console.ReadLine();
                        if (_changedUsername.ViableUsername())
                        {
                            if (_username == _changedUsername)
                            {
                                Console.WriteLine("You already have this username");
                                Console.ReadKey();
                                ChangeSettings();
                            }
                            foreach (User user in _users)
                            {                                
                                if (user.Username == _changedUsername) _alreadyExists = true;
                            }
                            if(_alreadyExists)
                            {
                                Console.WriteLine("Sorry, that username is already taken");
                                Console.ReadKey();
                                ChangeSettings();
                            }
                            else
                            {
                                Console.WriteLine($"Done, your new username is {_changedUsername}");
                                foreach (User _user in _users)
                                {
                                    if (_user.Username == _username) _user.Username = _changedUsername;
                                }
                                _username = _changedUsername;
                                _serializedUsers = JsonConvert.SerializeObject(_users);
                                using (StreamWriter sw = new StreamWriter(_usersPath)) sw.WriteLine(_serializedUsers);
                                using (StreamWriter sw = new StreamWriter(_usersLogPath))
                                {
                                    sw.WriteLine($@"{_logs} 
Log Report: User Changed Username
Username: {_username}
Time: {DateTime.Now.ToLocalTime()}");
                                }
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Thats not a valid username");
                            Console.ReadKey();
                            ChangeSettings();
                        }
                        break;
                    case 6:
                        Console.WriteLine($"Current password: {_password}");
                        Console.WriteLine("Enter new password:");
                        _changedPassword = Console.ReadLine();
                        if (_changedPassword.ViablePassword())
                        {
                            if (_password == _changedPassword)
                            {
                                Console.WriteLine("You already have this password");
                                Console.ReadKey();
                                ChangeSettings();
                            }
                            foreach (User user in _users)
                            {
                                if (user.Password == _changedPassword) _alreadyExists = true;
                            }
                            if (_alreadyExists)
                            {
                                Console.WriteLine("Sorry, that password is already taken");
                                Console.ReadKey();
                                ChangeSettings();
                            }
                            else
                            {
                                Console.WriteLine($"Done, your new password is {_changedPassword}");
                                foreach (User _user in _users)
                                {
                                    if (_user.Username == _username) _user.Password = _changedPassword;
                                }
                                _password = _changedPassword;
                                _serializedUsers = JsonConvert.SerializeObject(_users);
                                using (StreamWriter sw = new StreamWriter(_usersPath)) sw.WriteLine(_serializedUsers);
                                using (StreamWriter sw = new StreamWriter(_usersLogPath))
                                {
                                    sw.WriteLine($@"{_logs} 
Log Report: User Changed Password
Username: {_username}
Time: {DateTime.Now.ToLocalTime()}");
                                }
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Thats not a valid password");
                            Console.ReadKey();
                            ChangeSettings();
                        }
                        break;
              }
            }
            LoggedIn();
        }
        #endregion

        private void AccSettings()
        {
            Console.Clear();
            Console.WriteLine(@"Choose Option:
1) View Account Info
2) Change Account Info
3) Exit");
            if (!int.TryParse(Console.ReadLine(), out _answer) || _answer != 1 && _answer != 2 && _answer != 3)
            {
                Console.WriteLine("Please enter one of the give options above!");
                Console.ReadKey();
                AccSettings();
            }
            else if (_answer == 1)
            {
                Console.Clear();
                Console.WriteLine($@"Account info:
First name: {_firstName}
Last name: {_lastName}
Age: {_age}
E-Mail: {_email}
Username: {_username}
Password: {_password}");
                Console.ReadLine();
                AccSettings();
            }
            else if (_answer == 2) ChangeSettings();
            else if (_answer == 3) LoggedIn();
        }

        private void LoggedIn()
        {
            foreach (User user in _users)
            {
                if (user.Username == _username)
                {
                    _firstName = user.FirstName;
                    _lastName = user.LastName;
                    _age = user.Age;
                    _email = user.Email;
                }
            }
            Console.Clear();
            Console.WriteLine($@"Good Day Mr/Ms {_username}
1) Check news feed
2) Account settings
3) Log out");
            if (!int.TryParse(Console.ReadLine(), out _answer) || _answer != 1 && _answer != 2 && _answer != 3)
            {
                Console.WriteLine("Please enter one of the give options above!");
                Console.ReadKey();
                LoggedIn();
            }
            else if (_answer == 1) ShowNewsFeed();
            else if (_answer == 2) AccSettings();
            else if (_answer == 3) ChooseOptions();
        }

        private void Login()
        {
            bool found = false;
            Console.WriteLine("Enter Your Username:");
            _username = Console.ReadLine();
            Console.WriteLine("Enter Your Password:");
            _password = Console.ReadLine();
            if (_users == null) _users = new List<User>();
            foreach (User user in _users)
            {
                if (user.Username == _username)
                {
                    if (user.Password == _password)
                    {
                        found = true;
                        Console.WriteLine("Loging in....");
                        Thread.Sleep(2000);
                        using (StreamWriter sw = new StreamWriter(_usersLogPath))
                        {
                            sw.WriteLine($@"{_logs} 
Log Report: User Logged In
Username: {_username}
Time: {DateTime.Now.ToLocalTime()}");
                        }
                        LoggedIn();
                    }
                    else throw new InvalidLoginException(ErrorType.IncorrectPassword);
                }
            }
            if(!found) throw new InvalidLoginException(ErrorType.UsernameDoesntExist);
        }

        private void Register()
        {
            Console.WriteLine("Enter Your First Name:");
            _firstName = Console.ReadLine().ToString();
            Console.WriteLine("Enter Your Last Name:");
            _lastName = Console.ReadLine().ToString();
            Console.WriteLine("Enter Your Email Address(EX: James322@gmail.com):");
            _email = Console.ReadLine().ToString();
            Console.WriteLine("Enter a username(Must be atleast 3 characters long and cannot start with a number):");
            _username = Console.ReadLine();
            Console.WriteLine("Enter a password(must be at least 6 characters):");
            _password = Console.ReadLine().ToString();
            Console.WriteLine("Enter your age:");
            if (int.TryParse(Console.ReadLine(), out _age) && _age >= 0) ;
            else throw new InvalidRegistrationInput(ErrorType.NonViableAge); 
            CheckInputs();
            AddUser(_username, _firstName, _lastName, _email, _password, _age);            

            Console.WriteLine("Registering....");
            Thread.Sleep(2000);
            LoggedIn();
        }

        private void ShowNewsFeed()
        {
            Console.Clear();
            Console.WriteLine(@"1) Post Something
2) Exit");
            Console.WriteLine(_newsFeed);
            if (int.TryParse(Console.ReadLine(), out _answer) && _answer > 0 && _answer < 3)
            {
                if (_answer == 1) WritePost();
                else if (_answer == 2) LoggedIn();                
            }
            else
            {
                Console.WriteLine("Please enter one of the options above");
                Console.ReadKey();
                ShowNewsFeed();
            }
            Console.ReadKey();
        }

        private void WritePost()
        {
            Console.Clear();
            Console.WriteLine("Write your post:");
            _postContent = Console.ReadLine();

            if (_postContent.Trim() == "")
            {
                Console.WriteLine(@"You didn't write anything
1) Try Again");
                if (int.TryParse(Console.ReadLine(), out _answer) && _answer == 1) WritePost();
                else ShowNewsFeed();

                Console.ReadKey();
                WritePost();
            } else
            {
                _newsFeed += $@"{_username}
{DateTime.Now.Day} {DateTime.Now.ToString("MMMM")} at {DateTime.Now.Hour}:{DateTime.Now.Minute}
{_postContent}
----------------------------------------------------------";
                using (StreamWriter sw = new StreamWriter(_newsFeedPath)) sw.WriteLine(_newsFeed);
                using (StreamWriter sw = new StreamWriter(_usersLogPath)) sw.WriteLine(_logs + $@"
Log Report: User Posted
Username: {_username}
Time: {DateTime.Now.ToLocalTime()}");
            }
            Console.WriteLine("Done Your Post Was Made");
            Console.ReadKey();
            ShowNewsFeed();
        }

        public void ChooseOptions()
        {
            Console.Clear();
            Console.WriteLine(@"Choose Option:
1) Log In
2) Register
3) Exit Application");
            if (int.TryParse(Console.ReadLine(), out _answer))
            {
                if (_answer == 1) Login();
                else if (_answer == 2) Register();
                else if (_answer == 3) System.Environment.Exit(0);
                else throw new InvalidChoiceInputException();
            }
            else throw new InvalidChoiceInputException();
        }

        private void CheckInputs()
        {
            if (!_username.ViableUsername()) throw new InvalidRegistrationInput(ErrorType.NonViableUsername);
            if (!_password.ViablePassword()) throw new InvalidRegistrationInput(ErrorType.NonViablePassword);
            if (!_email.ViableEmail()) throw new InvalidRegistrationInput(ErrorType.NonViableEmail);
        }
    }

    #region Extension Methods
    public static class StringExtensionMethods
    {
        public static bool ViableUsername(this string username)
        {
            if(username.Length < 3) return false;  
            return true;
        }

        public static bool ViablePassword(this string pass)
        {
            if (pass.Length < 6) return false;
            return true;
        }     

        public static bool ViableEmail(this string email)
        {
            if (!email.Contains("@")) return false;
            if (!email.Contains("mail.com")) return false;
            if (email.Substring(0, email.IndexOf("@")).Trim() == "") return false;
            return true;
        }
    }
    #endregion
}