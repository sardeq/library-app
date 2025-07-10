using LibrarySystemWPF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibrarySystemWPF.Services
{
    public class BookService
    {
        private readonly DatabaseService _db = new DatabaseService();

        public List<Book> GetAllBooks()
        {
            string query = "SELECT * FROM Books";
            DataTable dt = _db.GetData(query);
            List<Book> books = new List<Book>();

            foreach (DataRow row in dt.Rows)
            {
                books.Add(new Book
                {
                    BookID = row["BookID"].ToString(),
                    Title = row["Title"].ToString(),
                    Author = row["Author"].ToString(),
                    ReleaseDate = Convert.ToDateTime(row["ReleaseDate"]),
                    BooksAvailable = Convert.ToInt32(row["BooksAvailable"]),
                    BorrowType = Convert.ToInt32(row["BorrowType"]),
                    BorrowDuration = Convert.ToInt32(row["BorrowDuration"])
                });
            }
            return books;
        }

        public bool AddBook(Book book)
        {
            string query = @"INSERT INTO Books 
                (BookID, Title, Author, ReleaseDate, BooksAvailable, BorrowType, BorrowDuration)
                VALUES (@BookID, @Title, @Author, @ReleaseDate, @BooksAvailable, @BorrowType, @BorrowDuration)";

            SqlParameter[] parameters = {
                new SqlParameter("@BookID", book.BookID),
                new SqlParameter("@Title", book.Title),
                new SqlParameter("@Author", book.Author),
                new SqlParameter("@ReleaseDate", book.ReleaseDate),
                new SqlParameter("@BooksAvailable", book.BooksAvailable),
                new SqlParameter("@BorrowType", book.BorrowType),
                new SqlParameter("@BorrowDuration", book.BorrowDuration)
            };

            return _db.ExecuteNonQuery(query, parameters);
        }

        public bool UpdateBook(Book book)
        {
            string query = @"UPDATE Books SET 
                Title = @Title,
                Author = @Author,
                ReleaseDate = @ReleaseDate,
                BooksAvailable = @BooksAvailable,
                BorrowType = @BorrowType,
                BorrowDuration = @BorrowDuration
                WHERE BookID = @BookID";

            SqlParameter[] parameters = {
                new SqlParameter("@BookID", book.BookID),
                new SqlParameter("@Title", book.Title),
                new SqlParameter("@Author", book.Author),
                new SqlParameter("@ReleaseDate", book.ReleaseDate),
                new SqlParameter("@BooksAvailable", book.BooksAvailable),
                new SqlParameter("@BorrowType", book.BorrowType),
                new SqlParameter("@BorrowDuration", book.BorrowDuration)
            };

            return _db.ExecuteNonQuery(query, parameters);
        }

        public (bool Success, string Error) DeleteBook(string bookId)
        {
            string borrowCheck = "SELECT COUNT(*) FROM Borrow WHERE BookID = @BookID AND Returned = 0";
            SqlParameter[] borrowParams = { new SqlParameter("@BookID", bookId) };

            if (Convert.ToInt32(_db.ExecuteScalar(borrowCheck, borrowParams)) > 0)
            {
                return (false, "Book cannot be deleted as it is currently borrowed");
            }

            string deleteQuery = "DELETE FROM Books WHERE BookID = @BookID";
            SqlParameter[] deleteParams = { new SqlParameter("@BookID", bookId) };

            bool success = _db.ExecuteNonQuery(deleteQuery, deleteParams);
            return (success, success ? "" : "Failed to delete book");
        }


        public List<Book> GetAvailableBooks(int userType)
        {
            string query = "SELECT * FROM Books WHERE BooksAvailable > 0";
            if (userType == 0) // Student
            {
                query += " AND BorrowType = 0";
            }

            DataTable dt = _db.GetData(query);
            List<Book> books = new List<Book>();

            foreach (DataRow row in dt.Rows)
            {
                books.Add(new Book
                {
                    BookID = row["BookID"].ToString(),
                    Title = row["Title"].ToString(),
                    Author = row["Author"].ToString(),
                    ReleaseDate = Convert.ToDateTime(row["ReleaseDate"]),
                    BooksAvailable = Convert.ToInt32(row["BooksAvailable"]),
                    BorrowType = Convert.ToInt32(row["BorrowType"]),
                    BorrowDuration = Convert.ToInt32(row["BorrowDuration"])
                });
            }
            return books;
        }

        public List<BorrowedBook> GetBorrowedBooks(int clientId)
        {
            string query = @"
                SELECT b.BookID, bk.Title, b.BorrowDate, b.ReturnDate
                FROM Borrow b
                INNER JOIN Books bk ON b.BookID = bk.BookID
                WHERE b.ClientID = @ClientID 
                AND b.PendingConfirmation = 0
                AND b.Returned = 0";

            SqlParameter[] parameters = { new SqlParameter("@ClientID", clientId) };
            DataTable dt = _db.GetDataN(query, parameters);

            List<BorrowedBook> borrowedBooks = new List<BorrowedBook>();
            foreach (DataRow row in dt.Rows)
            {
                borrowedBooks.Add(new BorrowedBook
                {
                    BookID = row["BookID"].ToString(),
                    Title = row["Title"].ToString(),
                    BorrowDate = Convert.ToDateTime(row["BorrowDate"]),
                    DueDate = Convert.ToDateTime(row["ReturnDate"])
                });
            }
            return borrowedBooks;
        }

        public bool RequestReturn(int clientId, string bookId)
        {
            string query = @"UPDATE Borrow SET PendingConfirmation = 1
                   WHERE ClientID = @ClientID AND BookID = @BookID";

            SqlParameter[] parameters = {
                new SqlParameter("@ClientID", clientId),
                new SqlParameter("@BookID", bookId),
            };
            return _db.ExecuteNonQuery(query, parameters);
        }

        public (bool Success, List<string> Errors) BorrowBooks(int clientId, List<string> bookIds)
        {
            var errors = new List<string>();
            var user = UserSession.CurrentUser;

            if (user.Type != (int)UserType.Admin && user.Status == (int)Status.Blocked)
            {
                errors.Add("Account is blocked.");
                return (false, errors);
            }

            // Check for overdue books (non-admins only)
            if (user.Type != (int)UserType.Admin && HasOverdueBooks(clientId))
            {
                errors.Add("Account blocked due to overdue books.");
                return (false, errors);
            }

            // Check borrowing quota (non-admins only)
            int currentBorrowed = GetBorrowedCount(clientId);
            if (user.Type != (int)UserType.Admin && user.BooksQuota - currentBorrowed < bookIds.Count)
            {
                errors.Add($"Quota exceeded. Available: {user.BooksQuota - currentBorrowed}");
                return (false, errors);
            }

            bool anySuccess = false;

            // Process each book
            foreach (var bookId in bookIds)
            {
                var book = GetBook(bookId);
                if (book == null)
                {
                    errors.Add($"Book {bookId} not found.");
                    continue;
                }

                if (book.BooksAvailable <= 0)
                {
                    errors.Add($"'{book.Title}' is unavailable.");
                    continue;
                }

                if (user.Type != (int)UserType.Admin && user.Type == (int)UserType.Student && book.BorrowType != 0)
                {
                    errors.Add($"'{book.Title}' is teacher-only.");
                    continue;
                }

                if (BorrowBook(clientId, bookId, user.BorrowDuration, book.BorrowDuration))
                {
                    anySuccess = true;
                }
                else
                {
                    errors.Add($"Failed to borrow '{book.Title}'. You may have already borrowed this book.");
                }
            }

            return (anySuccess, errors);
        }

        private Book GetBook(string bookId)
        {
            string query = "SELECT * FROM Books WHERE BookID = @BookID";
            SqlParameter[] parameters = { new SqlParameter("@BookID", bookId) };
            DataTable dt = _db.GetDataN(query, parameters);

            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            return new Book
            {
                BookID = bookId,
                Title = row["Title"].ToString(),
                Author = row["Author"].ToString(),
                BooksAvailable = Convert.ToInt32(row["BooksAvailable"]),
                BorrowType = Convert.ToInt32(row["BorrowType"]),
                BorrowDuration = Convert.ToInt32(row["BorrowDuration"])
            };
        }

        private int GetBorrowedCount(int clientId)
        {
            string query = @"SELECT COUNT(*) FROM Borrow 
                     WHERE ClientID = @ClientID 
                     AND Returned = 0 
                     AND PendingConfirmation = 0";
            SqlParameter[] parameters = { new SqlParameter("@ClientID", clientId) };
            return Convert.ToInt32(_db.ExecuteScalar(query, parameters));
        }

        public bool HasOverdueBooks(int clientId)
        {
            string query = @"SELECT COUNT(*) FROM Borrow 
                     WHERE ClientID = @ClientID 
                     AND ReturnDate < GETDATE() 
                     AND Returned = 0 AND PendingConfirmation = 0";
            SqlParameter[] parameters = { new SqlParameter("@ClientID", clientId) };
            return Convert.ToInt32(_db.ExecuteScalar(query, parameters)) > 0;
        }

        private bool BorrowBook(int clientId, string bookId, int userDuration, int bookDuration)
        {
            try
            {
                int duration = userDuration > 0 ? userDuration : bookDuration;
                string query = @"
            INSERT INTO Borrow (ClientID, BookID, BorrowDate, ReturnDate, Returned, PendingConfirmation)
            VALUES (@ClientID, @BookID, GETDATE(), DATEADD(day, @Duration, GETDATE()), 0, 0)";
                SqlParameter[] insertParams = {
            new SqlParameter("@ClientID", clientId),
            new SqlParameter("@BookID", bookId),
            new SqlParameter("@Duration", duration)
        };
                _db.ExecuteNonQuery(query, insertParams);

                string updateQuery = "UPDATE Books SET BooksAvailable = BooksAvailable - 1 WHERE BookID = @BookID";
                _db.ExecuteNonQuery(updateQuery, new[] { new SqlParameter("@BookID", bookId) });
                return true;
            }
            catch (SqlException ex) when (ex.Number == 2627) // Duplicate borrow attempt
            {
                return false;
            }
            catch
            {
                return false;
            }
        }
    }

    public class BorrowedBook
    {
        public string BookID { get; set; }
        public string Title { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
