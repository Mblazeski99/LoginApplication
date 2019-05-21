using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginApp.Enums
{
    public enum ErrorType
    {
        // Invalid Registration Exception
        UsernameTaken,
        PasswordTaken,
        NonViableAge,
        NonViableUsername,
        NonViablePassword,
        NonViableEmail,
        // Invalid Login Exception
        IncorrectPassword,
        UsernameDoesntExist
    }
}
