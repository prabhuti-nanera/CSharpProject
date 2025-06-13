using System;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.Utilities
{
    public delegate void TransactionHandler(Transaction transaction, string message);

    public class NotificationService
    {
        public event TransactionHandler OnTransactionAdded;

        public void NotifyTransactionAdded(Transaction transaction)
        {
            OnTransactionAdded?.Invoke(transaction, $"Transaction added: {transaction}");
        }
    }
}