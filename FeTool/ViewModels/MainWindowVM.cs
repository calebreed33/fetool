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

namespace FeTool.ViewModels
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public MainWindowVM()
        {
            this.ComplianceEntries = new ObservableCollection<ComplianceEntry>();
            onLoad();
        }

        private ObservableCollection<ComplianceEntry> complianceEntries;

        public ObservableCollection<ComplianceEntry> ComplianceEntries
        {
            get { return complianceEntries; }
            set { complianceEntries = value; }
        }

        private ComplianceEntry selectedV_Key;

        public ComplianceEntry SelectedV_Key
        {
            get { return selectedV_Key; }
            set { selectedV_Key = value;
                NotifyPropertyChanged("SelectedV_Key");
                }
            
        }

        private void onLoad()
        {
            foreach (string database in globalvariables.DatabaseLocations)
            {
                using (SQLiteConnection sqlite_connection = new SQLiteConnection("Data Source=" + database + ";Version=3;"))
                {
                    globalvariables.SQLite_Connections.Add(sqlite_connection);
                    sqlite_connection.Open();

                    string sql = "select V_Key,Cat from ComplianceEntries;";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite_connection);

                    SQLiteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ComplianceEntry complianceEntry = new ComplianceEntry();
                        complianceEntry.V_key = (string) reader["V_Key"];
                        complianceEntry.Cat = (long)reader["Cat"];
                        this.ComplianceEntries.Add(complianceEntry);
                    }
                    sqlite_connection.Close();
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

