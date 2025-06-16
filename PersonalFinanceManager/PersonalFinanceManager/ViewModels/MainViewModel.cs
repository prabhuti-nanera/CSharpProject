using PersonalFinanceManager.Models;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace PersonalFinanceManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly NotificationService _notificationService;
        private bool _isLoading;

        public ObservableCollection<Transaction> Transactions { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public Transaction SelectedTransaction { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public MainViewModel()
        {
            _dbService = new DatabaseService();
            _notificationService = new NotificationService();
            Transactions = new ObservableCollection<Transaction>();
            Categories = new ObservableCollection<Category>();
            IsLoading = true;
            Task.Run(async () =>
            {
                try
                {
                    await LoadCategoriesAsync();
                    await LoadTransactionsAsync();
                }
                finally
                {
                    IsLoading = false;
                }
            });
        }

        public async Task LoadCategoriesAsync()
        {
            try
            {
                var categories = await _dbService.GetCategoriesAsync();
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}");
            }
        }

        public async Task LoadTransactionsAsync()
        {
            try
            {
                var transactions = await _dbService.GetTransactionsAsync();
                Transactions.Clear();
                foreach (var transaction in transactions)
                {
                    Transactions.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transactions: {ex.Message}");
            }
        }

        public async void AddTransaction(Transaction transaction)
        {
            try
            {
                await _dbService.AddTransactionAsync(transaction);
                _notificationService.NotifyTransactionAdded(transaction);
                await LoadTransactionsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding transaction: {ex.Message}");
            }
        }

        public async void UpdateTransaction(Transaction transaction)
        {
            try
            {
                await _dbService.UpdateTransactionAsync(transaction);
                await LoadTransactionsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating transaction: {ex.Message}");
            }
        }

        public async void DeleteTransaction(int transactionId)
        {
            try
            {
                await _dbService.DeleteTransactionAsync(transactionId);
                await LoadTransactionsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting transaction: {ex.Message}");
            }
        }

        public void GenerateReport(int year, int month)
        {
            try
            {
                var report = _dbService.GenerateReport(year, month);
                MessageBox.Show($"Report for {month}/{year}:\nIncome: {report.TotalIncome:C}\nExpenses: {report.TotalExpenses:C}\nBalance: {report.Balance:C}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}");
            }
        }

        public void ExportToXml(string filePath)
        {
            try
            {
                _dbService.ExportToXml(filePath);
                MessageBox.Show($"Transactions exported to {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to XML: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}