using System;
using System.Data.SQLite;
using System.Windows;
using static G07_DBI_Biblotheksverwaltung.User_Book_BookLoan;

namespace G07_DBI_Biblotheksverwaltung
{
    public partial class UserWindow : Window
    {
        public User NewUser { get; set; }
        private SQLiteConnection connection;

        public UserWindow(SQLiteConnection conn)
        {
            InitializeComponent();
            NewUser = new User();
            connection = conn;
        }

        public UserWindow(User user, SQLiteConnection conn)
        {
            InitializeComponent();
            NewUser = user;
            txtName.Text = user.Name;
            txtEmail.Text = user.Email;
            connection = conn;
        }

        private void SaveUser_Click(object sender, RoutedEventArgs e)
        {
            if (NewUser.UserID == 0)
            {
                string query = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", txtName.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                string query = "UPDATE Users SET Name = @Name, Email = @Email WHERE UserID = @UserID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", txtName.Text);
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);
                    command.Parameters.AddWithValue("@UserID", NewUser.UserID);
                    command.ExecuteNonQuery();
                }
            }
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
