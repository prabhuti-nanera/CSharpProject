using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.Services
{
    public class ReportService<T> where T : Transaction
    {
        public async Task<ReportSummary> GenerateMonthlyReportAsync(List<T> transactions, int year, int month)
        {
            return await Task.Run(() =>
            {
                var monthlyTransactions = transactions.Where(t => t.Date.Year == year && t.Date.Month == month);
                var summary = new ReportSummary
                {
                    TotalIncome = monthlyTransactions
                        .Where(t => t.Type == TransactionType.Income)
                        .Aggregate(0m, (sum, t) => sum + t.Amount),
                    TotalExpense = monthlyTransactions
                        .Where(t => t.Type == TransactionType.Expense)
                        .Aggregate(0m, (sum, t) => sum + t.Amount)
                };
                return summary;
            });
        }

        public Dictionary<string, decimal> GetCategoryBreakdown(List<T> transactions)
        {
            return transactions
                .GroupBy(t => t.Category.Name)
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount));
        }

        public object GetAnonymousSummary(List<T> transactions)
        {
            return transactions.Select(t => new
            {
                t.Description,
                FormattedAmount = $"{t.Amount:C}",
                IsRecent = t.Date >= DateTime.Now.AddDays(-30)
            }).ToList();
        }
    }
}