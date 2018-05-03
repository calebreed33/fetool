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
using ExcelDataReader;

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
                // Save to temporary variable. May need to readdress due to not being global variable. Unlikely.
                string baseline = dlg.FileName;

                FileStream stream = File.Open(baseline, FileMode.Open, FileAccess.Read);
                //Check filetype
                if (System.IO.Path.GetExtension(baseline).ToUpper() == ".XLS")
                {
                    //Reading from a binary Excel file ('97-2003 format; *.xls)
                    IExcelDataReader reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else
                {
                    //Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    //reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
               // excelReader.IsFirstRowAsColumnNames = true;

                //Data set configuration
                //var conf = new ExcelDataSetConfiguration
                //{
                //    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                //    {
                //        UseHeaderRow = true
                //    }
                //};
                //DataSet dataSet = excelReader.AsDataSet(conf);
                //DataTable dataTable = dataSet.tables[0];

                //int i = 0;
                //while (dataTable.Rows[0][0] <= dataTable.GetLength(1))
                //{
                //    string systemName = dataTable.Rows[i][0];
                //    string checklist = dataTable.Rows[i][1];
                //    string topic = dataTable.Rows[i][2];
                //    string pdi = dataTable.Rows[i][3];
                //    string vKey = dataTable.Rows[i][4];
                //    string cat = dataTable.Rows[i][5];
                //    string discussion = dataTable.Rows[i][6];
                //    string notes = dataTable.Rows[i][0];
                //    string recommendation = dataTable.Rows[i][8];
                //    string iaControl = dataTable.Rows[i][9];
                //    string status = dataTable.Rows[i][10];
                //    i++;
                //    foreach (SQLiteConnection connection in globalvariables.SQLite_Connections)
                //    {
                //        SQLiteCommand command = new SQLiteCommand("INSERT INTO ComplianceEntries VALUES ()", connection);
                //        command.ExecuteNonQuery();

                //        command = SQLiteCommand("INSERT INTO ComplianceEntries VALUES ()", connection);
                //        command.ExecuteNonQuery();
                //    }
                //}
               // excelReader.Close();

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
            dlg.Filter = "Test Spreadsheet (*.xls)|*.xls;*xlsx";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Save to temporary variable
                string test = dlg.FileName;

                //TODO: Import test at test variable to database
                //ExcelDataReader.

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
            foreach (string database in globalvariables.DatabaseLocations)
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
            }
        }

        private void v_keybox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (this.DataContext as MainWindowVM).FilteredComplianceEntries.Refresh();
        }
    }
}