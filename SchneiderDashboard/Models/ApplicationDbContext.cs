using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SchneiderDashboard.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeDept> EmployeeDepts { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;database=employee_db;user=root;password=1234;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentCode).HasName("PRIMARY");

            entity.ToTable("department");

            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(10)
                .HasColumnName("Department_Code");
            entity.Property(e => e.DepartmentDesc)
                .HasMaxLength(50)
                .HasColumnName("Department_Desc");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.SesaId).HasName("PRIMARY");

            entity.ToTable("employee");

            entity.HasIndex(e => e.DepartmentCode, "Department_Code");

            entity.HasIndex(e => e.GenderCode, "Gender_Code");

            entity.Property(e => e.SesaId)
                .HasMaxLength(20)
                .HasColumnName("SESA_ID");
            entity.Property(e => e.Birthdate).HasColumnType("date");
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(10)
                .HasColumnName("Department_Code");
            entity.Property(e => e.GenderCode)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("Gender_Code");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.DepartmentCodeNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentCode)
                .HasConstraintName("employee_ibfk_2");

            entity.HasOne(d => d.GenderCodeNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.GenderCode)
                .HasConstraintName("employee_ibfk_1");
        });

        modelBuilder.Entity<EmployeeDept>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("employee_dept");

            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(10)
                .HasColumnName("Department_Code");
            entity.Property(e => e.SesaId)
                .HasMaxLength(20)
                .HasColumnName("SESA_ID");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderCode).HasName("PRIMARY");

            entity.ToTable("gender");

            entity.Property(e => e.GenderCode)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("Gender_Code");
            entity.Property(e => e.GenderDesc)
                .HasMaxLength(10)
                .HasColumnName("Gender_Desc");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
