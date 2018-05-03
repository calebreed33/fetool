using FeTool.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.IO;
using System.Collections;

namespace FeTool
{
    //TODO: These variables should later operate in the global:: namespace
    public static class globalvariables{
            public static List<string> DatabaseLocations = new List<string>();
            public static List<SQLiteConnection> SQLite_Connections = new List<SQLiteConnection>();
    }

    public partial class LoginScreen : Window
    {
        private ComboBox comboBox1 = new ComboBox();
        //private TextBox PasswordBox = new TextBox();
        private PasswordBox LoginPasswordBox = new PasswordBox();
        public char PasswordChar { get; set; }

        public LoginScreen()
        {
            InitializeComponent();
        }

        private void ComboBox_DropDownOpened(object sender, EventArgs e)
        {
            UsernameBox.Items.Clear();
            foreach (string database in globalvariables.DatabaseLocations){
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {
                    globalvariables.SQLite_Connections.Add(sqlite_connection);
                    sqlite_connection.Open();

                    string sql = "select userID from Users;";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite_connection);

                    SQLiteDataReader reader = command.ExecuteReader();

                    while (reader.Read()){
                        UsernameBox.Items.Add(reader["userID"]);
                    }
                    sqlite_connection.Close();
                }
            }
        }

        private void ComboBox_SelectionChanged (object sender, RoutedEventArgs e){
        }

        private void ImportData(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".db";
            dlg.Filter = "SQLite DB (*.db)|*.db";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Save to global variable
                globalvariables.DatabaseLocations.Add(dlg.FileName);

                // Verify the database connection/location
                string messageBoxText = "Connected to:";
                foreach (string databaseitem in globalvariables.DatabaseLocations) {
                    messageBoxText = messageBoxText + Environment.NewLine + databaseitem.Split('\\').Last();
                }

                string caption = "New DB Connection";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            foreach (string database in globalvariables.DatabaseLocations){
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {
                    globalvariables.SQLite_Connections.Add(sqlite_connection);
                    sqlite_connection.Open();

                    string sql = "SELECT userPassword FROM Users WHERE userID=" + UsernameBox.SelectedItem + ";";

                    SQLiteCommand command = new SQLiteCommand(sql, sqlite_connection);

                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["userPassword"] != null)
                        { //This line may not even be necessary
                            if (reader["userPassword"].ToString() == PasswordBox.Password.ToString()){
                                MainWindow window = new MainWindow();
                                this.Close();
                                window.ShowDialog();
                            }
                            else
                            {
                                string messageBoxText = "The password is incorrect.";
                                string caption = "Try Again";
                                MessageBoxButton button = MessageBoxButton.OK;
                                MessageBoxImage icon = MessageBoxImage.Error;
                                MessageBox.Show(messageBoxText, caption, button, icon);
                            }
                        }
                    }
                    sqlite_connection.Close();
                }
            }
        }
    }
}