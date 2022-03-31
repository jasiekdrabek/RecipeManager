using RecipeManager.DataAccess;
using RecipeManager.Model;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace RecipeManager.ViewModel
{
    public class RegistrationViewModel : BaseViewModel, IDisposable
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
        private ICommand _registrationCommand;
        public ICommand RegistrationCommand
        {
            get
            {
                if (_registrationCommand == null)
                    _registrationCommand = new RelayCommand(param => CommandRegistration(), null);

                return _registrationCommand;
            }
        }
        private UserValidation _userValidation;
        public RegistrationViewModel()
        {
            _userValidation = new UserValidation();
        }
        public void CommandRegistration()
        {
            if (_userValidation.ValidatePassword(Password))
            {
                if (MainWindow.UnitOfWork.UserRepository.GetByName(Login) == null && _userValidation.ValidateLogin(Login))
                {
                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        Login = Login,
                        Password = LoginViewModel.Md5(Password)
                    };
                    MainWindow.UnitOfWork.UserRepository.AddUser(user);
                    MainWindow.UnitOfWork.Save();
                    if (!Directory.Exists(user.Login))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(user.Login);
                    }
                    MessageBox.Show("Pomyślnie dodano nowe konto");
                    Login = string.Empty;
                    Password = string.Empty;
                }
                else
                {
                    MessageBox.Show("Użytkownik o takim loginie już istnieje");
                }
            }
            else
            {
                MessageBox.Show("Hasło musi zawierać wielką literę, małą literę orez cyfrę");
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