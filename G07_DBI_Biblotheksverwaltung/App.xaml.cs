using System;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace G07_DBI_Biblotheksverwaltung
{
    public partial class App : Application
    {
        public static SQLiteConnection Connection;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string dbPath = Path.Combine(projectDirectory, "Datenbank.db");

            Connection = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            Connection.Open();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Connection.Close();
            base.OnExit(e);
        }
    }
}
