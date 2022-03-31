using RecipeManager.DataAccess;
using RecipeManager.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace RecipeManager.ViewModel
{
    public class RecipeViewModel : BaseViewModel
    {
        private bool _visibilityEdit;
        public bool VisibilityEdit
        {
            get => _visibilityEdit;
            set
            {
                _visibilityEdit = value;
                OnPropertyChanged("VisibilityEdit");
            }
        }
        private bool _visibilitySave;
        public bool VisibilitySave
        {
            get => _visibilitySave;
            set
            {
                _visibilitySave = value;
                OnPropertyChanged("VisibilitySave");
            }
        }
        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;
                OnPropertyChanged("IsReadOnly");
            }
        }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private string _shortDescription;
        public string ShortDescription
        {
            get => _shortDescription;
            set
            {
                _shortDescription = value;
                OnPropertyChanged("ShortDescription");
            }
        }
        private string _introduction;
        public string Introduction
        {
            get => _introduction;
            set
            {
                _introduction = value;
                OnPropertyChanged("Introduction");
            }
        }
        private ProductValidation _productValidation;
        private string _preparation;
        public string Preparation
        {
            get => _preparation;
            set
            {
                _preparation = value;
                OnPropertyChanged("Preparation");
            }
        }
        private string _ingredients;
        public string Ingredients
        {
            get => _ingredients;
            set
            {
                _ingredients = value;
                OnPropertyChanged("Ingredients");
            }
        }
        private string _categories;
        public string Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged("Categories");
            }
        }
        private string _link;
        public string Link
        {
            get => _link;
            set
            {
                _link = value;
                OnPropertyChanged("Link");
            }
        }
        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                _isFavorite = value;
                OnPropertyChanged("IsFavorite");
            }
        }
        private ObservableCollection<string> _ingredientsLV;
        public ObservableCollection<string> IngredientsLV
        {
            get => _ingredientsLV;
            set
            {
                _ingredientsLV = value;
                OnPropertyChanged("IngredientsLV");
            }
        }
        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new RelayCommand(param => CommandEdit(), null);

                return _editCommand;
            }
        }
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(param => CommandSave(), null);

                return _saveCommand;
            }
        }
        private ICommand _addToShoppingListCommand;
        public ICommand AddToShoppingListCommand
        {
            get
            {
                if (_addToShoppingListCommand == null)
                    _addToShoppingListCommand = new RelayCommand(param => CommandAddToShoppingList((string)param), null);

                return _addToShoppingListCommand;
            }
        }
        private string oldRrecipeName = "";
        public RecipeViewModel(string recipeName = null)
        {
            _productValidation = new ProductValidation();
            IngredientsLV = new ObservableCollection<string>();
            if (recipeName == null)
            {
                IsFavorite = false;
                IsReadOnly = false;
                VisibilityEdit = true;
                VisibilitySave = false;
            }
            else
            {
                IsReadOnly = true;
                VisibilityEdit = false;
                VisibilitySave = true;
                oldRrecipeName = recipeName;
                var recipe = MainWindow.UnitOfWork.RecipeRepository.GetRecipe(recipeName);
                Name = recipe.Name;
                ShortDescription = recipe.ShortDescription;
                Introduction = recipe.Introduction;
                Preparation = recipe.Preparation;
                IsFavorite = recipe.IsFavorite;
                Link = recipe.Link;
                Ingredients = "";
                foreach (var ingridient in recipe.Ingredients)
                {
                    Ingredients += ingridient + "\r\n";
                    IngredientsLV.Add(ingridient);
                }
                Categories = "";
                foreach (var category in recipe.Categories)
                {
                    Categories += category + "\r\n";
                }
            }
        }
        public void CommandEdit()
        {
            IsReadOnly = false;
            VisibilityEdit = true;
            VisibilitySave = false;
        }
        public void CommandSave()
        {
            if (Name == "")
            {
                MessageBox.Show("Pole nazwa nie może być puste");
                return;
            }
            IngredientsLV.Clear();
            var ingridientslist = MainWindow.UnitOfWork.RecipeRepository.GetListStringFromMultilineString(Ingredients);
            foreach (var ingridient in ingridientslist)
            {
                IngredientsLV.Add(ingridient);
            }
            var categorieslist = MainWindow.UnitOfWork.RecipeRepository.GetListStringFromMultilineString(Categories);
            var recipe = new Recipe
            {
                Name = Name,
                ShortDescription = ShortDescription,
                Introduction = Introduction,
                Preparation = Preparation,
                Ingredients = ingridientslist,
                Categories = categorieslist,
                Link = Link,
                IsFavorite = IsFavorite
            };
            if (oldRrecipeName == Name)
            {
                MainWindow.UnitOfWork.RecipeRepository.AddRecipe(recipe);
            }
            else
            {
                if (MainWindow.UnitOfWork.RecipeRepository.GetAllRecipiesNames().Contains(Name))
                {
                    MessageBoxButton button = MessageBoxButton.YesNo;
                    string messageBoxText = "Przepis o tej nazwie już istnieje czy na pewno chcesz dodać ten przepis (stary przepis zostanie usunięty)";
                    MessageBoxResult result = MessageBox.Show(messageBoxText, null, button);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            MainWindow.UnitOfWork.RecipeRepository.AddRecipe(recipe);
                            MessageBox.Show("Pomyślnie dodano nowy przepis");
                            break;
                        case MessageBoxResult.No:
                            return;
                    }
                }
                else
                {
                    if (oldRrecipeName != "")
                    {
                        MainWindow.UnitOfWork.RecipeRepository.DeleteRecipe(MainWindow.UnitOfWork.RecipeRepository.GetRecipe(oldRrecipeName));
                    }
                    MainWindow.UnitOfWork.RecipeRepository.AddRecipe(recipe);
                    if (oldRrecipeName == "")
                    {
                        MessageBox.Show("Pomyślnie dodano nowy przepis");
                    }
                }
            }
            IsReadOnly = true;
            VisibilityEdit = false;
            VisibilitySave = true;
            RecipeListViewModel.GetRecipes();
            RecipeListViewModel.GetCategories();
        }
        public void CommandAddToShoppingList(string name)
        {
            if (_productValidation.ValidateName(name, MainWindowViewModel.User.Id))
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Quantity = 1,
                    UserId = MainWindowViewModel.User.Id
                };
                MainWindow.UnitOfWork.ProductRepository.AddProduct(product);
                MainWindow.UnitOfWork.Save();
                ShoppingListModelView.GetAllProducts();
            }
            else
            {
                MessageBox.Show("Podana nazwa ma nieodpowiednią długość lub produkt o tej nazwie już jest w liście zakupów");
            }
        }
    }
}
