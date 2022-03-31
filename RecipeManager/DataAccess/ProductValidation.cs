using System;

namespace RecipeManager.DataAccess
{
    public class ProductValidation
    {
        public bool ValidateName(string name,Guid id)
        {
            if (name.Length > 0 && name.Length < 40 && MainWindow.UnitOfWork.ProductRepository.CheckIfProductAlreadyExist(id,name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ValidateQuantity(string quantity)
        {
            int result = 0;
            int.TryParse(quantity, out result);
            if (result >= 1)
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
