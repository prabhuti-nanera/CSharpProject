using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.ViewModels;

namespace PersonalFinanceManager
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel = new MainViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (CategoryComboBox == null || TypeComboBox == null || TransactionsGrid == null)
            {
                MessageBox.Show("One or more controls are not initialized. Check XAML and project build.");
                return;
            }

            // Wait for categories and transactions to load
            while (_viewModel.IsLoading)
            {
                await Task.Delay(100); // Wait for async loading to complete
            }

            // Set default selection for TypeComboBox
            TypeComboBox.SelectedIndex = 0; // Default to "Income"

            // Set default selection for CategoryComboBox if categories are available
            if (_viewModel.Categories.Any())
            {
                CategoryComboBox.SelectedIndex = 0; // Select the first category
            }
            else
            {
                MessageBox.Show("No categories are available. Please add categories to the database to proceed.");
                TransactionsGrid.IsEnabled = false; // Disable the grid if no categories
            }
        }

        private async void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DescriptionTextBox == null || AmountTextBox == null || CategoryComboBox == null || TypeComboBox == null || DatePicker == null)
                {
                    MessageBox.Show("One or more controls are not initialized. Check XAML and project build.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
                {
                    MessageBox.Show("Please enter a description.");
                    return;
                }

                if (!decimal.TryParse(AmountTextBox.Text, out decimal amount))
                {
                    MessageBox.Show("Please enter a valid amount.");
                    return;
                }

                if (CategoryComboBox.SelectedIndex == -1 || CategoryComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Please select a category.");
                    return;
                }

                if (TypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a transaction type (Income/Expense).");
                    return;
                }

                // Refresh categories to ensure they match the database
                await _viewModel.LoadCategoriesAsync();

                int selectedCategoryId = (int)CategoryComboBox.SelectedValue;
                // Validate that the CategoryId exists in the database
                if (!_viewModel.Categories.Any(c => c.Id == selectedCategoryId))
                {
                    MessageBox.Show("The selected category is invalid or no longer exists in the database. Please select a different category.");
                    CategoryComboBox.SelectedIndex = _viewModel.Categories.Any() ? 0 : -1;
                    return;
                }

                var transaction = new Transaction
                {
                    Description = DescriptionTextBox.Text,
                    Amount = amount,
                    Date = DatePicker.SelectedDate.GetValueOrDefault(DateTime.Now),
                    CategoryId = selectedCategoryId,
                    Type = ((ComboBoxItem)TypeComboBox.SelectedItem).Content.ToString() == "Income"
                           ? TransactionType.Income
                           : TransactionType.Expense
                };
                _viewModel.AddTransaction(transaction);
                ClearInputFields();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FOREIGN KEY constraint"))
                {
                    MessageBox.Show("Error adding transaction: The selected category does not exist in the database. Please select a different category or add the category to the database.");
                    await _viewModel.LoadCategoriesAsync();
                    CategoryComboBox.SelectedIndex = _viewModel.Categories.Any() ? 0 : -1;
                }
                else
                {
                    MessageBox.Show($"Error adding transaction: {ex.Message}\nPlease ensure the database is accessible and the category exists.");
                }
            }
        }

        private void UpdateTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.SelectedTransaction == null)
                {
                    MessageBox.Show("Please select a transaction to update.");
                    return;
                }

                if (DescriptionTextBox == null || AmountTextBox == null || CategoryComboBox == null || TypeComboBox == null || DatePicker == null)
                {
                    MessageBox.Show("One or more controls are not initialized. Check XAML and project build.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
                {
                    MessageBox.Show("Please enter a description.");
                    return;
                }

                if (!decimal.TryParse(AmountTextBox.Text, out decimal amount))
                {
                    MessageBox.Show("Please enter a valid amount.");
                    return;
                }

                if (CategoryComboBox.SelectedIndex == -1 || CategoryComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Please select a category.");
                    return;
                }

                if (TypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a transaction type (Income/Expense).");
                    return;
                }

                int selectedCategoryId = (int)CategoryComboBox.SelectedValue;
                if (!_viewModel.Categories.Any(c => c.Id == selectedCategoryId))
                {
                    MessageBox.Show("The selected category is invalid or no longer exists in the database. Please refresh the categories.");
                    return;
                }

                var updatedTransaction = new Transaction
                {
                    Id = _viewModel.SelectedTransaction.Id,
                    Description = DescriptionTextBox.Text,
                    Amount = amount,
                    Date = DatePicker.SelectedDate.GetValueOrDefault(DateTime.Now),
                    CategoryId = selectedCategoryId,
                    Type = ((ComboBoxItem)TypeComboBox.SelectedItem).Content.ToString() == "Income"
                           ? TransactionType.Income
                           : TransactionType.Expense
                };
                _viewModel.UpdateTransaction(updatedTransaction);
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating transaction: {ex.Message}\nPlease ensure the database is accessible and the category exists.");
            }
        }

        private void DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_viewModel.SelectedTransaction == null)
                {
                    MessageBox.Show("Please select a transaction to delete.");
                    return;
                }

                var result = MessageBox.Show("Are you sure you want to delete this transaction?",
                                             "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.DeleteTransaction(_viewModel.SelectedTransaction.Id);
                    ClearInputFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting transaction: {ex.Message}");
            }
        }

        private void TransactionsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransactionsGrid == null || DescriptionTextBox == null || AmountTextBox == null || DatePicker == null || CategoryComboBox == null || TypeComboBox == null)
            {
                MessageBox.Show("One or more controls are not initialized. Check XAML and project build.");
                return;
            }

            if (TransactionsGrid.SelectedItem is Transaction selectedTransaction)
            {
                _viewModel.SelectedTransaction = selectedTransaction;
                DescriptionTextBox.Text = selectedTransaction.Description;
                AmountTextBox.Text = selectedTransaction.Amount.ToString();
                DatePicker.SelectedDate = selectedTransaction.Date;
                TypeComboBox.SelectedIndex = selectedTransaction.Type == TransactionType.Income ? 0 : 1;

                // Ensure Categories are loaded and ItemsSource is not empty
                if (!_viewModel.Categories.Any())
                {
                    MessageBox.Show("Categories are not loaded. Please ensure categories are available in the database.");
                    return;
                }

                // Try to set the selected category in the dropdown
                CategoryComboBox.SelectedValue = selectedTransaction.CategoryId;

                // Check if the category was actually selected
                if (CategoryComboBox.SelectedIndex == -1)
                {
                    // Category not found in the list
                    if (selectedTransaction.Category != null && !_viewModel.Categories.Any(c => c.Id == selectedTransaction.CategoryId))
                    {
                        _viewModel.Categories.Add(selectedTransaction.Category);
                        CategoryComboBox.SelectedValue = selectedTransaction.CategoryId;
                    }
                    else
                    {
                        MessageBox.Show($"Category with ID {selectedTransaction.CategoryId} not found. It may have been deleted or not loaded.");
                    }
                }
            }
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GenerateReport(DateTime.Now.Year, DateTime.Now.Month);
        }

        private void ExportXml_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ExportToXml("transactions.xml");
        }

        private void ClearInputFields()
        {
            if (DescriptionTextBox == null || AmountTextBox == null || DatePicker == null || CategoryComboBox == null || TypeComboBox == null)
            {
                MessageBox.Show("One or more controls are not initialized. Check XAML and project build.");
                return;
            }

            DescriptionTextBox.Text = string.Empty;
            AmountTextBox.Text = string.Empty;
            DatePicker.SelectedDate = null;
            CategoryComboBox.SelectedIndex = _viewModel.Categories.Any() ? 0 : -1;
            TypeComboBox.SelectedIndex = 0;
            _viewModel.SelectedTransaction = null;
        }
    }
}