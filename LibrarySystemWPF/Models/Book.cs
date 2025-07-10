using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemWPF.Models
{

    public class BookViewModel
    {
        public string BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int BooksAvailable { get; set; }
        public string BorrowTypeDesc { get; set; }
        public int BorrowDuration { get; set; }
        public bool IsSelected { get; set; }
    }

    public class Book
    {
        public string BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int BooksAvailable { get; set; }
        public int BorrowType { get; set; }
        public int BorrowDuration { get; set; }

        public string BorrowTypeDesc
        {
            get
            {
                switch (BorrowType)
                {
                    case 0: return "Student Only";
                    case 1: return "Teacher Only";
                    default: return "Unknown";
                }
            }
        }
    }
}
