using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FeTool.ViewModels
{
    public class MasterCollection : INotifyPropertyChanged
    {
        private string system_name, stig_id, v_key;

        public string System_name
        {
            get
            {
                return system_name;
            }
            set
            {
                system_name = value;
                NotifyPropertyChanged("System_name");
            }
        }
        public string Stig_id
        {
            get
            {
                return stig_id;
            }
            set
            {
                stig_id = value;
                NotifyPropertyChanged("Stig_id");
            }
        }
        public string V_key
        {
            get
            {
                return v_key;
            }
            set
            {
                v_key = value;
                NotifyPropertyChanged("V_key");
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