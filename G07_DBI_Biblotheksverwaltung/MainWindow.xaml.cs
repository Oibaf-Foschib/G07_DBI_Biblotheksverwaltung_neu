using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace G07_DBI_Biblotheksverwaltung
{
    /// <summary>
    /// Interaction logic for MainWindow.xamlD
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
            try
            {
                connection = new SQLiteConnection("Data Source=Datenbank.db;Version=3;");
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Verbinden zur Datenbank: " + ex.Message);
            }
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
            List<User_Book_BookLoan.Book> books = new List<User_Book_BookLoan.Book>();

            while (reader.Read())
            {
                books.Add(new User_Book_BookLoan.Book
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
            List<User_Book_BookLoan.User> users = new List<User_Book_BookLoan.User>();

            while (reader.Read())
            {
                users.Add(new User_Book_BookLoan.User
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
            List<User_Book_BookLoan.BookLoan> loans = new List<User_Book_BookLoan.BookLoan>();

            while (reader.Read())
            {
                loans.Add(new User_Book_BookLoan.BookLoan
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

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddLoanButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditLoanButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteLoanButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnSearchUsers_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}