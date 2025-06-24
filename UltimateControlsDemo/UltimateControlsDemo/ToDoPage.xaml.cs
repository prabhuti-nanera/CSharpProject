using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using UltimateControlsDemo.Models;
using UltimateControlsDemo.Services;

namespace UltimateControlsDemo
{
    public partial class ToDoPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<ToDoItem> ToDoItems { get; set; }

        public ToDoPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            ToDoItems = new ObservableCollection<ToDoItem>(_databaseService.GetToDoItems());
            ToDoList.ItemsSource = ToDoItems;
            this.FadeTo(1, 500);
        }

        private void OnAddTask(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ToDoEntry.Text))
            {
                var newItem = new ToDoItem { Title = ToDoEntry.Text, IsDone = false };
                _databaseService.AddToDoItem(newItem);
                ToDoItems.Add(newItem);
                ToDoEntry.Text = string.Empty;
            }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is ToDoItem item)
            {
                _databaseService.DeleteToDoItem(item.Id);
                ToDoItems.Remove(item);
            }
        }
    }
}