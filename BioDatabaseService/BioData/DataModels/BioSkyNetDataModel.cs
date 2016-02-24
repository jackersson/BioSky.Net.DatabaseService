namespace BioData.DataModels
{
  using System;
  using System.Data.Entity;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Linq;
  using BioContracts;

  public partial class BioSkyNetDataModel : DbContext
  {
    public BioSkyNetDataModel(  IConnectionBuilder connectionBuilder)
                              : base(connectionBuilder.create())
    {
    }
    public virtual DbSet<AccessDevice> AccessDevice { get; set; }
    public virtual DbSet<CaptureDevice> CaptureDevice { get; set; }
    public virtual DbSet<Card> Card { get; set; }
    public virtual DbSet<Location> Location { get; set; }
    public virtual DbSet<Person> Person { get; set; }
    public virtual DbSet<Photo> Photo { get; set; }
    public virtual DbSet<Visitor> Visitor { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Location>()
          .Property(e => e.Location_Name)
          .IsUnicode(false);

      modelBuilder.Entity<Location>()
          .Property(e => e.Description)
          .IsUnicode(false);

      modelBuilder.Entity<Location>()
          .HasMany(e => e.AccessDevice)
          .WithOptional(e => e.Location)
          .HasForeignKey(e => e.Location_Id);

      modelBuilder.Entity<Location>()
          .HasMany(e => e.CaptureDevice)
          .WithOptional(e => e.Location)
          .HasForeignKey(e => e.Location_Id);

      modelBuilder.Entity<Location>()
          .HasMany(e => e.Visitor)
          .WithRequired(e => e.Location)
          .HasForeignKey(e => e.Location_Id)
          .WillCascadeOnDelete(false);

      modelBuilder.Entity<Person>()
          .Property(e => e.First_Name_)
          .IsUnicode(false);

      modelBuilder.Entity<Person>()
          .Property(e => e.Last_Name_)
          .IsUnicode(false);

      modelBuilder.Entity<Person>()
          .Property(e => e.Country)
          .IsUnicode(false);

      modelBuilder.Entity<Person>()
          .Property(e => e.City)
          .IsUnicode(false);

      modelBuilder.Entity<Person>()
          .Property(e => e.Comments)
          .IsUnicode(false);

      modelBuilder.Entity<Person>()
          .Property(e => e.Email)
          .IsUnicode(false);

      modelBuilder.Entity<Person>()
          .HasMany(e => e.Card)
          .WithOptional(e => e.Person)
          .HasForeignKey(e => e.Person_Id);

      modelBuilder.Entity<Person>()
          .HasMany(e => e.Photo)
          .WithOptional(e => e.Person)
          .HasForeignKey(e => e.Person_Id);

      modelBuilder.Entity<Photo>()
          .HasMany(e => e.Visitor)
          .WithOptional(e => e.Photo)
          .HasForeignKey(e => e.Photo_ID);
    }
  }
}
