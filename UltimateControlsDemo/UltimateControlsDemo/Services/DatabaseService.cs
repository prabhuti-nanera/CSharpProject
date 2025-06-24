using Microsoft.Data.SqlClient;
using UltimateControlsDemo.Models;
using System.Collections.Generic;

namespace UltimateControlsDemo.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString = "Server=localhost;Database=UltimateControlsDemoDB;Integrated Security=True;"; // Customize this (e.g., Server=YourServerName\Instance)

        public DatabaseService()
        {
            InitializeDatabase();
        }

        /// <summary>
        /// Initializes the database by creating the ToDoItems table if it doesn't exist.
        /// </summary>
        private void InitializeDatabase()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                var command = new SqlCommand(@"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ToDoItems')
                    BEGIN
                        CREATE TABLE ToDoItems (
                            Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
                            Title NVARCHAR(255) NOT NULL,
                            IsDone BIT NOT NULL DEFAULT 0
                        );
                    END", connection);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // Log the exception (e.g., to a file or console) and handle gracefully
                Console.WriteLine($"Database initialization failed: {ex.Message}");
                throw; // Re-throw to allow calling code to handle the error
            }
        }

        /// <summary>
        /// Adds a new to-do item to the database.
        /// </summary>
        /// <param name="item">The ToDoItem to add.</param>
        public void AddToDoItem(ToDoItem item)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                var command = new SqlCommand("INSERT INTO ToDoItems (Title, IsDone) VALUES (@Title, @IsDone)", connection);
                command.Parameters.AddWithValue("@Title", item.Title ?? string.Empty); // Handle null titles
                command.Parameters.AddWithValue("@IsDone", item.IsDone);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Failed to add to-do item: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all to-do items from the database.
        /// </summary>
        /// <returns>A list of ToDoItem objects.</returns>
        public List<ToDoItem> GetToDoItems()
        {
            var items = new List<ToDoItem>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                var command = new SqlCommand("SELECT Id, Title, IsDone FROM ToDoItems", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new ToDoItem
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        IsDone = reader.GetBoolean(2)
                    });
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Failed to retrieve to-do items: {ex.Message}");
                throw;
            }
            return items;
        }

        /// <summary>
        /// Deletes a to-do item from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the to-do item to delete.</param>
        public void DeleteToDoItem(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                var command = new SqlCommand("DELETE FROM ToDoItems WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Failed to delete to-do item: {ex.Message}");
                throw;
            }
        }
    }
}