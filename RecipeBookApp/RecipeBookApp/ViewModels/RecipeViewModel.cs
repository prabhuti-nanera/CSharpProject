using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecipeBookApp.Models;

namespace RecipeBookApp.ViewModels
{
    public class RecipeViewModel : ObservableObject
    {
        private ObservableCollection<Recipe> _recipes = new();
        public ObservableCollection<Recipe> Recipes
        {
            get => _recipes;
            set => SetProperty(ref _recipes, value);
        }

        private string? _name;
        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string? _cuisine;
        public string? Cuisine
        {
            get => _cuisine;
            set => SetProperty(ref _cuisine, value);

        }

        private string? _ingredients;
        public string? Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }

        private int _prepTime;
        public int PrepTime
        {
            get => _prepTime;
            set => SetProperty(ref _prepTime, value);
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }

        public RecipeViewModel()
        {
            Recipes.Add(new Recipe
            {
                Name = "Poha",
                Cuisine = "Indian",
                Ingredients = "Flattened rice, Onion, Spices",
                PrepTime = 15,
                IsFavorite = true,
                CreatedDate = DateTime.Now
            });
        }

        [RelayCommand]
        private void AddRecipe()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Ingredients))
                return;

            Recipes.Add(new Recipe
            {
                Name = Name,
                Cuisine = Cuisine,
                Ingredients = Ingredients,
                PrepTime = PrepTime,
                IsFavorite = IsFavorite,
                CreatedDate = DateTime.Now
            });

            // Clear form
            Name = string.Empty;
            Cuisine = string.Empty;
            Ingredients = string.Empty;
            PrepTime = 0;
            IsFavorite = false;
        }
    }
}
