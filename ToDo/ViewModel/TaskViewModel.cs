using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo.Models;

namespace ToDo.ViewModel
{
    class TaskViewModel
    {
        private IList<Task> _TaskList;


        public TaskViewModel() 
        {
            using (UserContext db = new UserContext())
            {
                _TaskList = new List<Task>();
            
                foreach (Task task in db.Tasks)
                {
                    _TaskList.Add(task);
                }
            }
        }

        public void ReadTasksForUser() 
        {
            Tasks = null;

            Tasks = new List<Task>();
                                     
        }

        public IList<Task> Tasks
        {

            get { return _TaskList; }
            set { _TaskList = value; }
        }


        private ICommand mUpdater;

        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }


        private class Updater : ICommand
        {
            ICommand Members;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {

            }


        }

       








    }
}
