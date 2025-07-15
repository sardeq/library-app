using LibrarySystemWPF.Models;
using LibrarySystemWPF.Services;
using System;
using System.Windows;

namespace LibrarySystemWPF
{
    public partial class LoginWindow : Window
    {

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            var apiClient = new ApiClient();

            try
            {
                var result = await apiClient.Login(username, password);
                if (result != null)
                {
                    UserSession.Token = result.Token;
                    UserSession.CurrentUser = result.User;

                    apiClient.SetToken(UserSession.Token);
                    new MainWindow().Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid credentials");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }

    public class LoginResult
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}