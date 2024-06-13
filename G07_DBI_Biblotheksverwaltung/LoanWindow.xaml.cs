using System;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static G07_DBI_Biblotheksverwaltung.User_Book_BookLoan;

namespace G07_DBI_Biblotheksverwaltung
{
    public partial class LoanWindow : Window
    {
        public BookLoan NewLoan { get; set; }
        private SQLiteConnection connection;

        public LoanWindow(SQLiteConnection conn)
        {
            InitializeComponent();
            NewLoan = new BookLoan();
            connection = conn;
            LoadBooks();
            LoadUsers();
        }

        public LoanWindow(BookLoan loan, SQLiteConnection conn)
        {
            InitializeComponent();
            NewLoan = loan;
            connection = conn;
            LoadBooks();
            LoadUsers();
            SetSelectedItems();
            dpLoanDate.SelectedDate = loan.LoanDate;
            dpReturnDate.SelectedDate = loan.ReturnDate;
        }

        private void LoadBooks()
        {
            string query = "SELECT Title, Author FROM Books";
            using (var command = new SQLiteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    cbBookTitle.Items.Add(new ComboBoxItem
                    {
                        Content = reader["Title"].ToString(),
                        Tag = reader["Author"].ToString()
                    });
                }
            }
            cbBookTitle.DisplayMemberPath = "Content";
        }

        private void LoadUsers()
        {
            string query = "SELECT Name, Email FROM Users";
            using (var command = new SQLiteCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    cbUser.Items.Add(new ComboBoxItem
                    {
                        Content = reader["Name"].ToString(),
                        Tag = reader["Email"].ToString()
                    });
                }
            }
            cbUser.DisplayMemberPath = "Content";
        }

        private void SetSelectedItems()
        {
            foreach (ComboBoxItem item in cbBookTitle.Items)
            {
                if (item.Content.ToString() == NewLoan.BookTitle)
                {
                    cbBookTitle.SelectedItem = item;
                    break;
                }
            }

            foreach (ComboBoxItem item in cbUser.Items)
            {
                if (item.Content.ToString() == NewLoan.UserName)
                {
                    cbUser.SelectedItem = item;
                    break;
                }
            }
        }

        private void SaveLoan_Click(object sender, RoutedEventArgs e)
        {
            if (cbBookTitle.SelectedItem == null || cbUser.SelectedItem == null)
            {
                MessageBox.Show("Bitte wählen Sie einen Buchtitel und einen Benutzer aus.");
                return;
            }

            ComboBoxItem selectedBook = (ComboBoxItem)cbBookTitle.SelectedItem;
            ComboBoxItem selectedUser = (ComboBoxItem)cbUser.SelectedItem;

            int bookID = GetBookID(selectedBook.Content.ToString());
            int userID = GetUserID(selectedUser.Content.ToString());

            if (bookID == 0 || userID == 0)
            {
                MessageBox.Show("Fehler beim Hinzufügen der Ausleihe.");
                return;
            }

            if (NewLoan.LoanID == 0)
            {
                InsertLoan(bookID, userID, selectedBook, selectedUser);
            }
            else
            {
                UpdateLoan(bookID, userID, selectedBook, selectedUser);
            }

            this.DialogResult = true;
            this.Close();
        }

        private void InsertLoan(int bookID, int userID, ComboBoxItem selectedBook, ComboBoxItem selectedUser)
        {
            string query = "INSERT INTO Loans (BookID, UserID, LoanDate, ReturnDate) VALUES (@BookID, @UserID, @LoanDate, @ReturnDate)";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BookID", bookID);
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@LoanDate", dpLoanDate.SelectedDate ?? DateTime.Now);
                command.Parameters.AddWithValue("@ReturnDate", dpReturnDate.SelectedDate ?? DateTime.Now);
                command.ExecuteNonQuery();
                NewLoan.LoanID = (int)connection.LastInsertRowId;
                SetNewLoanDetails(selectedBook, selectedUser);
            }
        }

        private void UpdateLoan(int bookID, int userID, ComboBoxItem selectedBook, ComboBoxItem selectedUser)
        {
            string query = "UPDATE Loans SET BookID = @BookID, UserID = @UserID, LoanDate = @LoanDate, ReturnDate = @ReturnDate WHERE LoanID = @LoanID";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BookID", bookID);
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@LoanDate", dpLoanDate.SelectedDate ?? DateTime.Now);
                command.Parameters.AddWithValue("@ReturnDate", dpReturnDate.SelectedDate ?? DateTime.Now);
                command.Parameters.AddWithValue("@LoanID", NewLoan.LoanID);
                command.ExecuteNonQuery();
                SetNewLoanDetails(selectedBook, selectedUser);
            }
        }

        private void SetNewLoanDetails(ComboBoxItem selectedBook, ComboBoxItem selectedUser)
        {
            NewLoan.BookTitle = selectedBook.Content.ToString();
            NewLoan.BookAuthor = selectedBook.Tag.ToString();
            NewLoan.UserName = selectedUser.Content.ToString();
            NewLoan.UserEmail = selectedUser.Tag.ToString();
            NewLoan.LoanDate = dpLoanDate.SelectedDate ?? DateTime.Now;
            NewLoan.ReturnDate = dpReturnDate.SelectedDate ?? DateTime.Now;
        }

        private int GetBookID(string title)
        {
            string query = "SELECT BookID FROM Books WHERE Title = @Title";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                var result = command.ExecuteScalar();
                int bookID = 0;
                if (result != null)
                {
                    bookID = Convert.ToInt32(result);
                }
                return bookID;
            }
        }

        private int GetUserID(string name)
        {
            string query = "SELECT UserID FROM Users WHERE Name = @Name";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                var result = command.ExecuteScalar();
                int userID = 0;
                if (result != null)
                {
                    userID = Convert.ToInt32(result);
                }
                return userID;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
