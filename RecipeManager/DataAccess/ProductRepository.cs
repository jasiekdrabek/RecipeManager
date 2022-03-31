using RecipeManager.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RecipeManager.DataAccess
{
    public class ProductRepository
    {
        internal DbModel context;
        public ProductRepository(DbModel context)
        {
            this.context = context;
        }

        public Product Get(Guid id)
        {
            return context.Products.Find(id);
        }
        public List<Product> GetAll()
        {
            return context.Products.ToList();
        }
        public void AddProduct(Product Product)
        {
            if (Product != null)
            {
                context.Products.Add(Product);
            }
        }

        public void UpdateProduct(Product Product)
        {
            var ProductFind = context.Products.Where(x => x.Id == Product.Id).FirstOrDefault();
            ProductFind.Quantity = Product.Quantity;
            context.Products.Attach(ProductFind);
            context.Entry(ProductFind).State = EntityState.Modified;
        }
        public void RemoveProduct(Guid id)
        {
            var ProductFind = context.Products.Find(id);
            if (ProductFind != null)
            {
                context.Products.Remove(ProductFind);
            }
        }
        public bool CheckIfProductAlreadyExist(Guid userid, string name)
        {
            var product = context.Products.Where(x => x.Name == name && x.UserId == userid).FirstOrDefault();
            if (product == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
