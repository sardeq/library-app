using LibrarySystemWPF.Services;
using System;
using System.Data.SqlClient;
using System.Windows;

namespace LibrarySystemWPF
{
    public partial class LoginWindow : Window
    {
        private readonly DatabaseService _db = new DatabaseService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (ValidateUser(username, password))
            {
                new MainWindow().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid credentials");
            }
        }

        private bool ValidateUser(string username, string password)
        {
            string query = "SELECT COUNT(1) FROM Clients WHERE ClientName = @Username AND Password = @Password";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };

            try
            {
                var result = _db.ExecuteScalar(query, parameters);
                return (result != null && Convert.ToInt32(result) == 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
                return false;
            }
        }
    }
}