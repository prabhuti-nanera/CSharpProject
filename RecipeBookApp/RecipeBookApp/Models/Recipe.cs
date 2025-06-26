using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RecipeBookApp.Models
{
    public class Recipe : ObservableObject
    {
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

        private DateTime _createdDate;
        public DateTime CreatedDate
        {
            get => _createdDate;
            set => SetProperty(ref _createdDate, value);
        }
    }
}
