using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FeTool.Models
{
    class CommentEntry : INotifyPropertyChanged
    {
        private string userID, comment = "";
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
