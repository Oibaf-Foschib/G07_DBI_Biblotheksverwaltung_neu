using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace G07_DBI_Biblotheksverwaltung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLiteConnection connection;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToDatabase();
            LoadData();
        }

        private void ConnectToDatabase()
        {
            connection = new SQLiteConnection("Data Source=Datenbank;Version=3;");
            connection.Open();
        }

        private void LoadData()
        {
            LoadBooks();
            LoadUsers();
            LoadLoans();
        }

        private void LoadBooks()
        {
            string query = "SELECT * FROM Books";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Book> books = new List<Book>();

            while (reader.Read())
            {
                books.Add(new Book
                {
                    BookID = Convert.ToInt32(reader["BookID"]),
                    Title = reader["Title"].ToString(),
                    Author = reader["Author"].ToString(),
                    Genre = reader["Genre"].ToString(),
                    Year = Convert.ToInt32(reader["Year"])
                });
            }

            reader.Close();
            BooksDataGrid.ItemsSource = books;
        }

        private void LoadUsers()
        {
            string query = "SELECT * FROM Users";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<User> users = new List<User>();

            while (reader.Read())
            {
                users.Add(new User
                {
                    UserID = Convert.ToInt32(reader["UserID"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString()
                });
            }

            reader.Close();
            UsersDataGrid.ItemsSource = users;
        }

        private void LoadLoans()
        {
            string query = "SELECT * FROM BookLoans";
            SQLiteCommand command = new SQLiteCommand(query, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<BookLoan> loans = new List<BookLoan>();

            while (reader.Read())
            {
                loans.Add(new BookLoan
                {
                    BookTitle = reader["BookTitle"].ToString(),
                    BookAuthor = reader["BookAuthor"].ToString(),
                    UserName = reader["UserName"].ToString(),
                    UserEmail = reader["UserEmail"].ToString(),
                    LoanDate = Convert.ToDateTime(reader["LoanDate"]),
                    ReturnDate = Convert.ToDateTime(reader["ReturnDate"])
                });
            }

            reader.Close();
            LoansDataGrid.ItemsSource = loans;
        }
    }

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
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}