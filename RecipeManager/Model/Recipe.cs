using System.Collections.Generic;

namespace RecipeManager.Model
{
    public class Recipe
    {
        public string Name { set; get; }
        public string ShortDescription { set; get; }
        public string Introduction { set; get; }
        public List<string> Ingredients { set; get; }
        public string Preparation { set; get; }
        public List<string> Categories { get; set; }
        public bool IsFavorite { get; set; }
        public string Link { get; set; }
    }
}
