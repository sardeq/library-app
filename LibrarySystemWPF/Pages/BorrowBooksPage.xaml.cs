// BorrowBooksPage.xaml.cs
using LibrarySystemWPF.Models;
using LibrarySystemWPF.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibrarySystemWPF.Pages
{
    public partial class BorrowBooksPage : Page
    {
        private readonly BookService _bookService = new BookService();
        private List<Book> _allBooks = new List<Book>();

        public BorrowBooksPage()
        {
            InitializeComponent();
            LoadAvailableBooks();
        }

        private void LoadAvailableBooks(string searchTerm = null)
        {
            int userType = UserSession.CurrentUser?.Type ?? 0;
            _allBooks = _bookService.GetAvailableBooks(userType);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                _allBooks = _allBooks.Where(b =>
                    b.Title.ToLower().Contains(searchTerm) ||
                    b.Author.ToLower().Contains(searchTerm)
                ).ToList();
            }

            BooksGrid.ItemsSource = _allBooks.Select(b => new
            {
                b.BookID,
                b.Title,
                b.Author,
                b.BooksAvailable,
                BorrowTypeDesc = b.BorrowType == 0 ? "Everyone" : "Teachers Only",
                b.BorrowDuration,
                IsSelected = false
            }).ToList();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadAvailableBooks(SearchTextBox.Text);
        }

        private void BorrowButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBooks = BooksGrid.ItemsSource
                .Cast<dynamic>()
                .Where(b => b.IsSelected)
                .Select(b => (string)b.BookID)
                .ToList();

            if (selectedBooks.Count == 0)
            {
                MessageBox.Show("Please select at least one book to borrow.");
                return;
            }

            var (success, errors) = _bookService.BorrowBooks(UserSession.CurrentUser.ClientID, selectedBooks);
            if (success)
            {
                MessageBox.Show("Books borrowed successfully.");
                LoadAvailableBooks(); 
            }
            else
            {
                string errorMessage = string.Join("\n", errors);
                MessageBox.Show($"Borrowing failed:\n{errorMessage}");
            }
        }
    }
}