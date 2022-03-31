namespace RecipeManager.DataAccess
{
    public class RecipeValidation
    {
        public bool NameValidation(string name)
        {
            if (name == null)
            {
                return false;
            }
            if (name.Length >= 1)
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
