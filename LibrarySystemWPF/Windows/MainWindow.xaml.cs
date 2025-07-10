using LibrarySystemWPF.Models;
using LibrarySystemWPF.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibrarySystemWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContentFrame.Navigate(new DashboardPage());
            SetUserPermissions();
        }

        private void SetUserPermissions()
        {
            bool isAdmin = false;

            if (UserSession.CurrentUser != null && UserSession.CurrentUser.Type == (int)UserType.Admin)
            {
                isAdmin = true;
            }

            if (!isAdmin)
            {
                ManageUsersButton.Visibility = Visibility.Collapsed;
                ManageBooksButton.Visibility = Visibility.Collapsed;
                ReturnsButton.Visibility = Visibility.Collapsed;
                ReportsButton.Visibility = Visibility.Collapsed;
                AnalysisButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ManageUsers_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new ManageUsersPage());
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new DashboardPage());
        }

        private void ManageBooks_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new ManageBooksPage());
        }

        private void BorrowBooks_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new BorrowBooksPage());
        }

        private void MyBooks_Click(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new MyBooksPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                UserSession.CurrentUser = null;
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }


    }
}
