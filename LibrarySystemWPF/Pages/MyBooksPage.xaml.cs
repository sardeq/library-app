// MyBooksPage.xaml.cs
using LibrarySystemWPF.Models;
using LibrarySystemWPF.Services;
using System.Windows;
using System.Windows.Controls;

namespace LibrarySystemWPF.Pages
{
    public partial class MyBooksPage : Page
    {
        private readonly BookService _bookService = new BookService();

        public MyBooksPage()
        {
            InitializeComponent();
            LoadMyBooks();
        }

        private void LoadMyBooks()
        {
            if (UserSession.CurrentUser == null) return;

            var borrowedBooks = _bookService.GetBorrowedBooks(UserSession.CurrentUser.ClientID);
            BooksGrid.ItemsSource = borrowedBooks;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserSession.CurrentUser == null) return;

            var button = (Button)sender;
            string bookId = button.Tag.ToString();

            if (_bookService.RequestReturn(UserSession.CurrentUser.ClientID, bookId))
            {
                MessageBox.Show("Return request submitted. Please wait for confirmation.");
                LoadMyBooks();
            }
            else
            {
                MessageBox.Show("Failed to submit return request. Please try again.");
            }
        }
    }
}