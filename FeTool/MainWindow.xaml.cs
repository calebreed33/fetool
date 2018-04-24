using FeTool.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.IO;
using System.Collections;

//using Microsoft.Office.Interop.Excel;

namespace FeTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowVM();
            StackPanel1.DataContext = new ExpanderListViewModel();
            Generate_VKeys();
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            LoginScreen window = new LoginScreen();
            this.Close();
            window.ShowDialog();
        }

        private void HistoryClick(object sender, RoutedEventArgs e)
        {
            CommentHistory window = new CommentHistory();
            window.ShowDialog();
        }
        private void SaveComment(object sender, RoutedEventArgs e)
        {

            foreach (string database in globalvariables.DatabaseLocations)
            {
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {

                    globalvariables.SQLite_Connections.Add(sqlite_connection);
                    sqlite_connection.Open();

                    SQLiteCommand InsertSQL = new SQLiteCommand("INSERT INTO Comments (commentText) VALUES ('" + this.userComment.Text + "')", sqlite_connection);

                    InsertSQL.Connection = sqlite_connection;
                    InsertSQL.Parameters.Add(new SQLiteParameter("@commentText", ""));
                    InsertSQL.ExecuteNonQuery();
                    sqlite_connection.Close();
                }

            }
        }

        private void ImportBaselineClick(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".xls";
            dlg.Filter = "Baseline Spreadsheet (*.xls)|*.xls";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                //Excel.Application xlApp = new Excel.Application();
                //Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(dlg.FileName);
                //Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                //Excel.Range xlRange = xlWorksheet.UsedRange;

                // Save to temporary variable. May need to readdress due to not being global variable. Unlikely.
                string baseline = dlg.FileName;
                //TODO: Import baseline at baseline variable to database
                string messageBoxText = "Imported " + baseline;
                string caption = "Baseline Imported";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }

        private void ImportTestClick(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".xls";
            dlg.Filter = "Test Spreadsheet (*.xls)|*.xls";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Save to temporary variable
                string test = dlg.FileName;
                //TODO: Import test at test variable to database
                string messageBoxText = "Imported " + test;
                string caption = "Test Results Imported";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }

        //private void ListBox_OnLaunch(object sender, RoutedEventArgs e)
        private void Generate_VKeys()
        {
            /*foreach (string database in globalvariables.DatabaseLocations)
            {
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {
                    globalvariables.SQLite_Connections.Add(sqlite_connection);
                    sqlite_connection.Open();

                    string sql = "select V_Key from ComplianceEntries;";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite_connection);

                    SQLiteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        v_keybox.Items.Add(reader["V_Key"]);
                    }
                    sqlite_connection.Close();
                }
            }*/
        }

        private void v_keybox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (this.DataContext as MainWindowVM).FilteredComplianceEntries.Refresh();
        }

       
    }
}