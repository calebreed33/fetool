using FeTool.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace FeTool.ViewModels
{
    class CommentHistoryVM
    {
        public CommentHistoryVM()
        {
            this.UserAccounts = new ObservableCollection<string>();
            this.CommentHistory = new ObservableCollection<string>();
            this.Comments = new ObservableCollection<CommentEntry>();
            loadItemsFromDB();
            if (UserAccounts.Count > 0) SelectedUser = UserAccounts.ElementAt(0);
        }
        //private ObservableCollection<string> comment_history;
        private ObservableCollection<string> users;
        private ObservableCollection<string> commentHistory;
        private ObservableCollection<CommentEntry> comments;

        public ObservableCollection<string> UserAccounts
        {
            get { return users; }
            set { users = value; }
        }
        public ObservableCollection<string> CommentHistory
        {
            get { return commentHistory; }
            set { commentHistory = value; }
        }
        public ObservableCollection<CommentEntry> Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private string selecteduser;

        public string SelectedUser
        {
            get { return selecteduser; }
            set
            {
                selecteduser = value;
                NotifyPropertyChanged("SelectedUser");
            }

        }
        private void loadItemsFromDB()
        {
            // UsernameBox.Items.Clear();
            foreach (string database in globalvariables.DatabaseLocations)
            {
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {

                    sqlite_connection.Open();

                    string sql = "select V_Key,Stig_Name,System_Name,entryID,transactionID,commentText,userID from Comments;";
                    using (SQLiteCommand command = new SQLiteCommand(sql, sqlite_connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CommentEntry ce = new CommentEntry();
                                ce.Comment = (string)reader["commentText"];
                                ce.UserID = reader["userID"] as string ?? "";
                                if (!UserAccounts.Contains(reader["userID"] as string)) UserAccounts.Add(reader["userID"] as string);
                                ce.TransactionID = reader["transactionID"] as string ?? "";
                                ce.EntryID = reader["entryID"] as string ?? "";
                                ce.System_Name = reader["System_Name"] as string ?? "";
                                ce.Stig_Name = reader["Stig_Name"] as string ?? "";
                                ce.V_Key = reader["V_Key"] as string ?? "";
                                Comments.Add(ce);
                            }
                            reader.Close();
                            sqlite_connection.Close();
                            command.Dispose();
                        }

                    }

                }

            }

        }

        private ICollectionView filteredcollectionview;

        public ICollectionView FilteredComments
        {

            get
            {
                if (filteredcollectionview == null)
                {
                    filteredcollectionview = CollectionViewSource.GetDefaultView(Comments) as ICollectionView;
                    filteredcollectionview.Filter = userFilter;
                }
                return filteredcollectionview;
            }
        }

        private bool userFilter(object item)
        {
            CommentEntry ce = item as CommentEntry;

            return ce.UserID == selecteduser;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
