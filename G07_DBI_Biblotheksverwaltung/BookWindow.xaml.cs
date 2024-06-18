using System;
using System.Data.SQLite;
using System.Windows;
using static G07_DBI_Biblotheksverwaltung.User_Book_BookLoan;

namespace G07_DBI_Biblotheksverwaltung
{
    public partial class BookWindow : Window
    {
        public Book NewBook { get; set; }
        private SQLiteConnection connection;

        public BookWindow()
        {
            InitializeComponent();
            NewBook = new Book();
            connection = App.Connection;
        }

        public BookWindow(Book book)
        {
            InitializeComponent();
            NewBook = book;
            txtTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            txtGenre.Text = book.Genre;
            txtYear.Text = book.Year.ToString();
            connection = App.Connection;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int year;
                if (!int.TryParse(txtYear.Text, out year))
                {
                    MessageBox.Show("Bitte geben Sie ein gültiges Jahr ein.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtAuthor.Text) || string.IsNullOrWhiteSpace(txtGenre.Text))
                {
                    MessageBox.Show("Bitte füllen Sie alle Felder aus.");
                    return;
                }

                if (NewBook.BookID == 0)
                {
                    InsertBook(txtTitle.Text, txtAuthor.Text, txtGenre.Text, year);
                }
                else
                {
                    UpdateBook(NewBook.BookID, txtTitle.Text, txtAuthor.Text, txtGenre.Text, year);
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ein Fehler ist aufgetreten: " + ex.Message);
            }
        }

        private void InsertBook(string title, string author, string genre, int year)
        {
            string query = "INSERT INTO Books (Title, Author, Genre, Year) VALUES (@Title, @Author, @Genre, @Year)";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Author", author);
                command.Parameters.AddWithValue("@Genre", genre);
                command.Parameters.AddWithValue("@Year", year);
                command.ExecuteNonQuery();
                NewBook = new Book { Title = title, Author = author, Genre = genre, Year = year };
            }
        }

        private void UpdateBook(int bookID, string title, string author, string genre, int year)
        {
            string query = "UPDATE Books SET Title = @Title, Author = @Author, Genre = @Genre, Year = @Year WHERE BookID = @BookID";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Author", author);
                command.Parameters.AddWithValue("@Genre", genre);
                command.Parameters.AddWithValue("@Year", year);
                command.Parameters.AddWithValue("@BookID", bookID);
                command.ExecuteNonQuery();
                NewBook = new Book { BookID = bookID, Title = title, Author = author, Genre = genre, Year = year };
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
