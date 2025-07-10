using LibrarySystemWPF.Models;
using LibrarySystemWPF.Services;
using System;
using System.Data.SqlClient;
using System.Windows;

namespace LibrarySystemWPF
{
    public partial class LoginWindow : Window
    {
        private readonly DatabaseService _db = new DatabaseService();
        private readonly UserService userService = new UserService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            var user = userService.GetUserByUsername(username);
            if (user != null && user.Password == password)
            {
                UserSession.CurrentUser = user;
                new MainWindow().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid credentials");
            }
        }
    }
}