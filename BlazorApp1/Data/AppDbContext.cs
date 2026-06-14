using System;
using System.Collections.Generic;
using BlazorApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<TaskComment> TaskComments { get; set; }

    public virtual DbSet<TaskFile> TaskFiles { get; set; }

    public virtual DbSet<TaskHistory> TaskHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkTask> WorkTasks { get; set; }

    public virtual DbSet<WorkTaskPriority> WorkTaskPriorities { get; set; }

    public virtual DbSet<WorkTaskStatus> WorkTaskStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.idD).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("Отдел"));

            entity.Property(e => e.Name)
                .HasMaxLength(225)
                .HasComment("Название отдела");
        });

        modelBuilder.Entity<TaskComment>(entity =>
        {
            entity.HasKey(e => e.idTC).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("Комментарии"));

            entity.HasIndex(e => e.Tasks_idT, "fk_TaskComments_Tasks1_idx");

            entity.HasIndex(e => e.Users_idU, "fk_TaskComments_Users1_idx");

            entity.Property(e => e.Comment).HasColumnType("text");
            entity.Property(e => e.CommentTC).HasColumnType("datetime");
        });

        modelBuilder.Entity<TaskFile>(entity =>
        {
            entity.HasKey(e => e.idTF).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("Файлы"));

            entity.HasIndex(e => e.Tasks_idT, "fk_TaskFiles_Tasks1_idx");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.UploadedAtTF).HasColumnType("datetime");
            entity.Property(e => e._FilePath)
                .HasMaxLength(500)
                .HasColumnName(" FilePath");
        });

        modelBuilder.Entity<TaskHistory>(entity =>
        {
            entity.HasKey(e => e.idTH).HasName("PRIMARY");

            entity.ToTable("TaskHistory", tb => tb.HasComment("История изменений"));

            entity.HasIndex(e => e.Tasks_idT, "fk_TaskHistory_Tasks1_idx");

            entity.HasIndex(e => e.Users_idU, "fk_TaskHistory_Users1_idx");

            entity.Property(e => e.ActionType).HasMaxLength(45);
            entity.Property(e => e.ChangedAtTH).HasColumnType("datetime");
            entity.Property(e => e.FieldName).HasMaxLength(100);
            entity.Property(e => e.NewValue).HasColumnType("text");
            entity.Property(e => e.OldValue).HasColumnType("text");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.idU).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("Пользователи"));

            entity.HasIndex(e => e.Email, "Email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Departments_idD, "fk_Users_Departments_idx");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(225);
            entity.Property(e => e.IsActive).HasDefaultValueSql("'1'");
            entity.Property(e => e.NameU).HasMaxLength(225);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        modelBuilder.Entity<WorkTask>(entity =>
        {
            entity.HasKey(e => e.idT).HasName("PRIMARY");

            entity.ToTable("WorkTask");

            entity.HasIndex(e => e.TaskStatus_idTS);
            entity.HasIndex(e => e.TaskPriority_idTP);

            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.DueDate).HasColumnType("datetime");

            // 🔗 СТАТУС
            entity.HasOne(d => d.Status)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TaskStatus_idTS)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // 🔗 ПРИОРИТЕТ
            entity.HasOne(d => d.Priority)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TaskPriority_idTP)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<WorkTaskPriority>(entity =>
        {
            entity.HasKey(e => e.idTP).HasName("PRIMARY");

            entity.ToTable("WorkTaskPriority", tb => tb.HasComment("Приоритеты"));

            entity.Property(e => e.Level).HasMaxLength(45);
            entity.Property(e => e.NameTP).HasMaxLength(45);
        });

        modelBuilder.Entity<WorkTaskStatus>(entity =>
        {
            entity.HasKey(e => e.idTS).HasName("PRIMARY");

            entity.ToTable("WorkTaskStatus", tb => tb.HasComment("Статус"));

            entity.Property(e => e.NameTS)
                .HasMaxLength(45)
                .HasComment("Название статуса");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
