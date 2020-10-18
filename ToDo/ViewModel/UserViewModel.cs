using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDo.Models;

namespace ToDo.ViewModel
{
    class UserViewModel
    {

        private IList<User> _UserList;


        public UserViewModel()
        {
            using (UserContext db = new UserContext())
            {
                _UserList = new List<User>();

                foreach (User user in db.Users)
                {
                    _UserList.Add(user);
                }
            }
        }

        public IList<User> Users
        {

            get { return _UserList; }
            set { _UserList = value; }
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
