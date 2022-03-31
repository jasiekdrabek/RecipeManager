using RecipeManager.ViewModel;
using System.Windows;

namespace RecipeManager.View
{
    /// <summary>
    /// Interaction logic for RecipeWindow.xaml
    /// </summary>
    public partial class RecipeWindow : Window
    {
        public RecipeWindow(string recipeName = null)
        {
            InitializeComponent();
            this.DataContext = new RecipeViewModel(recipeName);
        }
    }
}
