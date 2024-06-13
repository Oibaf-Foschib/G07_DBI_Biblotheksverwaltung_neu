using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G07_DBI_Biblotheksverwaltung
{
    public class User_Book_BookLoan
    {
        public class Book
        {
            public int BookID { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public string Genre { get; set; }
            public int Year { get; set; }
        }

        public class User
        {
            public int UserID { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public class BookLoan
        {
            public int LoanID { get; set; }
            public int BookID { get; set; }
            public int UserID { get; set; }
            public string BookTitle { get; set; }
            public string BookAuthor { get; set; }
            public string UserName { get; set; }
            public string UserEmail { get; set; }
            public DateTime LoanDate { get; set; }
            public DateTime ReturnDate { get; set; }
        }
    }
}
