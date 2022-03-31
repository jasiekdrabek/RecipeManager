using RecipeManager.DataAccess;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace RecipeManager.ViewModel
{
    public class LoginViewModel : BaseViewModel, IDisposable
    {
        private Guid _id;
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        private UserValidation _userValidation;
        private ICommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new RelayCommand(param => CommandLogin(), null);

                return _loginCommand;
            }
        }
        public LoginViewModel()
        {
            _userValidation = new UserValidation();
        }
        public static string Md5(string password)
        {
            using (var md5Hash = MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = md5Hash.ComputeHash(sourceBytes);
                var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                return hash;
            }
        }
        public void CommandLogin()
        {
            if (MainWindow.UnitOfWork.UserRepository.GetByName(Login) != null && _userValidation.ValidateLogin(Login))
            {
                if (Md5(Password) == MainWindow.UnitOfWork.UserRepository.GetByName(Login).Password.Replace(" ", string.Empty)
                    && _userValidation.ValidatePassword(Password))
                {
                    Password = Md5(Password);
                    var window = Application.Current.Windows.OfType<MainWindow>().Take(1).SingleOrDefault();
                    window.GridLogin.Visibility = Visibility.Collapsed;
                    window.GridAfterLogin.Visibility = Visibility.Visible;
                    window.TBPassword.Password = string.Empty;
                    MainWindowViewModel.User = MainWindow.UnitOfWork.UserRepository.GetByName(Login);
                    MainWindowViewModel.Info = "Zalogowano jako: " + Login;
                    ShoppingListModelView.GetAllProducts();
                    RecipeListViewModel.GetRecipes();
                    RecipeListViewModel.GetCategories();
                    Login = string.Empty;
                    Password = string.Empty;
                }
                else
                {
                    MessageBox.Show("Podano błędne hasło");
                }
            }
            else
            {
                MessageBox.Show("Podano błędny login");
            }
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    MainWindow.UnitOfWork.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}