using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace XunitAssessment.Models;

public partial class DemoContext : DbContext
{
    public DemoContext()
    {
    }

    public DemoContext(DbContextOptions<DemoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aocolumn> Aocolumns { get; set; }

    public virtual DbSet<Aotable> Aotables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=VM-104; database=PAS;User Id=Training;Password=May2022#;Trusted_Connection=False;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aocolumn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_AOColumn_Id");

            entity.ToTable("AOColumn", tb => tb.HasTrigger("tr_AOColumn_Delete"));

            entity.HasIndex(e => e.Name, "ix_AOColumn_Name");

            entity.HasIndex(e => e.TableId, "ix_AOColumn_TableId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Comment).HasMaxLength(2048);
            entity.Property(e => e.DataType).HasMaxLength(128);
            entity.Property(e => e.Description).HasMaxLength(128);
            entity.Property(e => e.Distortion).HasMaxLength(64);
            entity.Property(e => e.Name).HasMaxLength(128);
            entity.Property(e => e.Type).HasMaxLength(128);

            entity.HasOne(d => d.Table).WithMany(p => p.Aocolumns)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_AOColumn_AOTable");
        });

        modelBuilder.Entity<Aotable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_AOTable_Id");

            entity.ToTable("AOTable");

            entity.HasIndex(e => e.Name, "ix_AOTable_Name");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Boundary).HasDefaultValueSql("((0))");
            entity.Property(e => e.Cache).HasDefaultValueSql("((0))");
            entity.Property(e => e.Comment).HasMaxLength(2048);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.History).HasDefaultValueSql("((0))");
            entity.Property(e => e.Identifier).HasDefaultValueSql("((0))");
            entity.Property(e => e.Log).HasDefaultValueSql("((0))");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Notify).HasDefaultValueSql("((0))");
            entity.Property(e => e.Type).HasMaxLength(128);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
