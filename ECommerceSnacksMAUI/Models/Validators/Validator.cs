using System.Text.RegularExpressions;

namespace ECommerceSnacksMAUI.Models.Validators
{
    public class Validator : IValidator
    {
        public string NameError { get; set; } = "";
        public string EmailError { get; set; } = "";
        public string PhoneError { get; set; } = "";
        public string PasswordError { get; set; } = "";

        private const string EmptyNameErrorMsg = "Please, enter a name.";
        private const string InvalidNameErrorMsg = "Please, enter a valid name.";
        private const string EmptyEmailErrorMsg = "Please, enter an email.";
        private const string InvalidEmailErrorMsg = "Please, enter a valid email.";
        private const string EmptyPhoneErrorMsg = "Please, enter a phone number";
        private const string InvalidPhoneErrorMsg = "Please, enter a valid phone number.";
        private const string EmptyPasswordErrorMsg = "Please, enter a password.";
        private const string InvalidPasswordErrorMsg = "Password must be at least 8 characters long and include both letters and numbers.";

        public Task<bool> Validate(string name, string email, string phoneNumber, string password)
        {
            var isNameValid = ValidateName(name);
            var isEmailValid = ValidateEmail(email);
            var isPhoneValid = ValidatePhoneNumber(phoneNumber);
            var isPasswordValid = ValidatePassword(password);

            return Task.FromResult(isNameValid && isEmailValid && isPhoneValid && isPasswordValid);
        }

        private bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                NameError = EmptyNameErrorMsg;
                return false;
            }

            if (name.Length < 3)
            {
                NameError = InvalidNameErrorMsg;
                return false;
            }

            NameError = "";
            return true;
        }

        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                EmailError = EmptyEmailErrorMsg;
                return false;
            }

            if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                EmailError = InvalidEmailErrorMsg;
                return false;
            }

            EmailError = "";
            return true;
        }

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                PhoneError = EmptyPhoneErrorMsg;
                return false;
            }

            if (phoneNumber.Length < 12)
            {
                PhoneError = InvalidPhoneErrorMsg;
                return false;
            }

            PhoneError = "";
            return true;
        }

        private bool ValidatePassword(string senha)
        {
            if (string.IsNullOrEmpty(senha))
            {
                PasswordError = EmptyPasswordErrorMsg;
                return false;
            }

            if (senha.Length < 8 || !Regex.IsMatch(senha, @"[a-zA-Z]") || !Regex.IsMatch(senha, @"\d"))
            {
                PasswordError = InvalidPasswordErrorMsg;
                return false;
            }

            PasswordError = "";
            return true;
        }
    }
}
