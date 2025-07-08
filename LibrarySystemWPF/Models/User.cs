using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemWPF.Models
{
    public class User
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int LanguageID { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string Password { get; set; }
        public int BooksQuota { get; set; }
        public int BorrowDuration { get; set; }
        public string GenderDesc { get; set; }
        public string StatusDesc { get; set; }
        public string TypeDesc { get; set; }
        public string LanguageDesc { get; set; }
    }
}
