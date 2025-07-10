using System;
using System.Windows;
using LibrarySystemWPF.Models;

namespace LibrarySystemWPF
{
    public partial class BookDialog : Window
    {
        public Book Book { get; set; }

        public BookDialog(Book book = null)
        {
            InitializeComponent();
            Book = book ?? new Book
            {
                BookID = Guid.NewGuid().ToString(),
                ReleaseDate = DateTime.Today,
                BooksAvailable = 1,
                BorrowDuration = 7
            };

            DataContext = Book;
            TxtBookId.Text = Book.BookID;
            TxtTitle.Text = Book.Title;
            TxtAuthor.Text = Book.Author;
            DpReleaseDate.SelectedDate = Book.ReleaseDate;
            TxtAvailable.Text = Book.BooksAvailable.ToString();
            CbBorrowType.SelectedIndex = Book.BorrowType;
            TxtDuration.Text = Book.BorrowDuration.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtTitle.Text))
            {
                MessageBox.Show("Title is required");
                return;
            }

            Book.Title = TxtTitle.Text;
            Book.Author = TxtAuthor.Text;
            Book.ReleaseDate = DpReleaseDate.SelectedDate ?? DateTime.Today;
            Book.BooksAvailable = int.TryParse(TxtAvailable.Text, out int avail) ? avail : 1;
            Book.BorrowType = CbBorrowType.SelectedIndex;
            Book.BorrowDuration = int.TryParse(TxtDuration.Text, out int dur) ? dur : 7;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}