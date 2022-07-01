using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TestApplication.data_models;

namespace TestApplication.main
{
    public partial class TestApplicationDBContext : DbContext
    {
        public TestApplicationDBContext()
        {
        }

        public TestApplicationDBContext(DbContextOptions<TestApplicationDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<WorkingTask> WorkingTasks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TestApplicationDB;Username=testApplicationDBUser;Password=123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("status");

                entity.Property(e => e.StatusId)
                    .ValueGeneratedNever()
                    .HasColumnName("status_id");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<WorkingTask>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("task_pkey");

                entity.ToTable("working_task");

                entity.Property(e => e.TaskId)
                    .HasColumnName("task_id")
                    .HasDefaultValueSql("nextval('task_id_seq'::regclass)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.WorkingTasks)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("status_id");
            });

            modelBuilder.HasSequence("task_to_subtask_id_seq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
