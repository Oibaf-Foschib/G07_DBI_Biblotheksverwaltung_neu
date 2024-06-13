using System;
using System.Data.SQLite;
using System.Windows;
using static G07_DBI_Biblotheksverwaltung.User_Book_BookLoan;

namespace G07_DBI_Biblotheksverwaltung
{
    public partial class UserWindow : Window
    {

        public UserWindow(SQLiteConnection conn)
        {
            InitializeComponent();
        }

        private void SaveUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
