using System.Text.RegularExpressions;

namespace RecipeManager.DataAccess
{
    public class UserValidation
    {
        public bool ValidatePassword(string password)
        {
            if (password == null)
            {
                return false;
            }
            Regex smallLetters = new Regex("[a-z]");
            Regex bigLetters = new Regex("[A-Z]");
            Regex numbers = new Regex("[0-9]");
            var condition1 = smallLetters.Matches(password).Count;
            var condition3 = bigLetters.Matches(password).Count;
            var condition2 = numbers.Matches(password).Count;
            if (condition1 >= 1 && condition2 >= 1 && condition3 >= 1 && password.Length < 40 && password.Length > 0)
            {
                return true;
            }
            return false;
        }
        public bool ValidateLogin(string login)
        {
            if(login == null)
            {
                return false;
            }
            if (login.Length > 0 && login.Length < 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
