using FeTool.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace FeTool.ViewModels
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public MainWindowVM()
        {
            this.ComplianceEntries = new ObservableCollection<ComplianceEntry>();
            this.System_names = new ObservableCollection<string>();
            this.Stig_IDs = new ObservableCollection<string>();
            this.Users = new ObservableCollection<string>();
            this.Comments = new ObservableCollection<string>();
            onLoad();
            Generate_Users();
            if (System_names.Count > 0) SelectedSystem_Name = System_names.ElementAt(0);
            if (Stig_IDs.Count > 0) SelectedStig_ID = Stig_IDs.ElementAt(0);
        }

        private ObservableCollection<ComplianceEntry> complianceEntries;
        private ObservableCollection<string> system_names;
        private ObservableCollection<string> stig_ids;
        private ObservableCollection<string> users;
        private ObservableCollection<string> comments;

        private ICollectionView filteredcollectionview;

        public ICollectionView FilteredComplianceEntries
        {
            get { if (filteredcollectionview == null)
                {
                    filteredcollectionview = CollectionViewSource.GetDefaultView(ComplianceEntries) as ICollectionView;
                    filteredcollectionview.Filter = V_KeyFilter;
                }
                return filteredcollectionview;
            }
        }

        public ObservableCollection<ComplianceEntry> ComplianceEntries
        {
            get { return complianceEntries; }
            set { complianceEntries = value; }
        }

        public ObservableCollection<string> System_names
        {
            get { return system_names; }
            set { system_names = value; }
        }

        public ObservableCollection<string> Stig_IDs
        {
            get { return stig_ids; }
            set { stig_ids = value; }
        }
        public ObservableCollection<string> Users
        {
            get { return users; }
            set { users = value; }
        }
        public ObservableCollection<string> Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private ComplianceEntry selectedV_Key;
        private string selectedsystem_name;
        private string selectedstig_id;
        private string selecteduser;
        private string mostrecentcomment;

        public ComplianceEntry SelectedV_Key
        {
            get { return selectedV_Key; }
            set { selectedV_Key = value;
                NotifyPropertyChanged("SelectedV_Key");
            }
        }
        public string SelectedSystem_Name
        {
            get { return selectedsystem_name; }
            set
            {
                selectedsystem_name = value;
                NotifyPropertyChanged("SelectedSystem_Name");
            }
        }

        public string SelectedStig_ID
        {
            get { return selectedstig_id; }
            set
            {
                selectedstig_id = value;
                NotifyPropertyChanged("SelectedStig_ID");
            }
        }

        public string SelectedUser
        {
            get { return selecteduser; }
            set
            {
                selecteduser = value;
                NotifyPropertyChanged("SelectedUser");
                MostRecentComment = FindMostRecentComment();
            }
        }

        public string MostRecentComment
        {
            get { return mostrecentcomment; }
            set
            {
                mostrecentcomment = value;
                NotifyPropertyChanged("MostRecentComment");
            }
        }

        private string FindMostRecentComment()
        {
            List<CommentEntry> commentList = new List<CommentEntry>();

            foreach (string database in globalvariables.DatabaseLocations)
            {
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {
                    sqlite_connection.Open();

                    string sql = "select commentText,transactionDateTime,Comments.userID from Comments inner join Transactions on Comments.transactionID = Transactions.transactionID";
                    using (SQLiteCommand command = new SQLiteCommand(sql, sqlite_connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CommentEntry ce = new CommentEntry();
                                ce.Comment = reader["commentText"] as string ?? "";
                                ce.UserID = reader["userID"] as string ?? "";
                                ce.TransactionDateTime = (long)reader["transactionDateTime"];
                                commentList.Add(ce);
                            }
                            reader.Close();
                            sqlite_connection.Close();
                            command.Dispose();
                        }
                    }
                }
            }
            long latestDateTime = 0;
            CommentEntry latestComment = new CommentEntry();

            foreach(CommentEntry ce in commentList)
            {
                if(ce.TransactionDateTime > latestDateTime && ce.UserID == SelectedUser)
                {
                    latestComment = ce;
                    latestDateTime = ce.TransactionDateTime;
                }
            }
            return latestComment.Comment;
        }

        private bool V_KeyFilter(object item)
        {
            ComplianceEntry v_key = item as ComplianceEntry;

            return ((SelectedStig_ID == v_key.Stig_ID) && (SelectedSystem_Name == v_key.System_name)) || (SelectedStig_ID=="All Stigs" && (SelectedSystem_Name == v_key.System_name)) || (SelectedSystem_Name == "All Systems" && (SelectedStig_ID == v_key.Stig_ID));
        }

        private void onLoad()
        {
            foreach (string database in globalvariables.DatabaseLocations)
            {
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {
                    sqlite_connection.Open();

                    string sql = "select Recommendation,IA_Controls,Notes,Topic,PDI,Status,System_Name,V_Key,Cat,Discussion,Stig_ID from ComplianceEntries";
                    using (SQLiteCommand command = new SQLiteCommand(sql, sqlite_connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ComplianceEntry complianceEntry = new ComplianceEntry();
                                complianceEntry.System_name = reader["System_Name"] as string ?? "";
                                if (!System_names.Contains(reader["System_Name"] as string)) System_names.Add(reader["System_Name"] as string);
                                if (!Stig_IDs.Contains((string)reader["Stig_ID"])) Stig_IDs.Add((string)reader["Stig_ID"]);
                                complianceEntry.V_key = (string)reader["V_Key"];
                                complianceEntry.Stig_ID = (string)reader["Stig_ID"];
                                complianceEntry.Cat = (long)reader["Cat"];
                                complianceEntry.Status = (string)reader["Status"];
                                complianceEntry.Discussion = (string)reader["Discussion"];
                                complianceEntry.Pdi = (string)reader["PDI"];
                                complianceEntry.Topic = (string)reader["Topic"];
                                complianceEntry.Notes = (string)reader["Notes"];
                                complianceEntry.Ia_controls = (string)reader["IA_Controls"];
                                complianceEntry.Recommendation = (string)reader["Recommendation"];
                                this.ComplianceEntries.Add(complianceEntry);
                            }
                            Stig_IDs.Add("All Stigs");
                            System_names.Add("All Systems");

                            reader.Close();
                            sqlite_connection.Close();
                            command.Dispose();
                        }
                    }
                }
            }
        }

        private void Generate_Users()
        {
            foreach (string database in globalvariables.DatabaseLocations)
            {
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {
                    sqlite_connection.Open();

                    string sql = "select userID,commentText from Comments";
                    using (SQLiteCommand command = new SQLiteCommand(sql, sqlite_connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Users.Add(reader["userID"] as string);
                                Comments.Add(reader["commentText"] as string);
                            }
                            reader.Close();
                            sqlite_connection.Close();
                            command.Dispose();
                        }
                    }
                }
            }
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