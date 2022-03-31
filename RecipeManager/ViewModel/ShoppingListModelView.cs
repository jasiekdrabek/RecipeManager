using RecipeManager.DataAccess;
using RecipeManager.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace RecipeManager.ViewModel
{
    public class ShoppingListModelView : BaseViewModel, IDisposable
    {
        private static ObservableCollection<Product> _products;
        public static ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                StaticPropertyChanged?.Invoke(null, ProductsPropertyEventArgs);
            }
        }
        public static event PropertyChangedEventHandler StaticPropertyChanged;
        private static readonly PropertyChangedEventArgs ProductsPropertyEventArgs = new PropertyChangedEventArgs(nameof(Products));
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
        private string _quantity;
        public string Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }
        private ICommand _addProductCommand;
        public ICommand AddProductCommand
        {
            get
            {
                if (_addProductCommand == null)
                    _addProductCommand = new RelayCommand(param => CommandAddProduct(), null);

                return _addProductCommand;
            }
        }
        private ICommand _deleteProductCommand;
        public ICommand DeleteProductCommand
        {
            get
            {
                if (_deleteProductCommand == null)
                    _deleteProductCommand = new RelayCommand(param => CommandDeleteProduct((Guid)param), null);

                return _deleteProductCommand;
            }
        }
        private ICommand _deleteAllProductsCommand;
        public ICommand DeleteAllProductsCommand
        {
            get
            {
                if (_deleteAllProductsCommand == null)
                    _deleteAllProductsCommand = new RelayCommand(param => CommandDeleteAllProducts(), null);

                return _deleteAllProductsCommand;
            }
        }
        private ICommand _addQuantityCommand;
        public ICommand AddQuantityCommand
        {
            get
            {
                if (_addQuantityCommand == null)
                    _addQuantityCommand = new RelayCommand(param => CommandAddQuantity((Guid)param), null);

                return _addQuantityCommand;
            }
        }
        private ICommand _subtractQuantityCommand;
        public ICommand SubtractQuantityCommand
        {
            get
            {
                if (_subtractQuantityCommand == null)
                    _subtractQuantityCommand = new RelayCommand(param => CommandSubtractQuantity((Guid)param), null);

                return _subtractQuantityCommand;
            }
        }
        private ProductValidation _productValidation;
        public ShoppingListModelView()
        {
            _productValidation = new ProductValidation();
        }
        public void CommandAddProduct()
        {
            if (_productValidation.ValidateName(Name,MainWindowViewModel.User.Id))
            {
                if (_productValidation.ValidateQuantity(Quantity))
                {
                    var quantity = int.Parse(Quantity);
                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Name = this.Name,
                        Quantity = quantity,
                        UserId = MainWindowViewModel.User.Id
                    };
                    MainWindow.UnitOfWork.ProductRepository.AddProduct(product);
                    MainWindow.UnitOfWork.Save();
                    GetAllProducts();
                }
                else
                {
                    MessageBox.Show("Podana ilość musi być całkowitą liczbą dodatnią");
                }
            }
            else
            {
                MessageBox.Show("Podana nazwa ma nieodpowiednią długość lub produkt o tej nazwie już jest w liście zakupów");
            }
        }
        public static void CommandDeleteProduct(Guid Id)
        {
            MessageBoxButton button = MessageBoxButton.YesNo;
            string messageBoxText = "Czy na pewno chcesz usunąć produkt z listy zakupów?";
            MessageBoxResult result = MessageBox.Show(messageBoxText, null, button);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    var product = MainWindow.UnitOfWork.ProductRepository.Get(Id);
                    MainWindow.UnitOfWork.ProductRepository.RemoveProduct(product.Id);
                    MainWindow.UnitOfWork.Save();
                    GetAllProducts();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        public  void CommandDeleteAllProducts()
        {
            MessageBoxButton button = MessageBoxButton.YesNo;
            string messageBoxText = "Czy na pewno chcesz usunąć listę zakupów?";
            MessageBoxResult result = MessageBox.Show(messageBoxText, null, button);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    var products = MainWindow.UnitOfWork.ProductRepository.GetAll();
                    foreach (var product in products)
                    {
                        MainWindow.UnitOfWork.ProductRepository.RemoveProduct(product.Id);
                    }
                    MainWindow.UnitOfWork.Save();
                    GetAllProducts();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }
        public static void CommandAddQuantity(Guid id)
        {
            var product = MainWindow.UnitOfWork.ProductRepository.Get(id);
            product.Quantity += 1;
            MainWindow.UnitOfWork.Save();
            GetAllProducts();
        }
        public static void CommandSubtractQuantity(Guid id)
        {
            var product = MainWindow.UnitOfWork.ProductRepository.Get(id);
            product.Quantity -= 1;
            if (product.Quantity == 0)
            {
                CommandDeleteProduct(id);
            }
            else
            {
                MainWindow.UnitOfWork.ProductRepository.UpdateProduct(product);
                MainWindow.UnitOfWork.Save();
            }
            GetAllProducts();
        }
        public static void GetAllProducts()
        {
            Products = new ObservableCollection<Product>();
            var products = MainWindow.UnitOfWork.ProductRepository.GetAll();
            foreach (var product in products)
            {
                if (product.UserId == MainWindowViewModel.User.Id)
                {
                    Products.Add(product);
                }
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