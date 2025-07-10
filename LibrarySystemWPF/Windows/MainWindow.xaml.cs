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
    }
}
