using Newtonsoft.Json;
using RecipeManager.Model;
using RecipeManager.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace RecipeManager.DataAccess
{
    public class RecipeRepository
    {
        public void AddRecipe(Recipe recipe)
        {
            string path;
            var login = MainWindowViewModel.User.Login.Replace(" ", "");
            path = login + "/" + recipe.Name + ".json";
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, recipe);
            }
        }
        public void DeleteRecipe(Recipe recipe)
        {
            string path;
            var login = MainWindowViewModel.User.Login.Replace(" ", "");
            path = login + "/" + recipe.Name + ".json";
            File.Delete(path);
        }
        public Recipe GetRecipe(string name)
        {
            string path;
            var login = MainWindowViewModel.User.Login.Replace(" ", "");
            path = login + "/" + name + ".json";
            if (File.Exists(path))
            {
                var recipe = JsonConvert.DeserializeObject<Recipe>(File.ReadAllText(path));
                return recipe;
            }
            else
            {
                return new Recipe();
            }
        }
        public List<Recipe> GetAllRecipies()
        {
            var path = MainWindowViewModel.User.Login.Replace(" ", string.Empty);
            List<Recipe> recipiesNamelist = new List<Recipe>();
            foreach (string file in Directory.EnumerateFiles(path, "*.json"))
            {
                string contents = File.ReadAllText(file);
                Recipe recipe = JsonConvert.DeserializeObject<Recipe>(contents);
                recipiesNamelist.Add(recipe);
            }
            return recipiesNamelist;
        }
        public List<string> GetListStringFromMultilineString(string input)
        {
            if (input != null)
            {
                List<string> list = new List<string>(
                               input.Split(new string[] { "\r\n" },
                               StringSplitOptions.RemoveEmptyEntries));
                return list;
            }
            else
            {
                return new List<string>();
            }
        }
        public List<string> GetAllRecipiesNames()
        {
            List<string> recipiesNamelist = new List<string>();
            var recipies = GetAllRecipies();
            foreach (var recipe in recipies)
            {
                recipiesNamelist.Add(recipe.Name);
            }
            return recipiesNamelist;
        }
    }
}
