using RecipeManager.Model;
using System;

namespace RecipeManager.DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private DbModel _context = new DbModel();
        private UserRepository _userRepository;
        public UserRepository UserRepository
        {
            get
            {

                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(_context);
                }
                return _userRepository;
            }
        }
        private ProductRepository _productRepository;
        public ProductRepository ProductRepository
        {
            get
            {

                if (this._productRepository == null)
                {
                    this._productRepository = new ProductRepository(_context);
                }
                return _productRepository;
            }
        }
        private RecipeRepository _recipeRepository;
        public RecipeRepository RecipeRepository
        {
            get
            {
                if(this._recipeRepository == null)
                {
                    this._recipeRepository = new RecipeRepository();
                }
                return _recipeRepository;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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
