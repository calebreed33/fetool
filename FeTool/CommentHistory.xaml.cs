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
    /// <summary>
    /// Interaction logic for CommentHistory.xaml
    /// </summary>
    public static class globalvariables
    {
        public static List<string> DatabaseLocations = new List<string>();
        public static List<SQLiteConnection> SQLite_Connections = new List<SQLiteConnection>();

    }
    public partial class CommentHistory : Window
    {
        private ListView listview1 = new ListView();

        public CommentHistory()
        {
            InitializeComponent();
        }
        private void commentList_SelectionChanged()
        {
            // UsernameBox.Items.Clear();
            foreach (string database in globalvariables.DatabaseLocations)
            {
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {

                    sqlite_connection.Open();

                    string sql = "select commentText from Comments;";
                    using (SQLiteCommand command = new SQLiteCommand(sql, sqlite_connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                commentList.Items.Add(reader["commentText"]);
                            }
                            reader.Close();
                            sqlite_connection.Close();
                            command.Dispose();
                        }

                    }

                }

            }


        }
    }
}

