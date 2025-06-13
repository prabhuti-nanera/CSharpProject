using System;

namespace PersonalFinanceManager.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public TransactionType Type { get; set; }
        public Category Category { get; set; } 

        public Transaction() { }
        public Transaction(string description, decimal amount, DateTime date, int categoryId, TransactionType type)
        {
            Description = description;
            Amount = amount;
            Date = date;
            CategoryId = categoryId;
            Type = type;
        }

        public override string ToString() => $"{Description}: {Amount:C} ({Type}) on {Date:MM/dd/yyyy}";

        public static Transaction operator +(Transaction t1, Transaction t2)
        {
            return new Transaction
            {
                Description = $"{t1.Description} + {t2.Description}",
                Amount = t1.Amount + t2.Amount,
                Date = t1.Date,
                CategoryId = t1.CategoryId,
                Type = t1.Type
            };
        }
    }
}