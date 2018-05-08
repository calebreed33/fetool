using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FeTool.Models
{
    class CommentEntry : INotifyPropertyChanged
    {
        private string transactionID,v_key, stig_name, system_name, entryID, userID, comment = "";
        private long transactionDateTime = 0;
        public string UserID
        {
            get
            {
                return userID;
            }
            set
            {
                userID = value;
                NotifyPropertyChanged("UserID");
            }
        }
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
                NotifyPropertyChanged("Comment");
            }
        }
        public string EntryID
        {
            get
            {
                return entryID;
            }
            set
            {
                entryID = value;
                NotifyPropertyChanged("EntryID");
            }
        }

        public string TransactionID
        {
            get
            {
                return transactionID;
            }
            set
            {
                transactionID = value;
                NotifyPropertyChanged("TransactionID");
            }
        }

        public string System_Name
        {
            get
            {
                return system_name;
            }
            set
            {
                system_name = value;
                NotifyPropertyChanged("System_Name");
            }
        }
        public string V_Key
        {
            get
            {
                return v_key;
            }
            set
            {
                v_key = value;
                NotifyPropertyChanged("V_Key");
            }
        }
        public string Stig_Name
        {
            get
            {
                return stig_name;
            }
            set
            {
                stig_name = value;
                NotifyPropertyChanged("Stig_Name");
            }
        }

        public long TransactionDateTime
        {
            get
            {
                return transactionDateTime;
            }
            set
            {
                transactionDateTime = value;
                NotifyPropertyChanged("TransactionDateTime");
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
