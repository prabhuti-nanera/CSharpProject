using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementSystem
{
    public partial class MainWindow : Window
    {
        private SqlConnection sqlConnection;
        private DataTable authorTable;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["LibraryManagementSystem.Properties.Settings.LibraryDBConnectionString"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Connection string 'LibraryManagementSystem.Properties.Settings.LibraryDBConnectionString' not found in App.config.", "Configuration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                MessageBox.Show("Database connection opened successfully.", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                DisplayAuthors();
                DisplayBorrowingRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize database connection: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayAuthors()
        {
            if (sqlConnection == null) return;

            try
            {
                string query = "SELECT * FROM Authors";
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    authorTable = new DataTable();
                    sqlDataAdapter.Fill(authorTable);

                    // Add a computed column for full name
                    authorTable.Columns.Add("FullName", typeof(string), "FirstName + ' ' + LastName");

                    MessageBox.Show($"Retrieved {authorTable.Rows.Count} authors", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    authorList.DisplayMemberPath = "FullName"; // Display full name instead of just LastName
                    authorList.SelectedValuePath = "Id";
                    authorList.ItemsSource = authorTable.DefaultView;
                    authorList.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading authors: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayBooksForAuthor(int authorId)
        {
            if (sqlConnection == null) return;

            try
            {
                string query = "SELECT * FROM Books WHERE AuthorId = @AuthorId";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@AuthorId", authorId);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        DataTable bookTable = new DataTable();
                        sqlDataAdapter.Fill(bookTable);

                        MessageBox.Show($"Retrieved {bookTable.Rows.Count} books for AuthorId {authorId}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                        bookList.DisplayMemberPath = "Title";
                        bookList.SelectedValuePath = "Id";
                        bookList.ItemsSource = bookTable.DefaultView;
                        bookList.Items.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayBorrowingRecords()
        {
            if (sqlConnection == null) return;

            try
            {
                string query = "SELECT br.Id, br.BookId, br.BorrowerName, br.BorrowDate, b.Title " +
                              "FROM BorrowingRecords br " +
                              "JOIN Books b ON br.BookId = b.Id";
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataTable borrowingTable = new DataTable();
                    sqlDataAdapter.Fill(borrowingTable);

                    // Add a computed column for display
                    borrowingTable.Columns.Add("DisplayText", typeof(string), "Title + ' - ' + BorrowerName + ' - ' + BorrowDate");

                    MessageBox.Show($"Retrieved {borrowingTable.Rows.Count} borrowing records", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    borrowingList.DisplayMemberPath = "DisplayText"; // Show book title, borrower, and date
                    borrowingList.SelectedValuePath = "Id";
                    borrowingList.ItemsSource = borrowingTable.DefaultView;
                    borrowingList.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading borrowing records: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void authorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sqlConnection == null || authorList.SelectedItem == null) return;

            try
            {
                DataRowView selectedAuthor = authorList.SelectedItem as DataRowView;
                if (selectedAuthor != null)
                {
                    int authorId = Convert.ToInt32(selectedAuthor["Id"]);
                    DisplayBooksForAuthor(authorId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books for author: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddAuthorClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null)
            {
                MessageBox.Show("Database connection is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(authorFirstName.Text) || string.IsNullOrWhiteSpace(authorLastName.Text))
            {
                MessageBox.Show("First Name and Last Name are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int birthYear = 0; // Initialize with a default value
            if (!string.IsNullOrWhiteSpace(authorBirthYear.Text))
            {
                if (!int.TryParse(authorBirthYear.Text, out birthYear))
                {
                    MessageBox.Show("Birth Year must be a valid integer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            try
            {
                string query = "INSERT INTO Authors (FirstName, LastName, BirthYear) VALUES (@FirstName, @LastName, @BirthYear)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@FirstName", authorFirstName.Text);
                    sqlCommand.Parameters.AddWithValue("@LastName", authorLastName.Text);
                    sqlCommand.Parameters.AddWithValue("@BirthYear", birthYear == 0 ? (object)DBNull.Value : birthYear);

                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }

                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    MessageBox.Show($"Rows affected: {rowsAffected}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                MessageBox.Show("Author added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayAuthors();

                authorFirstName.Text = "";
                authorLastName.Text = "";
                authorBirthYear.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding author: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteAuthorClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null || authorList.SelectedItem == null)
            {
                MessageBox.Show("Please select an author to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView selectedAuthor = authorList.SelectedItem as DataRowView;
                int authorId = Convert.ToInt32(selectedAuthor["Id"]);

                // Delete related borrowing records first
                string deleteBorrowingQuery = "DELETE br FROM BorrowingRecords br JOIN Books b ON br.BookId = b.Id WHERE b.AuthorId = @AuthorId";
                using (SqlCommand sqlCommand = new SqlCommand(deleteBorrowingQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@AuthorId", authorId);
                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }

                // Delete related books
                string deleteBooksQuery = "DELETE FROM Books WHERE AuthorId = @AuthorId";
                using (SqlCommand sqlCommand = new SqlCommand(deleteBooksQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@AuthorId", authorId);
                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }

                // Delete the author
                string deleteAuthorQuery = "DELETE FROM Authors WHERE Id = @Id";
                using (SqlCommand sqlCommand = new SqlCommand(deleteAuthorQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", authorId);
                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Author deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayAuthors();
                bookList.ItemsSource = null; // Clear book list
                DisplayBorrowingRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting author: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddBookClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null || authorList.SelectedItem == null)
            {
                MessageBox.Show("Please select an author to add a book.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(bookTitle.Text))
            {
                MessageBox.Show("Book Title is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int publicationYear = 0; // Initialize with a default value
            if (!string.IsNullOrWhiteSpace(bookPublicationYear.Text))
            {
                if (!int.TryParse(bookPublicationYear.Text, out publicationYear))
                {
                    MessageBox.Show("Publication Year must be a valid integer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            try
            {
                DataRowView selectedAuthor = authorList.SelectedItem as DataRowView;
                int authorId = Convert.ToInt32(selectedAuthor["Id"]);

                string query = "INSERT INTO Books (Title, AuthorId, PublicationYear, ISBN) VALUES (@Title, @AuthorId, @PublicationYear, @ISBN)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Title", bookTitle.Text);
                    sqlCommand.Parameters.AddWithValue("@AuthorId", authorId);
                    sqlCommand.Parameters.AddWithValue("@PublicationYear", publicationYear == 0 ? (object)DBNull.Value : publicationYear);
                    sqlCommand.Parameters.AddWithValue("@ISBN", string.IsNullOrWhiteSpace(bookISBN.Text) ? (object)DBNull.Value : bookISBN.Text);

                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Book added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayBooksForAuthor(authorId);

                bookTitle.Text = "";
                bookPublicationYear.Text = "";
                bookISBN.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding book: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteBookClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null || bookList.SelectedItem == null)
            {
                MessageBox.Show("Please select a book to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView selectedBook = bookList.SelectedItem as DataRowView;
                int bookId = Convert.ToInt32(selectedBook["Id"]);

                // Delete related borrowing records
                string deleteBorrowingQuery = "DELETE FROM BorrowingRecords WHERE BookId = @BookId";
                using (SqlCommand sqlCommand = new SqlCommand(deleteBorrowingQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);
                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }

                // Delete the book
                string deleteBookQuery = "DELETE FROM Books WHERE Id = @Id";
                using (SqlCommand sqlCommand = new SqlCommand(deleteBookQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", bookId);
                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Book deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                DataRowView selectedAuthor = authorList.SelectedItem as DataRowView;
                int authorId = Convert.ToInt32(selectedAuthor["Id"]);
                DisplayBooksForAuthor(authorId);
                DisplayBorrowingRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting book: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BorrowBookClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null || bookList.SelectedItem == null)
            {
                MessageBox.Show("Please select a book to borrow.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(borrowerName.Text) || string.IsNullOrWhiteSpace(borrowDate.Text))
            {
                MessageBox.Show("Borrower Name and Borrow Date are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!DateTime.TryParse(borrowDate.Text, out DateTime borrowDateValue))
            {
                MessageBox.Show("Borrow Date must be in YYYY-MM-DD format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView selectedBook = bookList.SelectedItem as DataRowView;
                int bookId = Convert.ToInt32(selectedBook["Id"]);

                string query = "INSERT INTO BorrowingRecords (BookId, BorrowerName, BorrowDate) VALUES (@BookId, @BorrowerName, @BorrowDate)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);
                    sqlCommand.Parameters.AddWithValue("@BorrowerName", borrowerName.Text);
                    sqlCommand.Parameters.AddWithValue("@BorrowDate", borrowDateValue);

                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Book borrowed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayBorrowingRecords();

                borrowerName.Text = "";
                borrowDate.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error recording borrow: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReturnBookClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null || borrowingList.SelectedItem == null)
            {
                MessageBox.Show("Please select a borrowing record to return.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView selectedBorrowing = borrowingList.SelectedItem as DataRowView;
                int borrowingId = Convert.ToInt32(selectedBorrowing["Id"]);

                string query = "DELETE FROM BorrowingRecords WHERE Id = @Id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", borrowingId);
                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Book returned successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayBorrowingRecords();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error returning book: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}