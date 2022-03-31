using RecipeManager.DataAccess;
using RecipeManager.Model;
using RecipeManager.View;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RecipeManager.ViewModel
{
    public class UserInfoViewModel : BaseViewModel, IDisposable
    {
        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;
                OnPropertyChanged("OldPassword");
            }
        }
        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }
        private UserValidation _userValidation;
        private ICommand _logoutCommand;
        public ICommand LogoutCommand {
            get
            {
                if (_logoutCommand == null)
                    _logoutCommand = new RelayCommand(param => CommandLogout(), null);

                return _logoutCommand;
            }
        }
        private ICommand _changePasswordCommand;
        public ICommand ChangePasswordCommand
        {
            get
            {
                if (_changePasswordCommand == null)
                    _changePasswordCommand = new RelayCommand(param => CommandChangePassword(), null);

                return _changePasswordCommand;
            }
        }
        private ICommand _deleteUserCommand;
        public ICommand DeleteUserCommand
        {
            get
            {
                if (_deleteUserCommand == null)
                    _deleteUserCommand = new RelayCommand(param => CommandDeleteUser(), null);

                return _deleteUserCommand;
            }
        }
        public UserInfoViewModel()
        {
            _userValidation = new UserValidation();
        }
        public void CommandLogout()
        {
            MainWindowViewModel.User = null;
            var windows = Application.Current.Windows.OfType<RecipeWindow>().ToList();
            if (windows != null)
            {
                foreach (var window1 in windows)
                {
                    window1.Close();
                }
            }
            var window = Application.Current.Windows.OfType<MainWindow>().Take(1).SingleOrDefault();
            window.GridLogin.Visibility = Visibility.Visible;
            window.GridAfterLogin.Visibility = Visibility.Collapsed;
            MainWindow.ResetTBText();
        }
        public void CommandChangePassword()
        {
            var x = LoginViewModel.Md5(NewPassword);
            if (LoginViewModel.Md5(OldPassword) == MainWindowViewModel.User.Password.Replace(" ",string.Empty))
            {
                if (_userValidation.ValidatePassword(NewPassword))
                {
                    var user = new User
                    {
                        Id = MainWindowViewModel.User.Id,
                        Login = MainWindowViewModel.User.Login,
                        Password = LoginViewModel.Md5(NewPassword)
                    };
                    MainWindow.UnitOfWork.UserRepository.UpdateUser(user);
                    MainWindow.UnitOfWork.Save();
                    MessageBox.Show("Pomyślnie zmieniono hasło");
                }
                else
                {
                    MessageBox.Show("Nowe hasło musi zawierać wielką litere, małą literę oraz cyfrę");
                }
            }
            else
            {
                MessageBox.Show("Podano błędne hasło");
            }
        }
        public void CommandDeleteUser()
        {
            MessageBoxButton button = MessageBoxButton.YesNo;
            string messageBoxText = "Czy na pewno chcesz usunąć konto?";
            MessageBoxResult result = MessageBox.Show(messageBoxText, null, button);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    MainWindow.UnitOfWork.UserRepository.RemoveUser(MainWindowViewModel.User.Id);
                    MainWindow.UnitOfWork.Save();
                    if (Directory.Exists(MainWindowViewModel.User.Login))
                    {
                        Directory.Delete(MainWindowViewModel.User.Login, true);
                    }
                    MessageBox.Show("Pomyślnie usunięto konto");
                    CommandLogout();
                    break;
                case MessageBoxResult.No:
                    break;
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
