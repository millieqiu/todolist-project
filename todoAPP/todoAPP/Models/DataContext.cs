using System;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
namespace todoAPP.Models
{
	public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Todo>()
                .Property(t => t.UpdatedAt)
                .HasDefaultValueSql("getdate()");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Todo> TodoList { get; set; }
    }
}

