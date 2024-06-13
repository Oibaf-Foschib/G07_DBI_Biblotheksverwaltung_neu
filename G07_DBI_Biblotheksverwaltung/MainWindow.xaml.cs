using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using static G07_DBI_Biblotheksverwaltung.User_Book_BookLoan;

namespace G07_DBI_Biblotheksverwaltung
{
    public partial class MainWindow : Window
    {
        private SQLiteConnection connection;
        private ObservableCollection<Book> books;
        private ObservableCollection<User> users;
        private ObservableCollection<BookLoan> loans;

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
                string projectDirectory = System.IO.Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
                string dbPath = System.IO.Path.Combine(projectDirectory, "Datenbank.db");
                connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
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
            books = new ObservableCollection<Book>();
            string query = "SELECT * FROM Books";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
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
                }
            }
            BooksDataGrid.ItemsSource = books;
        }

        private void LoadUsers()
        {
            users = new ObservableCollection<User>();
            string query = "SELECT * FROM Users";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString()
                        });
                    }
                }
            }
            UsersDataGrid.ItemsSource = users;
        }

        private void LoadLoans()
        {
            loans = new ObservableCollection<BookLoan>();
            string query = "SELECT Loans.LoanID, Books.Title AS BookTitle, Books.Author AS BookAuthor, Users.Name AS UserName, Users.Email AS UserEmail, Loans.LoanDate, Loans.ReturnDate " +
                           "FROM Loans " +
                           "INNER JOIN Books ON Loans.BookID = Books.BookID " +
                           "INNER JOIN Users ON Loans.UserID = Users.UserID";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loans.Add(new BookLoan
                        {
                            LoanID = Convert.ToInt32(reader["LoanID"]),
                            BookTitle = reader["BookTitle"].ToString(),
                            BookAuthor = reader["BookAuthor"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            UserEmail = reader["UserEmail"].ToString(),
                            LoanDate = Convert.ToDateTime(reader["LoanDate"]),
                            ReturnDate = Convert.ToDateTime(reader["ReturnDate"])
                        });
                    }
                }
            }
            LoansDataGrid.ItemsSource = loans;
        }

        private void BtnSearchUser_Click(object sender, RoutedEventArgs e)
        {
            string searchText = TxtSearchUsers.Text.ToLower().Trim();
            var filteredUsers = new ObservableCollection<User>();

            foreach (User user in users)
            {
                if (user.Name.ToLower().Contains(searchText) || user.Email.ToLower().Contains(searchText))
                {
                    filteredUsers.Add(user);
                }
            }
            UsersDataGrid.ItemsSource = filteredUsers;
        }

        private void BtnSearchBook_Click(object sender, RoutedEventArgs e)
        {
            string searchText = TxtSearch.Text.ToLower().Trim();
            var filteredBooks = new ObservableCollection<Book>();

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

        private void BtnSearchLoans_Click(object sender, RoutedEventArgs e)
        {
            string searchText = TxtSearchLoans.Text.ToLower().Trim();
            var filteredLoans = new ObservableCollection<BookLoan>();

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

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                try
                {
                    string query = "DELETE FROM Books WHERE BookID = @BookID";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookID", selectedBook.BookID);
                        command.ExecuteNonQuery();
                        Logger.Log($"Buch mit BookID: {selectedBook.BookID} gelöscht.");
                    }
                    books.Remove(selectedBook);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Fehler beim Löschen des Buchs: {ex.Message}");
                    MessageBox.Show("Ein Fehler ist aufgetreten: " + ex.Message);
                }
            }
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                try
                {
                    string query = "DELETE FROM Users WHERE UserID = @UserID";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", selectedUser.UserID);
                        command.ExecuteNonQuery();
                        Logger.Log($"Benutzer mit UserID: {selectedUser.UserID} gelöscht.");
                    }
                    users.Remove(selectedUser);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Fehler beim Löschen des Benutzers: {ex.Message}");
                    MessageBox.Show("Ein Fehler ist aufgetreten: " + ex.Message);
                }
            }
        }

        private void AddLoanButton_Click(object sender, RoutedEventArgs e)
        {
            LoanWindow loanWindow = new LoanWindow(connection);
            if (loanWindow.ShowDialog() == true)
            {
                loans.Add(loanWindow.NewLoan);
                Logger.Log("Ausleihliste nach Hinzufügen einer neuen Ausleihe neu geladen.");
            }
        }

        private void EditLoanButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoansDataGrid.SelectedItem is BookLoan selectedLoan)
            {
                LoanWindow loanWindow = new LoanWindow(selectedLoan, connection);
                if (loanWindow.ShowDialog() == true)
                {
                    int index = loans.IndexOf(selectedLoan);
                    loans[index] = loanWindow.NewLoan;
                    LoansDataGrid.ItemsSource = null;
                    LoansDataGrid.ItemsSource = loans;
                    Logger.Log("Ausleihliste nach Bearbeiten einer Ausleihe neu geladen.");
                }
            }
        }

        private void DeleteLoanButton_Click(object sender, RoutedEventArgs e)
        {
            if (LoansDataGrid.SelectedItem is BookLoan selectedLoan)
            {
                try
                {
                    string query = "DELETE FROM Loans WHERE LoanID = @LoanID";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LoanID", selectedLoan.LoanID);
                        command.ExecuteNonQuery();
                        Logger.Log($"Ausleihe mit LoanID: {selectedLoan.LoanID} gelöscht.");
                    }
                    loans.Remove(selectedLoan);
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Fehler beim Löschen der Ausleihe: {ex.Message}");
                    MessageBox.Show("Ein Fehler ist aufgetreten: " + ex.Message);
                }
            }
        }
    }
}
