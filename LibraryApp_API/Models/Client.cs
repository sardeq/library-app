using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp_API.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int LanguageID { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public int GenderID { get; set; }
        public int StatusID { get; set; }
        public int TypeID { get; set; }
        public string Password { get; set; }
        public int BooksQuota { get; set; }
        public int BorrowDuration { get; set; }

        public virtual Gender GenderRef { get; set; }
        public virtual Status StatusRef { get; set; }
        public virtual Type TypeRef { get; set; }
        public virtual Language LanguageRef { get; set; }
    }

    public class Gender
    {
        public int GenderID { get; set; }
        public string GenderDesc { get; set; }
    }

    public class Status
    {
        public int StatusID { get; set; }
        public string StatusDesc { get; set; }
    }

    public class Type
    {
        public int TypeID { get; set; }
        public string TypeDesc { get; set; }
    }

    public class Language
    {
        public int LanguageID { get; set; }
        public string LanguageDesc { get; set; }
    }
}
