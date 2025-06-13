using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.Services;
using PersonalFinanceManager.Utilities;

namespace PersonalFinanceManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService = new DatabaseService();
        private readonly ReportService<Transaction> _reportService = new ReportService<Transaction>();
        private readonly FileService _fileService = new FileService();
        private readonly NotificationService _notificationService = new NotificationService();
        private List<Transaction> _transactions;
        private Transaction _selectedTransaction;

        public List<Transaction> Transactions
        {
            get => _transactions;
            set
            {
                _transactions = value;
                OnPropertyChanged(nameof(Transactions));
            }
        }

        public Transaction SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                _selectedTransaction = value;
                OnPropertyChanged(nameof(SelectedTransaction));
            }
        }

        public MainViewModel()
        {
            LoadTransactions();
            _notificationService.OnTransactionAdded += (t, msg) => MessageBox.Show(msg);
        }

        public async void LoadTransactions()
        {
            try
            {
                Transactions = await _dbService.GetTransactionsAsync();
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
                LoadTransactions();
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
                LoadTransactions();
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
                LoadTransactions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting transaction: {ex.Message}");
            }
        }

        public async void GenerateReport(int year, int month)
        {
            try
            {
                var summary = await _reportService.GenerateMonthlyReportAsync(Transactions, year, month);
                MessageBox.Show($"Income: {summary.TotalIncome:C}, Expense: {summary.TotalExpense:C}, Balance: {summary.Balance:C}");
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
                _fileService.ExportToXml(Transactions, filePath);
                MessageBox.Show("Transactions exported successfully!");
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