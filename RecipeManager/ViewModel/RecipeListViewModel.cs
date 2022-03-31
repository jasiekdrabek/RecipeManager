using RecipeManager.Model;
using RecipeManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RecipeManager.ViewModel
{
    public class RecipeListViewModel : BaseViewModel, IDisposable
    {
        private string _searchingPhrase;
        public string SearchingPhrase
        {
            get => _searchingPhrase;
            set
            {
                _searchingPhrase = value;
                OnPropertyChanged("SearchingPhrase");
            }
        }
        private string _nodeCategory;
        public string NodeCategory
        {
            get => _nodeCategory;
            set
            {
                _nodeCategory = value;
                OnPropertyChanged("NodeCategory");
            }
        }
        private static ObservableCollection<Recipe> _recipesNames;
        public static ObservableCollection<Recipe> RecipesNames
        {
            get => _recipesNames;
            set
            {
                _recipesNames = value;
                StaticPropertyChanged?.Invoke(null, recipesNamesPropertyEventArgs);
            }
        }
        private static ObservableCollection<string> _categoriesNames;
        public static ObservableCollection<string> CategoriesNames
        {
            get => _categoriesNames;
            set
            {
                _categoriesNames = value;
                StaticPropertyChanged?.Invoke(null, categoriesNamesPropertyEventArgs);
            }
        }
        public static event PropertyChangedEventHandler StaticPropertyChanged;
        private static readonly PropertyChangedEventArgs recipesNamesPropertyEventArgs = new PropertyChangedEventArgs(nameof(RecipesNames));
        private static readonly PropertyChangedEventArgs categoriesNamesPropertyEventArgs = new PropertyChangedEventArgs(nameof(CategoriesNames));
        private ICommand _addNewRecipeCommand;
        public ICommand AddNewRecipeCommand
        {
            get
            {
                if (_addNewRecipeCommand == null)
                    _addNewRecipeCommand = new RelayCommand(param => CommandAddNewRecipe(), null);

                return _addNewRecipeCommand;
            }
        }
        private ICommand _addToFavoriteCommand;
        public ICommand AddToFavoriteCommand
        {
            get
            {
                if (_addToFavoriteCommand == null)
                    _addToFavoriteCommand = new RelayCommand(param => CommandAddToFavorite((Recipe)param), null);

                return _addToFavoriteCommand;
            }
        }
        private ICommand _openRecipeCommand;
        public ICommand OpenRecipeCommand
        {
            get
            {
                if (_openRecipeCommand == null)
                    _openRecipeCommand = new RelayCommand(param => CommandOpenRecipe((string)param), null);

                return _openRecipeCommand;
            }
        }
        private ICommand _deleteRecipeCommand;
        public ICommand DeleteRecipeCommand
        {
            get
            {
                if (_deleteRecipeCommand == null)
                    _deleteRecipeCommand = new RelayCommand(param => CommandDeleteRecipe((string)param), null);

                return _deleteRecipeCommand;
            }
        }
        private ICommand _searchRecipeCommand;
        public ICommand SearchRecipeCommand
        {
            get
            {
                if (_searchRecipeCommand == null)
                    _searchRecipeCommand = new RelayCommand(param => CommandSearchRecipe(), null);

                return _searchRecipeCommand;
            }
        }
        private bool disposed = false;
        public void CommandAddNewRecipe()
        {
            var window = new RecipeWindow();
            window.Show();
        }
        public void CommandOpenRecipe(string name)
        {
            var window = new RecipeWindow(name);
            window.Show();
        }
        public void CommandDeleteRecipe(string name)
        {
            MessageBoxButton button = MessageBoxButton.YesNo;
            string messageBoxText = "Czy na pewno chcesz usunąć przepis?";
            MessageBoxResult result = MessageBox.Show(messageBoxText, null, button);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    var recipe = MainWindow.UnitOfWork.RecipeRepository.GetRecipe(name);
                    MainWindow.UnitOfWork.RecipeRepository.DeleteRecipe(recipe);
                    MessageBox.Show("Pomyślnie usunięto przepis " + name + ".");
                    RecipesNames.Remove(RecipesNames.Where(x => x.Name == recipe.Name).FirstOrDefault());
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        public void CommandSearchRecipe()
        {
            if (NodeCategory != "")
            {
                GetRecipes(SearchingPhrase, NodeCategory);
            }
            else
            {
                GetRecipes(SearchingPhrase);
            }
        }
        public void CommandAddToFavorite(Recipe recipe)
        {
            MainWindow.UnitOfWork.RecipeRepository.AddRecipe(recipe);
            CommandSearchRecipe();
        }
        public static void GetRecipes(string search = null, string category = null)
        {
            RecipesNames = new ObservableCollection<Recipe>();
            var recipes = new List<Recipe>();
            var recipeNameList = MainWindow.UnitOfWork.RecipeRepository.GetAllRecipies();
            foreach (var recipe in recipeNameList)
            {
                if (category != null)
                {
                    if (search != null)
                    {
                        if (recipe.Name.Contains(search) && recipe.Categories.Contains(category))
                        {
                            recipes.Add(recipe);
                        }
                    }
                    else
                    {
                        if (recipe.Categories.Contains(category))
                        {
                            recipes.Add(recipe);
                        }
                    }
                }
                else
                {
                    if (search != null)
                    {
                        if (recipe.Name.Contains(search))
                        {
                            recipes.Add(recipe);
                        }
                    }
                    else
                    {
                        recipes.Add(recipe);
                    }
                }
            }
            var recipesOrderBy = recipes.OrderByDescending(x => x.IsFavorite);
            foreach (var recipeOrderBy in recipesOrderBy)
            {
                RecipesNames.Add(recipeOrderBy);
            }
        }
        public static void GetCategories()
        {
            CategoriesNames = new ObservableCollection<string>();
            List<string> categorieslist = new List<string>();
            CategoriesNames.Add(string.Empty);
            var recipeNameList = MainWindow.UnitOfWork.RecipeRepository.GetAllRecipies();
            foreach (var recipe in recipeNameList)
            {
                foreach (var category in recipe.Categories)
                {
                    if (!categorieslist.Contains(category))
                    {
                        categorieslist.Add(category);
                    }
                }
            }
            foreach (var category in categorieslist)
            {
                CategoriesNames.Add(category);
            }
        }
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
