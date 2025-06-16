using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PersonalFinanceManager.Models;
using System.IO;

namespace PersonalFinanceManager.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString = "Server=localhost\\SQLEXPRESS;Database=FinanceDB;Trusted_Connection=True;";

        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            var transactions = new List<Transaction>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "SELECT t.Id, t.Description, t.Amount, t.Date, t.CategoryId, t.Type, c.Name as CategoryName " +
                    "FROM Transactions t LEFT JOIN Categories c ON t.CategoryId = c.Id", connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var transaction = new Transaction
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            Amount = reader.GetDecimal(2),
                            Date = reader.GetDateTime(3),
                            CategoryId = reader.GetInt32(4),
                            Type = (TransactionType)Enum.Parse(typeof(TransactionType), reader.GetString(5)),
                            Category = new Category
                            {
                                Id = reader.GetInt32(4),
                                Name = reader.IsDBNull(6) ? "Unknown" : reader.GetString(6)
                            }
                        };
                        transactions.Add(transaction);
                    }
                }
            }
            return transactions;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = new List<Category>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT Id, Name FROM Categories", connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var category = new Category
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        categories.Add(category);
                    }
                }
            }
            return categories;
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "INSERT INTO Transactions (Description, Amount, Date, CategoryId, Type) " +
                    "VALUES (@Description, @Amount, @Date, @CategoryId, @Type)", connection);
                command.Parameters.AddWithValue("@Description", transaction.Description);
                command.Parameters.AddWithValue("@Amount", transaction.Amount);
                command.Parameters.AddWithValue("@Date", transaction.Date);
                command.Parameters.AddWithValue("@CategoryId", transaction.CategoryId);
                command.Parameters.AddWithValue("@Type", transaction.Type.ToString());
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "UPDATE Transactions SET Description = @Description, Amount = @Amount, Date = @Date, " +
                    "CategoryId = @CategoryId, Type = @Type WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", transaction.Id);
                command.Parameters.AddWithValue("@Description", transaction.Description);
                command.Parameters.AddWithValue("@Amount", transaction.Amount);
                command.Parameters.AddWithValue("@Date", transaction.Date);
                command.Parameters.AddWithValue("@CategoryId", transaction.CategoryId);
                command.Parameters.AddWithValue("@Type", transaction.Type.ToString());
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteTransactionAsync(int transactionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Transactions WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", transactionId);
                await command.ExecuteNonQueryAsync();
            }
        }

        public void ExportToXml(string filePath)
        {
            var transactions = GetTransactionsAsync().GetAwaiter().GetResult();
            var serializer = new XmlSerializer(typeof(List<Transaction>));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, transactions);
            }
        }

        public Report GenerateReport(int year, int month)
        {
            var report = new Report();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "SELECT Type, Amount FROM Transactions " +
                    "WHERE YEAR(Date) = @Year AND MONTH(Date) = @Month", connection);
                command.Parameters.AddWithValue("@Year", year);
                command.Parameters.AddWithValue("@Month", month);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var type = (TransactionType)Enum.Parse(typeof(TransactionType), reader.GetString(0));
                        var amount = reader.GetDecimal(1);
                        if (type == TransactionType.Income)
                        {
                            report.TotalIncome += amount;
                        }
                        else
                        {
                            report.TotalExpenses += amount;
                        }
                    }
                }
            }
            return report;
        }
    }
}