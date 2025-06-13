using System;
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
            CategoryComboBox.ItemsSource = new[] { new Category { Id = 1, Name = "Food" }, new Category { Id = 2, Name = "Rent" } };
            CategoryComboBox.DisplayMemberPath = "Name";
            CategoryComboBox.SelectedValuePath = "Id";
            TypeComboBox.SelectedIndex = 0; 
        }

        private void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (!decimal.TryParse(AmountTextBox.Text, out decimal amount))
                {
                    MessageBox.Show("Please enter a valid amount.");
                    return;
                }

                
                if (CategoryComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Please select a category.");
                    return;
                }

               
                if (TypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a transaction type (Income/Expense).");
                    return;
                }

                var transaction = new Transaction
                {
                    Description = DescriptionTextBox.Text,
                    Amount = amount,
                    Date = DatePicker.SelectedDate.GetValueOrDefault(DateTime.Now),
                    CategoryId = (int)CategoryComboBox.SelectedValue,
                    Type = ((ComboBoxItem)TypeComboBox.SelectedItem).Content.ToString() == "Income"
                           ? TransactionType.Income
                           : TransactionType.Expense
                };
                _viewModel.AddTransaction(transaction);
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding transaction: {ex.Message}");
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

               
                if (!decimal.TryParse(AmountTextBox.Text, out decimal amount))
                {
                    MessageBox.Show("Please enter a valid amount.");
                    return;
                }

                if (CategoryComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Please select a category.");
                    return;
                }

                if (TypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a transaction type (Income/Expense).");
                    return;
                }

                var updatedTransaction = new Transaction
                {
                    Id = _viewModel.SelectedTransaction.Id,
                    Description = DescriptionTextBox.Text,
                    Amount = amount,
                    Date = DatePicker.SelectedDate.GetValueOrDefault(DateTime.Now),
                    CategoryId = (int)CategoryComboBox.SelectedValue,
                    Type = ((ComboBoxItem)TypeComboBox.SelectedItem).Content.ToString() == "Income"
                           ? TransactionType.Income
                           : TransactionType.Expense
                };
                _viewModel.UpdateTransaction(updatedTransaction);
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating transaction: {ex.Message}");
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
            if (TransactionsGrid.SelectedItem is Transaction selectedTransaction)
            {
                _viewModel.SelectedTransaction = selectedTransaction;
                DescriptionTextBox.Text = selectedTransaction.Description;
                AmountTextBox.Text = selectedTransaction.Amount.ToString();
                DatePicker.SelectedDate = selectedTransaction.Date;
                CategoryComboBox.SelectedValue = selectedTransaction.CategoryId;
                TypeComboBox.SelectedIndex = selectedTransaction.Type == TransactionType.Income ? 0 : 1;
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
            DescriptionTextBox.Text = string.Empty;
            AmountTextBox.Text = string.Empty;
            DatePicker.SelectedDate = null;
            CategoryComboBox.SelectedIndex = -1;
            TypeComboBox.SelectedIndex = 0;
            _viewModel.SelectedTransaction = null;
        }
    }
}