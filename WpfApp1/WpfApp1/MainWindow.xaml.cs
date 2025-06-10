using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private SqlConnection sqlConnection;
        private DataTable productTable;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["WpfApp1.Properties.Settings.DerekTutorialDBConnectionString"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Connection string 'WpfApp1.Properties.Settings.DerekTutorialDBConnectionString' not found in App.config.", "Configuration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                MessageBox.Show("Database connection opened successfully.", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                DisplayStores();
                DisplayAllProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize database connection: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayStores()
        {
            if (sqlConnection == null) return;

            try
            {
                string query = "SELECT * FROM Store";
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    DataTable storeTable = new DataTable();
                    sqlDataAdapter.Fill(storeTable);

                    MessageBox.Show($"Retrieved {storeTable.Rows.Count} stores", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    string dataLog = "Store Data:\n";
                    foreach (DataRow row in storeTable.Rows)
                    {
                        dataLog += $"Id={row["Id"]}, Name={row["Name"]}, City={row["City"]}\n";
                    }
                    MessageBox.Show(dataLog, "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    storeList.DisplayMemberPath = "Name";
                    storeList.ItemsSource = storeTable.DefaultView;
                    storeList.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stores: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayAllProducts()
        {
            if (sqlConnection == null) return;

            try
            {
                string query = "SELECT * FROM Product";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    productTable = new DataTable();
                    sqlDataAdapter.Fill(productTable);

                    MessageBox.Show($"Retrieved {productTable.Rows.Count} products", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    productList.DisplayMemberPath = "Brand";
                    productList.SelectedValuePath = "Id";
                    productList.ItemsSource = productTable.DefaultView;
                    productList.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (productTable == null) return;

            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            string filter = "";
            if (textBox.Name == "manufacturerTextBox")
            {
                filter = $"Manufacturer LIKE '%{textBox.Text}%'";
            }
            else if (textBox.Name == "brandTextBox")
            {
                filter = $"Brand LIKE '%{textBox.Text}%'";
            }

            if (!string.IsNullOrEmpty(filter))
            {
                DataView dv = productTable.DefaultView;
                dv.RowFilter = filter;
                productList.ItemsSource = dv;
            }
            else
            {
                productList.ItemsSource = productTable.DefaultView;
            }
        }

        private void storeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sqlConnection == null || storeList.SelectedItem == null) return;

            try
            {
                DataRowView selectedStore = storeList.SelectedItem as DataRowView;
                if (selectedStore != null)
                {
                    int storeId = Convert.ToInt32(selectedStore["Id"]);
                    string query = "SELECT * FROM Inventory WHERE StoreId = @StoreId";
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@StoreId", storeId);
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                        {
                            DataTable inventoryTable = new DataTable();
                            sqlDataAdapter.Fill(inventoryTable);

                            MessageBox.Show($"Retrieved {inventoryTable.Rows.Count} inventory items for StoreId {storeId}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                            storeInventory.DisplayMemberPath = "ProductName";
                            storeInventory.ItemsSource = inventoryTable.DefaultView;
                            storeInventory.Items.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading inventory: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddStoreClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null)
            {
                MessageBox.Show("Database connection is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(storeName.Text) || string.IsNullOrWhiteSpace(storeCity.Text))
            {
                MessageBox.Show("Store Name and City are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(storeZipcode.Text) && !System.Text.RegularExpressions.Regex.IsMatch(storeZipcode.Text, @"^\d{5}(-\d{4})?$"))
            {
                MessageBox.Show("Invalid Zipcode format. Use 12345 or 12345-6789.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string logMessage = $"Attempting to insert: Name={storeName.Text}, Street={storeStreet.Text}, City={storeCity.Text}, State={storeState.Text}, Zip={storeZipcode.Text}";
                MessageBox.Show(logMessage, "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);

                string query = "INSERT INTO Store (Name, Street, City, State, Zip) VALUES (@Name, @Street, @City, @State, @Zip)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Name", storeName.Text);
                    sqlCommand.Parameters.AddWithValue("@Street", storeStreet.Text);
                    sqlCommand.Parameters.AddWithValue("@City", storeCity.Text);
                    sqlCommand.Parameters.AddWithValue("@State", storeState.Text);
                    sqlCommand.Parameters.AddWithValue("@Zip", storeZipcode.Text);

                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        MessageBox.Show("Connection is closed. Attempting to open...", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        sqlConnection.Open();
                    }

                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    MessageBox.Show($"Rows affected: {rowsAffected}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                MessageBox.Show("Store added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayStores();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding store: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddInventoryClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null)
            {
                MessageBox.Show("Database connection is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (storeList.SelectedItem == null)
            {
                MessageBox.Show("Please select a store to add inventory.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(inventoryProductName.Text))
            {
                MessageBox.Show("Product Name is required for inventory.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView selectedStore = storeList.SelectedItem as DataRowView;
                int storeId = Convert.ToInt32(selectedStore["Id"]);
                string productName = inventoryProductName.Text;

                string query = "INSERT INTO Inventory (StoreId, ProductName) VALUES (@StoreId, @ProductName)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@StoreId", storeId);
                    sqlCommand.Parameters.AddWithValue("@ProductName", productName);

                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }

                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    MessageBox.Show($"Rows affected: {rowsAffected}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                MessageBox.Show("Inventory item added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                storeList_SelectionChanged(null, null);
                inventoryProductName.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding inventory: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProductClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null) return;

            if (string.IsNullOrWhiteSpace(brandTextBox.Text))
            {
                MessageBox.Show("Brand is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string query = "INSERT INTO Product (Manufacturer, Brand) VALUES (@Manufacturer, @Brand)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Manufacturer", manufacturerTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("@Brand", brandTextBox.Text);

                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Product added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayAllProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteStoreClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null || storeList.SelectedItem == null) return;

            try
            {
                DataRowView selectedStore = storeList.SelectedItem as DataRowView;
                if (selectedStore == null) return;

                int storeId = Convert.ToInt32(selectedStore["Id"]);

                string deleteInventoryQuery = "DELETE FROM Inventory WHERE StoreId = @StoreId";
                using (SqlCommand sqlCommand = new SqlCommand(deleteInventoryQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@StoreId", storeId);
                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }

                string deleteStoreQuery = "DELETE FROM Store WHERE Id = @Id";
                using (SqlCommand sqlCommand = new SqlCommand(deleteStoreQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", storeId);
                    if (sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Store deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayStores();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting store: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteInventoryClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null)
            {
                MessageBox.Show("Database connection is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (storeInventory.SelectedItem == null)
            {
                MessageBox.Show("Please select an inventory item to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView selectedInventory = storeInventory.SelectedItem as DataRowView;
                int inventoryId = Convert.ToInt32(selectedInventory["Id"]);

                string query = "DELETE FROM Inventory WHERE Id = @Id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", inventoryId);

                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }

                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    MessageBox.Show($"Rows affected: {rowsAffected}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                MessageBox.Show("Inventory item deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                storeList_SelectionChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting inventory: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteProductClick(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null)
            {
                MessageBox.Show("Database connection is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (productList.SelectedItem == null)
            {
                MessageBox.Show("Please select a product to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView selectedProduct = productList.SelectedItem as DataRowView;
                int productId = Convert.ToInt32(selectedProduct["Id"]);
                string productBrand = selectedProduct["Brand"].ToString();

                string deleteInventoryQuery = "DELETE FROM Inventory WHERE ProductName = @ProductName";
                using (SqlCommand sqlCommand = new SqlCommand(deleteInventoryQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ProductName", productBrand);
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }
                    sqlCommand.ExecuteNonQuery();
                }

                string deleteProductQuery = "DELETE FROM Product WHERE Id = @Id";
                using (SqlCommand sqlCommand = new SqlCommand(deleteProductQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", productId);
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        sqlConnection.Open();
                    }

                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    MessageBox.Show($"Rows affected: {rowsAffected}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DisplayAllProducts();
                storeList_SelectionChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting product: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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