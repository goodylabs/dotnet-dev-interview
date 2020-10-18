namespace ToDo.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class UserContext : DbContext
    {
       
        public UserContext()
            : base("name=UserContext")
        {
        }

      
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }

    
}