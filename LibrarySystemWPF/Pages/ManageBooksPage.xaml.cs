// ManageBooksPage.xaml.cs
using LibrarySystemWPF.Models;
using LibrarySystemWPF.Services;
using System.Windows;
using System.Windows.Controls;

namespace LibrarySystemWPF.Pages
{
    public partial class ManageBooksPage : Page
    {
        private readonly BookService _bookService = new BookService();

        public ManageBooksPage()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            BooksGrid.ItemsSource = _bookService.GetAllBooks();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var bookDialog = new BookDialog();
            if (bookDialog.ShowDialog() == true)
            {
                if (_bookService.AddBook(bookDialog.Book))
                {
                    LoadBooks();
                    MessageBox.Show("Book added successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to add book.");
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Book selectedBook)
            {
                var bookDialog = new BookDialog(selectedBook);
                if (bookDialog.ShowDialog() == true)
                {
                    if (_bookService.UpdateBook(bookDialog.Book))
                    {
                        LoadBooks();
                        MessageBox.Show("Book updated successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to update book.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a book to edit.");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem is Book selectedBook)
            {
                var (success, error) = _bookService.DeleteBook(selectedBook.BookID);

                if (success)
                {
                    LoadBooks();
                    MessageBox.Show("Book deleted successfully!");
                }
                else
                {
                    MessageBox.Show($"Failed to delete book: {error}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a book to delete.");
            }
        }
    }
}