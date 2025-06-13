using PersonalFinanceManager.Models;
using System;

namespace PersonalFinanceManager.Utilities
{
    public static class TransactionFactory
    {
        public static Transaction CreateTransaction(string description, decimal amount, TransactionType type)
        {
            return new Transaction(description, amount, DateTime.Now, 1, type);
        }
    }
}