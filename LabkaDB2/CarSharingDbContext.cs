using System;
using System.Collections.Generic;
using LabkaDB2.Models;
using Microsoft.EntityFrameworkCore;

namespace LabkaDB2;

public partial class CarSharingDbContext : DbContext
{
    public CarSharingDbContext()
    {
    }

    public CarSharingDbContext(DbContextOptions<CarSharingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarSharingZone> CarSharingZones { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Rent> Rents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= DESKTOP-88FDH6D\\SQLEXPRESS; Database=CarSharingDB; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Color)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("color");
            entity.Property(e => e.Cszid).HasColumnName("CSZID");
            entity.Property(e => e.IsRented).HasColumnName("isRented");
            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.ProduceYear).HasColumnName("produceYear");
            entity.Property(e => e.TechInspirationDate).HasColumnName("techInspirationDate");

            entity.HasOne(d => d.Csz).WithMany(p => p.Cars)
                .HasForeignKey(d => d.Cszid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cars_CSZ");

            entity.HasOne(d => d.Model).WithMany(p => p.Cars)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cars_Models");
        });

        modelBuilder.Entity<CarSharingZone>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarCapacity).HasColumnName("carCapacity");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(11, 8)")
                .HasColumnName("latitude");
            entity.Property(e => e.Longtitude)
                .HasColumnType("decimal(11, 8)")
                .HasColumnName("longtitude");
            entity.Property(e => e.Town)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("town");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreationDate).HasColumnName("creationDate");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.NumOfOrders).HasColumnName("numOfOrders");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Brand)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("brand");
            entity.Property(e => e.EngineVolume).HasColumnName("engineVolume");
            entity.Property(e => e.HasCooling).HasColumnName("hasCooling");
            entity.Property(e => e.IsAutomatic).HasColumnName("isAutomatic");
            entity.Property(e => e.ModelName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("modelName");
            entity.Property(e => e.NumOfSeats).HasColumnName("numOfSeats");
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarId).HasColumnName("carID");
            entity.Property(e => e.CostPerDay).HasColumnName("costPerDay");
            entity.Property(e => e.Cszid).HasColumnName("CSZID");
            entity.Property(e => e.CustId).HasColumnName("custID");
            entity.Property(e => e.FinishDate).HasColumnName("finishDate");
            entity.Property(e => e.StartDate).HasColumnName("startDate");

            entity.HasOne(d => d.Car).WithMany(p => p.Rents)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rents_Cars");

            entity.HasOne(d => d.Csz).WithMany(p => p.Rents)
                .HasForeignKey(d => d.Cszid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rents_CSZ");

            entity.HasOne(d => d.Cust).WithMany(p => p.Rents)
                .HasForeignKey(d => d.CustId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rents_Customer");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
