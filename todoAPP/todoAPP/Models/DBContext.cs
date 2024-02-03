using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace todoAPP.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Todo> Todo { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:Database");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .HasName("PK_TodoList")
                    .IsClustered(false);

                entity.HasIndex(e => e.Idx, "CX_Todo")
                    .IsClustered();

                entity.HasIndex(e => new { e.Uid, e.UserId }, "IX_Todo");

                entity.Property(e => e.Uid).ValueGeneratedNever();

                entity.Property(e => e.Idx).ValueGeneratedOnAdd();

                entity.Property(e => e.Text).HasMaxLength(30);

                entity.Property(e => e.Weather).HasMaxLength(6);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Todo)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Todo");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .IsClustered(false);

                entity.HasIndex(e => e.Idx, "CX_User")
                    .IsUnique()
                    .IsClustered();

                entity.HasIndex(e => new { e.Uid, e.Username }, "IX_User")
                    .IsUnique();

                entity.Property(e => e.Uid).ValueGeneratedNever();

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.Idx).ValueGeneratedOnAdd();

                entity.Property(e => e.Nickname).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Salt).HasMaxLength(30);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
