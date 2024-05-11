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

        public virtual DbSet<Kanban> Kanban { get; set; } = null!;
        public virtual DbSet<KanbanSwimlane> KanbanSwimlane { get; set; } = null!;
        public virtual DbSet<Todo> Todo { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;
        public virtual DbSet<UserTag> UserTag { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:Database");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kanban>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .HasName("PK__Kanban__C5B69A4B0445C3DC")
                    .IsClustered(false);

                entity.HasIndex(e => e.Idx, "CX_Kanban")
                    .IsClustered();

                entity.HasIndex(e => e.Uid, "IX_Kanban");

                entity.Property(e => e.Uid).ValueGeneratedNever();

                entity.Property(e => e.Idx).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Kanban)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Kanban");
            });

            modelBuilder.Entity<KanbanSwimlane>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .HasName("PK__tmp_ms_x__C5B69A4BE47E58B1")
                    .IsClustered(false);

                entity.HasIndex(e => e.Idx, "CX_KanbanSwimlane")
                    .IsClustered();

                entity.HasIndex(e => e.Uid, "IX_KanbanSwimlane");

                entity.Property(e => e.Uid).ValueGeneratedNever();

                entity.Property(e => e.Idx).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.HasOne(d => d.Kanban)
                    .WithMany(p => p.KanbanSwimlane)
                    .HasForeignKey(d => d.KanbanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Kanban_KanbanSwimlane");
            });

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .HasName("PK_TodoList")
                    .IsClustered(false);

                entity.HasIndex(e => e.Idx, "CX_Todo")
                    .IsClustered();

                entity.HasIndex(e => new { e.Uid, e.KanbanSwimlaneId }, "IX_Todo");

                entity.Property(e => e.Uid).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.GeneralTodoPosition).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Idx).ValueGeneratedOnAdd();

                entity.Property(e => e.SwimlaneTodoPosition).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.KanbanSwimlane)
                    .WithMany(p => p.Todo)
                    .HasForeignKey(d => d.KanbanSwimlaneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KanbanSwimlane_Todo");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.Todo)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserTag_Todo");

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

                entity.HasIndex(e => e.Username, "UC_Username")
                    .IsUnique();

                entity.Property(e => e.Uid).ValueGeneratedNever();

                entity.Property(e => e.Avatar).HasMaxLength(1024);

                entity.Property(e => e.Idx).ValueGeneratedOnAdd();

                entity.Property(e => e.Nickname).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Salt).HasMaxLength(30);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<UserTag>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .IsClustered(false);

                entity.HasIndex(e => e.Idx, "CX_UserTag")
                    .IsClustered();

                entity.HasIndex(e => e.Uid, "IX_UserTag");

                entity.Property(e => e.Uid).ValueGeneratedNever();

                entity.Property(e => e.Idx).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasMaxLength(15);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTag)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserTag");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
