using System;
using Microsoft.EntityFrameworkCore;
namespace todoAPP.Models
{
	public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Todo> TodoList { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}

