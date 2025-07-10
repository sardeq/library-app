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
    }
}