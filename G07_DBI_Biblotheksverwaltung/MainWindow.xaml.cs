using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;
using static G07_DBI_Biblotheksverwaltung.User_Book_BookLoan;

namespace G07_DBI_Biblotheksverwaltung
{
    /// <summary>
    /// Interaction logic for MainWindow.xamlD
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLiteConnection connection;

        private List<Book> books = new List<Book>();
        private List<User> users = new List<User>();
        private List<BookLoan> loans = new List<BookLoan>();

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
            books.Clear();

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
            users.Clear();

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
            loans.Clear();

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

        private void BtnSearchUser_Click(object sender, RoutedEventArgs e)
        {
            string searchText = TxtSearchUsers.Text.ToLower().Trim();
            List<User> filteredUsers = new List<User>();

            if (string.IsNullOrEmpty(searchText))
            {
                UsersDataGrid.ItemsSource = users;
            }
            else
            {
                foreach (User user in users)
                {
                    if (user.Name.ToLower().Contains(searchText) || user.Email.ToLower().Contains(searchText))
                    {
                        filteredUsers.Add(user);
                    }
                }
                UsersDataGrid.ItemsSource = filteredUsers;
            }
        }

        private void BtnSearchBook_Click(object sender, RoutedEventArgs e)
        {
            string searchText = TxtSearch.Text.ToLower().Trim();
            List<Book> filteredBooks = new List<Book>();

            if (string.IsNullOrEmpty(searchText))
            {
                BooksDataGrid.ItemsSource = books;
            }
            else
            {
                foreach (Book book in books)
                {
                    if (book.Title.ToLower().Contains(searchText) ||
                        book.Author.ToLower().Contains(searchText) ||
                        book.Genre.ToLower().Contains(searchText))
                    {
                        filteredBooks.Add(book); 
                    }
                }
                BooksDataGrid.ItemsSource = filteredBooks; 
            }
        }

        private void BtnSearchLoans_Click(object sender, RoutedEventArgs e)
        {
            string searchText = TxtSearchLoans.Text.ToLower().Trim();
            List<BookLoan> filteredLoans = new List<BookLoan>();

            if (string.IsNullOrEmpty(searchText))
            {
                LoansDataGrid.ItemsSource = loans;
            }
            else
            {
                foreach (BookLoan loan in loans)
                {
                    if (loan.BookTitle.ToLower().Contains(searchText) ||
                        loan.UserName.ToLower().Contains(searchText) ||
                        loan.UserEmail.ToLower().Contains(searchText))
                    {
                        filteredLoans.Add(loan);
                    }
                }
                LoansDataGrid.ItemsSource = filteredLoans;
            }
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