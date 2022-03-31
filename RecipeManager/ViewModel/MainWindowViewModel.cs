using RecipeManager.Model;
using System.ComponentModel;

namespace RecipeManager.ViewModel
{
    public class MainWindowViewModel 
    {
        private static User _user;
        public static User User 
        { 
            get=>_user;
            set
            {
                _user = value;
                StaticPropertyChanged?.Invoke(null, UserPropertyEventArgs);
            } 
        }
        private static string _info;
        public static string Info
        {
            get => _info;

            set
            {
                _info = value;
                StaticPropertyChanged?.Invoke(null, InfoPropertyEventArgs);
            }
        }
        public static event PropertyChangedEventHandler StaticPropertyChanged;
        private static readonly PropertyChangedEventArgs InfoPropertyEventArgs = new PropertyChangedEventArgs(nameof(Info));
        private static readonly PropertyChangedEventArgs UserPropertyEventArgs = new PropertyChangedEventArgs(nameof(User));
        public LoginViewModel  LoginViewModel {get; set; }
        public RegistrationViewModel RegistrationViewModel { get; set; }
        public UserInfoViewModel UserInfoViewModel { get; set; }
        public ShoppingListModelView ShoppingListModelView { get; set; }
        public RecipeListViewModel RecipeListViewModel { get; set; }
        public MainWindowViewModel()
        {
            LoginViewModel = new LoginViewModel();
            RegistrationViewModel = new RegistrationViewModel();
            UserInfoViewModel = new UserInfoViewModel();
            ShoppingListModelView = new ShoppingListModelView();
            RecipeListViewModel = new RecipeListViewModel();
        }
    }
}