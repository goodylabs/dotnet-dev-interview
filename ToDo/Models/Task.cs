using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo
{
    public class Task : INotifyPropertyChanged
    {
        private int taskId;
        private string content;
        private bool iscompleted;

        public int? UserId { get; set; }      
        public User User { get; set; }


        public int TaskId 
        {
            get
            {
                return taskId;
            }
            set 
            {
                taskId = value;
                OnPropertyChanged("TaskId");
            }
        }

        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }

        public bool IsCompleted
        {
            get
            {
                return iscompleted;
            }
            set
            {
                iscompleted = value;
                OnPropertyChanged("IsCompleted");
            }
        }





        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
