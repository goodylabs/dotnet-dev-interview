using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDo.Models;
using ToDo.ViewModel;

namespace ToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {     
        public MainWindow()
        {
            InitializeComponent();
            //CreateUserTask()
            ReloadUsers();             
            UserList.ItemsSource = Users;
            TaskGrid.ItemsSource = Tasks;
        }

       
        public ObservableCollection<User> Users = new ObservableCollection<User>();
        public ObservableCollection<Task> Tasks = new ObservableCollection<Task>();

        public void CreateUserTask() 
        {
            using (UserContext db = new UserContext()) 
            {
                User us1 = new User { Name = "Joe" };
                db.Users.Add(us1);
                db.SaveChanges();

                Task t1 = new Task { Content = "Add",IsCompleted = false ,User = us1 };
                Task t2 = new Task { Content = "Commit", IsCompleted = false, User = us1 };
                Task t3 = new Task { Content = "Push", IsCompleted = true, User = us1 };
                db.Tasks.AddRange(new List<Task> { t1, t2, t3 });
                db.SaveChanges();            
            }            
        }

        public void ReloadUsers() 
        {
            Users.Clear();
            using (UserContext db = new UserContext())
            {
                foreach (User user in db.Users)
                {
                    Users.Add(user);
                }
            }
        }

      
        private void Read_btn_Click(object sender, RoutedEventArgs e)
        {
            Tasks.Clear();
            if (UserList.SelectedItem != null)
            {
                var user = UserList.SelectedItem as User;
                using (UserContext db = new UserContext())
                {
                    foreach (Task task in db.Tasks.Where(x => x.UserId == user.UserId))
                    {
                        Tasks.Add(task);
                    }
                }
            }
            else { MessageBox.Show("Choose some user."); }
        }


        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if (UserList.SelectedItem != null)
            {
                using (UserContext db = new UserContext())
                {
                    User user = UserList.SelectedValue as User;                   
                    var users = db.Users.Include(u => u.Tasks).
                        SingleOrDefault(x => x.UserId == user.UserId);

                    foreach (var task in users.Tasks.ToList())
                        db.Tasks.Remove(task);

                    db.Users.Remove(users);
                    db.SaveChanges();
                    Tasks.Clear();
                    Users.Remove(user);
                }
            }
            else { MessageBox.Show("Choose a user."); }
        }


        private void Create_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(AddUser_field.Text))
            {
                using (UserContext db = new UserContext())
                {
                    var user_name = AddUser_field.Text;
                    User user = new User { Name = user_name };
                    db.Users.Add(user);
                    db.SaveChanges();
                    Users.Add(user);
                }
            }
            else{ MessageBox.Show("User's name field empty.");}
        }

        private void Update_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(AddUser_field.Text) && UserList.SelectedItem != null)
            {
                using (UserContext db = new UserContext())
                {
                    var newName = AddUser_field.Text;
                    User name = UserList.SelectedValue as User;
                    User user = db.Users.First(x => x.UserId == name.UserId);
                    user.Name = newName;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }

                ReloadUsers();
            }
            else { MessageBox.Show("Select a user and write new name."); }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var new_task = Task_field.Text;
            User user_name = (User)UserList.SelectedValue;
            if (!String.IsNullOrEmpty(new_task) && UserList.SelectedItem!=null)
            {
                using (UserContext db = new UserContext())
                {
                    var user = db.Users.Where(x => x.UserId == user_name.UserId).FirstOrDefault<User>();
                    Task task = new Task { Content = new_task, User = user };
                    db.Tasks.Add(task);
                    db.SaveChanges();
                    Tasks.Add(task);
                }
                
            }
            else { MessageBox.Show("Select user and write new task."); }
        }

        private void Delete_task_btn_Click(object sender, RoutedEventArgs e)
        {
            if(TaskGrid.SelectedItem != null) 
            {
                using(UserContext db = new UserContext()) 
                {
                    Task choosen_task = TaskGrid.SelectedItem as Task;
                    Task del_task= db.Tasks.First(x => x.TaskId == choosen_task.TaskId);
                    db.Tasks.Remove(del_task);
                    db.SaveChanges();
                    Tasks.Remove(choosen_task);
                }
            }
            else { MessageBox.Show("Select task for delete."); }
        }

        private void Update_task_btn_Click(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrEmpty(Task_field.Text) && TaskGrid.SelectedValue != null) 
            {
                using(UserContext db = new UserContext()) 
                {
                    var new_task = Task_field.Text;
                    Task choosen_task = TaskGrid.SelectedValue as Task;
                    Task task = db.Tasks.First(x => x.TaskId == choosen_task.TaskId);
                    task.Content = new_task;
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();

                    choosen_task.Content = new_task;
                }                        
            }
            else { MessageBox.Show("Select task and type new name."); }

        }
     
        private void ReadAll_btn_Click(object sender, RoutedEventArgs e)
        {
            Tasks.Clear();
            using (UserContext db = new UserContext())
            {
                foreach (Task task in db.Tasks)
                {
                    Tasks.Add(task);
                }
            }
        }

        private void Complete_chb_Checked(object sender, RoutedEventArgs e)
        {           
            using(UserContext db = new UserContext()) 
            {
                foreach (var item in Tasks)
                {
                    if (item.IsCompleted)
                    {                        
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }

    }
}
