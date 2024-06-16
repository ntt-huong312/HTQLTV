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

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-TG70GLL\\MSSQLSERVER01;Initial Catalog=HTQLTV;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C22774176491");

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
                .HasConstraintName("FK__Books__CategoryI__4F9CCB9E");
        });

        modelBuilder.Entity<BorrowReturn>(entity =>
        {
            entity.HasKey(e => e.BorrowReturnId).HasName("PK__Borrow_R__73234511DB8932CF");

            entity.ToTable("Borrow_Return");

            entity.Property(e => e.BorrowReturnId).HasColumnName("BorrowReturnID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.ReaderId).HasColumnName("ReaderID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.TotalBorrowed).HasDefaultValue(0);
            entity.Property(e => e.TotalReturned).HasDefaultValue(0);

            entity.HasOne(d => d.Book).WithMany(p => p.BorrowReturns)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Borrow_Re__BookI__592635D8");

            entity.HasOne(d => d.Reader).WithMany(p => p.BorrowReturns)
                .HasForeignKey(d => d.ReaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Borrow_Re__Reade__5832119F");

            entity.HasOne(d => d.Staff).WithMany(p => p.BorrowReturns)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Borrow_Re__Staff__5A1A5A11");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B6E999F37");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.ReaderId).HasName("PK__Readers__8E67A5817DFE3FAD");

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
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AAF7E72D57FB");

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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC10048F47");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E435187120").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            //entity.Property(e => e.AssociatedId).HasColumnName("AssociatedID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RoleID).HasColumnName("RoleID");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
