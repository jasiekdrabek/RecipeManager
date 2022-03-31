using RecipeManager.DataAccess;
using RecipeManager.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public UnitOfWork UnitOfWork = new UnitOfWork();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).LoginViewModel.Password = ((PasswordBox)sender).Password; }
        }
        public static void ResetTBText()
        {
            var window = Application.Current.Windows.OfType<MainWindow>().Take(1).SingleOrDefault();
            window.TBOldPassword.Text = string.Empty;
            window.TBNewPassword.Text = string.Empty;
            window.TBProduct.Text = string.Empty;
            window.TBProductQuantity.Text = string.Empty;
            window.TBSearch.Text = string.Empty;
        }
    }
}
