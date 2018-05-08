using FeTool.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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
            Generate_Users();
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
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlite_connection))
                    {
                        sqlite_connection.Open();
                        cmd.CommandText = "INSERT INTO Comments (commentText, commentID) VALUES (@commentText, Null)";

                        cmd.Parameters.AddWithValue("commentText", commentText.Text);
                        cmd.Parameters.AddWithValue("commentID", " ");

                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "SELECT * FROM Comments";

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string comment = reader["commentText"].ToString();
                                commentText.Text += comment + '\n';
                                Console.WriteLine(commentText);
                            }
                            reader.Close();
                        }
                        commentText.Clear();

                    }
                    sqlite_connection.Close();
                }
            }
        }
        private void ImportBaselineClick(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                // Set filter for file extension and default file extension
                DefaultExt = ".xls",
                Filter = "Baseline Spreadsheet (*.xls)|*.xls"
            };

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Save to temporary variable. May need to readdress due to not being global variable. Unlikely.
                string baseline = dlg.FileName;

                FileStream stream = File.Open(baseline, FileMode.Open, FileAccess.Read);

                //Check filetype
                IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream); //Supports all filetypes as of 3.1-ish

                //Data set (whole workbook) configuration
                DataSet dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    // Gets or sets a callback to obtain configuration options for a DataTable.
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        // Gets or sets a value indicating whether to use a row from the
                        // data as column names.
                        UseHeaderRow = false,
                    }
                });

                //First worksheet
                DataTable dataTable = dataSet.Tables[0];

                //This skips the header line
                int i = 1;

                //while (dataTable.Rows[0][0] <= dataTable.GetLength(1)) <--Remove if code works
                //While there are unread rows in dataTable, import data from each row
                while (i <= dataTable.Select().Length)
                {
                    string systemName = dataTable.Rows[i][0].ToString();
                    string checklist = dataTable.Rows[i][1].ToString();
                    string topic = dataTable.Rows[i][2].ToString();
                    string pdi = dataTable.Rows[i][3].ToString();
                    string vKey = dataTable.Rows[i][4].ToString();
                    string cat = dataTable.Rows[i][5].ToString();
                    string discussion = dataTable.Rows[i][6].ToString();
                    string notes = dataTable.Rows[i][0].ToString();
                    string recommendation = dataTable.Rows[i][8].ToString();
                    string iaControl = dataTable.Rows[i][9].ToString();
                    string status = dataTable.Rows[i][10].ToString();
                    DateTime dtNow = DateTime.Now;
                    long dateTime = dtNow.Year * 10000000000 + dtNow.Month * 100000000 + dtNow.Day * 1000000 + dtNow.Hour * 10000 + dtNow.Minute * 100 + dtNow.Second;

                    foreach (string database in globalvariables.DatabaseLocations)
                    {
                        using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                        {
//Use this code when you switch to a distributed DB, i.e. you're using separate tables for systems, STIGs, etc.
/*
                            //if SysName not in DB.Systems, add it
                            command = new SQLiteCommand("Select System_ID FROM Systems WHERE System_Name=" + systemName + ";", connection);
                            SQLiteDataReader tempreader = command.ExecuteReader();
                            if (tempreader.Read() == false)
                            {
                                command = new SQLiteCommand("INSERT INTO Systems VALUES (NULL," + systemName + ");", connection);
                                command.ExecuteNonQuery();
                                tempreader = command.ExecuteReader();
                            }
                            tempreader.Read();
                            int sysID = (int)tempreader["System_Name"];

                            //if SysName not in DB.Stigs, add it
                            command = new SQLiteCommand("Select System_ID FROM STIGs WHERE Stig_Name=" + checklist + ";", connection);
                            tempreader = command.ExecuteReader();
                            if (tempreader.Read() == false)
                            {
                                command = new SQLiteCommand("INSERT INTO STIGs VALUES (NULL," + checklist + ");", connection);
                                command.ExecuteNonQuery();
                            }
                            tempreader.Read();
                            int stigID = (int)tempreader["Stig_ID"];

                            //if V-Key not in DB.VKeys, add it
                            command = new SQLiteCommand("SELECT COUNT(VKey_ID) FROM VKeys WHERE VKey_ID=" + vKey + ";", connection);
                            tempreader = command.ExecuteReader();
                            if (tempreader.Read() == false)
                            {
                                command = new SQLiteCommand("INSERT INTO VKeys VALUES ("+ vKey +");", connection);
                                command.ExecuteNonQuery();
                            }
                            tempreader.Read();
                            int stigID = (int)tempreader["Stig_ID"];
    */
                            //Add to Transactions
                            SQLiteCommand command = new SQLiteCommand("INSERT INTO Transactions(transactionDateTime, userID)" +
                                "VALUES (" + dateTime + ", " + globalvariables.SessionUser +")", connection);
                            command.ExecuteNonQuery();
                            long transactionID = connection.LastInsertRowId;

                            //Add to DataSets
                            command = new SQLiteCommand("INSERT INTO DataSets(dataSetType,transactionID)" +
                                "VALUES (" + "'Baseline', " + transactionID + ")", connection);
                            command.ExecuteNonQuery();

                            //Add to ComplianceEntries
                            command = new SQLiteCommand("INSERT INTO ComplianceEntries(System_Name,Topic,PDI,V_Key," +
                                "Cat, Discussion, Notes, Recommendation, IA_Controls, Status, comments, Stig_ID)" +
                                "VALUES (" + systemName + ", " + topic + ", " + pdi + ", " + vKey + ", " + cat + ", " +
                                discussion + ", " + notes + ", " + recommendation + ", " + iaControl + ", " + status +
                                ")", connection);
                            command.ExecuteNonQuery();
                            command.Dispose();
                        }
                    }
                    i++;
                }
                reader.Close();

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
        private void Generate_Users()
        {
            foreach (string database in globalvariables.DatabaseLocations)
            {
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {
                    sqlite_connection.Open();

                    string sql = "select permType from Permissions";
                    using (SQLiteCommand command = new SQLiteCommand(sql, sqlite_connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usercombobox.Items.Add(reader["permType"]);
                            }
                            reader.Close();
                            sqlite_connection.Close();
                            command.Dispose();
                        }
                    }
                }
            }
        }

        private void v_keybox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (this.DataContext as MainWindowVM).FilteredComplianceEntries.Refresh();
        }

        private void user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}