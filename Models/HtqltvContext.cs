using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HTQLTV.Models;

public partial class HtqltvContext : DbContext
{
    public HtqltvContext()
    {
    }

    public HtqltvContext(DbContextOptions<HtqltvContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BorrowReturn> BorrowReturns { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Statistic> Statistics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-CHPMAL9;Initial Catalog=HTQLTV;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C2275B3B6BE0");

            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.BookImage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Publisher).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Books__CategoryI__15A53433");
        });

        modelBuilder.Entity<BorrowReturn>(entity =>
        {
            entity.HasKey(e => e.BorrowReturnId).HasName("PK__Borrow_R__732345119A93A8AF");

            entity.ToTable("Borrow_Return");

            entity.Property(e => e.BorrowReturnId).HasColumnName("BorrowReturnID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.ReaderId).HasColumnName("ReaderID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.StatId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StatID");

            entity.HasOne(d => d.Book).WithMany(p => p.BorrowReturns)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Borrow_Re__BookI__2022C2A6");

            entity.HasOne(d => d.Reader).WithMany(p => p.BorrowReturns)
                .HasForeignKey(d => d.ReaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Borrow_Re__Reade__1F2E9E6D");

            entity.HasOne(d => d.Staff).WithMany(p => p.BorrowReturns)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Borrow_Re__Staff__2116E6DF");

            entity.HasOne(d => d.Stat).WithMany(p => p.BorrowReturns)
                .HasForeignKey(d => d.StatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Borrow_Re__StatI__220B0B18");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B347D47D5");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.ReaderId).HasName("PK__Readers__8E67A581C174B1D0");

            entity.Property(e => e.ReaderId).HasColumnName("ReaderID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ReaderAddress).HasMaxLength(255);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AAF7AE57B5F7");

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Position).HasMaxLength(100);
        });

        modelBuilder.Entity<Statistic>(entity =>
        {
            entity.HasKey(e => e.StatId).HasName("PK__Statisti__3A162D1E935DF1C6");

            entity.ToTable("Statistic");

            entity.Property(e => e.StatId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StatID");
            entity.Property(e => e.BookId).HasColumnName("BookID");

            entity.HasOne(d => d.Book).WithMany(p => p.Statistics)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Statistic__BookI__1C5231C2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC34409C09");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4B0840767").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.AssociatedId).HasColumnName("AssociatedID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
