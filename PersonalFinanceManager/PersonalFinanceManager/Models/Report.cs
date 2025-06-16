namespace PersonalFinanceManager.Models
{
    public class Report
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance => TotalIncome - TotalExpenses;
    }
}