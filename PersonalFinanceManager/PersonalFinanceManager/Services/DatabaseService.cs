using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using PersonalFinanceManager.Models;

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
                var command = new SqlCommand("SELECT t.Id, t.Description, t.Amount, t.Date, t.CategoryId, t.IsIncome, c.Name " +
                                             "FROM Transactions t JOIN Categories c ON t.CategoryId = c.Id", connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        transactions.Add(new Transaction
                        {
                            Id = reader.GetInt32(0),
                            Description = reader.GetString(1),
                            Amount = reader.GetDecimal(2),
                            Date = reader.GetDateTime(3),
                            CategoryId = reader.GetInt32(4),
                            Type = reader.GetBoolean(5) ? TransactionType.Income : TransactionType.Expense,
                            Category = new Category { Id = reader.GetInt32(4), Name = reader.GetString(6) }
                        });
                    }
                }
            }
            return transactions;
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Transactions (Description, Amount, Date, CategoryId, IsIncome) " +
                                             "VALUES (@desc, @amount, @date, @catId, @isIncome)", connection);
                command.Parameters.AddWithValue("@desc", transaction.Description);
                command.Parameters.AddWithValue("@amount", transaction.Amount);
                command.Parameters.AddWithValue("@date", transaction.Date);
                command.Parameters.AddWithValue("@catId", transaction.CategoryId);
                command.Parameters.AddWithValue("@isIncome", transaction.Type == TransactionType.Income);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE Transactions SET Description = @desc, Amount = @amount, Date = @date, " +
                                             "CategoryId = @catId, IsIncome = @isIncome WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@id", transaction.Id);
                command.Parameters.AddWithValue("@desc", transaction.Description ?? (object)DBNull.Value); 
                command.Parameters.AddWithValue("@amount", transaction.Amount);
                command.Parameters.AddWithValue("@date", transaction.Date);
                command.Parameters.AddWithValue("@catId", transaction.CategoryId);
                command.Parameters.AddWithValue("@isIncome", transaction.Type == TransactionType.Income);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteTransactionAsync(int transactionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Transactions WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@id", transactionId);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}