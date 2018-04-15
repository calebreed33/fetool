using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FeTool.Models
{
    class ComplianceEntry : INotifyPropertyChanged
    {
        private string system_name, checklist, pdi, v_key, discussion, notes, recommendation, ia_controls, status, comments;
        private long topic, cat, entryid;

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

        public string Checklist
        {
            get
            {
                return checklist;
            }
            set
            {
                checklist = value;
                NotifyPropertyChanged("Checklist");
            }
        }
        public string Pdi
        {
            get
            {
                return pdi;
            }
            set
            {
                pdi = value;
                NotifyPropertyChanged("pdi");
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

        public string Discussion
        {
            get
            {
                return discussion;
            }
            set
            {
                discussion = value;
                NotifyPropertyChanged("Discussion");
            }
        }
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
                NotifyPropertyChanged("Notes");
            }
        }
        public string Recommendation
        {
            get
            {
                return recommendation;
            }
            set
            {
                recommendation = value;
                NotifyPropertyChanged("Recommendation");
            }
        }
        public string Ia_controls
        {
            get
            {
                return ia_controls;
            }
            set
            {
                ia_controls = value;
                NotifyPropertyChanged("IA_Controls");
            }
        }
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                NotifyPropertyChanged("Status");
            }
        }
        public string Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
                NotifyPropertyChanged("Comments");
            }
        }
        public long Topic
        {
            get
            {
                return topic;
            }
            set
            {
                topic = value;
                NotifyPropertyChanged("Topic");
            }
        }
        public long Cat
        {
            get
            {
                return cat;
            }
            set
            {
                cat = value;
                NotifyPropertyChanged("Cat");
            }
        }
        public long Entryid
        {
            get
            {
                return entryid;
            }
            set
            {
                entryid = value;
                NotifyPropertyChanged("Entryid");
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
