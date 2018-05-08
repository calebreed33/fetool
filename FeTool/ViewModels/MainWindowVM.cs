using FeTool.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
            onLoad();
            if (System_names.Count > 0) SelectedSystem_Name = System_names.ElementAt(0);
            if (Stig_IDs.Count > 0) SelectedStig_ID = Stig_IDs.ElementAt(0);
        }

        private ObservableCollection<ComplianceEntry> complianceEntries;
        private ObservableCollection<string> system_names;
        private ObservableCollection<string> stig_ids;

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

        private ComplianceEntry selectedV_Key;
        private string selectedsystem_name;
        private string selectedstig_id;
        private string selecteduser;

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
            }
        }

        private bool V_KeyFilter(object item)
        {
            ComplianceEntry v_key = item as ComplianceEntry;

            return ((SelectedStig_ID == v_key.Stig_ID) && (SelectedSystem_Name == v_key.System_name));
        }

        private void onLoad()
        {
            foreach (string database in globalvariables.DatabaseLocations)
            {
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {
                    sqlite_connection.Open();

                    string sql = "select System_Name,V_Key,Cat,Discussion,Stig_ID from ComplianceEntries";
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
                                complianceEntry.Discussion = (string)reader["Discussion"];
                                this.ComplianceEntries.Add(complianceEntry);
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