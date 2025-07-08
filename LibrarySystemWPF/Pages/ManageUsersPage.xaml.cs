// ManageUsersPage.xaml.cs
using LibrarySystemWPF.Models;
using LibrarySystemWPF.Services;
using System.Windows;
using System.Windows.Controls;

namespace LibrarySystemWPF.Pages
{
    public partial class ManageUsersPage : Page
    {
        private readonly UserService _userService = new UserService();

        public ManageUsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            UsersGrid.ItemsSource = _userService.GetAllUsers();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var userDialog = new UserDialog(new User());
            if (userDialog.ShowDialog() == true)
            {
                if (_userService.SaveUser(userDialog.User))
                {
                    LoadUsers();
                    MessageBox.Show("User added successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to add user.");
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is User selectedUser)
            {
                var userDialog = new UserDialog(selectedUser);
                if (userDialog.ShowDialog() == true)
                {
                    if (_userService.SaveUser(userDialog.User))
                    {
                        LoadUsers();
                        MessageBox.Show("User updated successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to update user.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to edit.");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is User selectedUser)
            {
                if (MessageBox.Show($"Are you sure you want to delete user '{selectedUser.ClientName}'?",
                    "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (_userService.DeleteUser(selectedUser.ClientID))
                    {
                        LoadUsers();
                        MessageBox.Show("User deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete user.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.");
            }
        }
    }
}