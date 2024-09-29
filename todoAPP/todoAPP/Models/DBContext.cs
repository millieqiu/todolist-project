using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace todoAPP.Models;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kanban> Kanban { get; set; }

    public virtual DbSet<KanbanSwimlane> KanbanSwimlane { get; set; }

    public virtual DbSet<Todo> Todo { get; set; }

    public virtual DbSet<User> User { get; set; }

    public virtual DbSet<UserTag> UserTag { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Database");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kanban>(entity =>
        {
            entity.HasKey(e => e.Uid)
                .HasName("PK__Kanban__C5B69A4B0445C3DC")
                .IsClustered(false);

            entity.ToTable(tb => tb.HasComment("看板"));

            entity.HasIndex(e => e.Idx, "CX_Kanban").IsClustered();

            entity.HasIndex(e => e.Uid, "IX_Kanban");

            entity.Property(e => e.Uid).ValueGeneratedNever();
            entity.Property(e => e.Idx).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasComment("看板名稱");
            entity.Property(e => e.UserId).HasComment("使用者Id");

            entity.HasOne(d => d.User).WithMany(p => p.Kanban)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Kanban");
        });

        modelBuilder.Entity<KanbanSwimlane>(entity =>
        {
            entity.HasKey(e => e.Uid)
                .HasName("PK__tmp_ms_x__C5B69A4B5C6D9436")
                .IsClustered(false);

            entity.ToTable(tb => tb.HasComment("Swimlane"));

            entity.HasIndex(e => e.Idx, "CX_KanbanSwimlane").IsClustered();

            entity.HasIndex(e => e.Uid, "IX_KanbanSwimlane");

            entity.Property(e => e.Uid).ValueGeneratedNever();
            entity.Property(e => e.Idx).ValueGeneratedOnAdd();
            entity.Property(e => e.KanbanId).HasComment("看板Id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasComment("名稱");
            entity.Property(e => e.Position)
                .HasComment("排序順序")
                .HasColumnType("decimal(18, 10)");
            entity.Property(e => e.Type).HasComment("種類，0：預設，1：一般");

            entity.HasOne(d => d.Kanban).WithMany(p => p.KanbanSwimlane)
                .HasForeignKey(d => d.KanbanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kanban_KanbanSwimlane");
        });

        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(e => e.Uid)
                .HasName("PK_TodoList")
                .IsClustered(false);

            entity.ToTable(tb => tb.HasComment("待辦事項"));

            entity.HasIndex(e => e.Idx, "CX_Todo").IsClustered();

            entity.HasIndex(e => new { e.Uid, e.KanbanSwimlaneId }, "IX_Todo");

            entity.Property(e => e.Uid).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasComment("建立時間");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasComment("備註");
            entity.Property(e => e.ExecuteAt).HasComment("預計執行時間");
            entity.Property(e => e.GeneralTodoPosition)
                .HasComment("待辦事項列表順序")
                .HasColumnType("decimal(18, 10)");
            entity.Property(e => e.Idx).ValueGeneratedOnAdd();
            entity.Property(e => e.KanbanSwimlaneId).HasComment("SwimlaneId");
            entity.Property(e => e.Status).HasComment("狀態");
            entity.Property(e => e.SwimlaneTodoPosition)
                .HasComment("看板模式列表順序")
                .HasColumnType("decimal(18, 10)");
            entity.Property(e => e.TagId).HasComment("標籤Id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasComment("標題");
            entity.Property(e => e.UpdateAt).HasComment("最後更新時間");
            entity.Property(e => e.UserId).HasComment("使用者Id");

            entity.HasOne(d => d.KanbanSwimlane).WithMany(p => p.Todo)
                .HasForeignKey(d => d.KanbanSwimlaneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KanbanSwimlane_Todo");

            entity.HasOne(d => d.Tag).WithMany(p => p.Todo)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserTag_Todo");

            entity.HasOne(d => d.User).WithMany(p => p.Todo)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Todo");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Uid).IsClustered(false);

            entity.ToTable(tb => tb.HasComment("使用者"));

            entity.HasIndex(e => e.Idx, "CX_User")
                .IsUnique()
                .IsClustered();

            entity.HasIndex(e => new { e.Uid, e.Username }, "IX_User").IsUnique();

            entity.HasIndex(e => e.Username, "UC_Username").IsUnique();

            entity.Property(e => e.Uid).ValueGeneratedNever();
            entity.Property(e => e.Avatar)
                .HasMaxLength(1024)
                .HasComment("頭像");
            entity.Property(e => e.Idx).ValueGeneratedOnAdd();
            entity.Property(e => e.Nickname)
                .HasMaxLength(50)
                .HasComment("暱稱");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasComment("密碼");
            entity.Property(e => e.Role).HasComment("角色");
            entity.Property(e => e.Salt)
                .HasMaxLength(30)
                .HasComment("鹽");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasComment("帳號");
        });

        modelBuilder.Entity<UserTag>(entity =>
        {
            entity.HasKey(e => e.Uid).IsClustered(false);

            entity.ToTable(tb => tb.HasComment("標籤"));

            entity.HasIndex(e => e.Idx, "CX_UserTag").IsClustered();

            entity.HasIndex(e => e.Uid, "IX_UserTag");

            entity.Property(e => e.Uid).ValueGeneratedNever();
            entity.Property(e => e.Color)
                .HasMaxLength(10)
                .HasComment("顏色");
            entity.Property(e => e.Idx).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(15)
                .HasComment("名稱");
            entity.Property(e => e.Type).HasComment("標籤種類，0：預設，1：一般");
            entity.Property(e => e.UserId).HasComment("使用者Id");

            entity.HasOne(d => d.User).WithMany(p => p.UserTag)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserTag");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
