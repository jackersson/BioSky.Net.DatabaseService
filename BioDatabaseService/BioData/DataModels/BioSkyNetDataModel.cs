namespace BioData.DataModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using BioContracts;

    public partial class BioSkyNetDataModel : DbContext
    {
        public BioSkyNetDataModel( IConnectionBuilder connectionBuilder) 
                                 : base(connectionBuilder.Create()) 

        {
        }

        public virtual DbSet<AccessDevice> AccessDevice { get; set; }
        public virtual DbSet<BiometricLocation> BiometricLocation { get; set; }
        public virtual DbSet<CaptureDevice> CaptureDevice { get; set; }
        public virtual DbSet<Card> Card { get; set; }
        public virtual DbSet<Criminal> Criminal { get; set; }
        public virtual DbSet<EyesCharacteristic> EyesCharacteristic { get; set; }
        public virtual DbSet<FaceCharacteristic> FaceCharacteristic { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonAccess> PersonAccess { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<PortraitCharacteristic> PortraitCharacteristic { get; set; }
        public virtual DbSet<Visitor> Visitor { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BiometricLocation>()
                .HasMany(e => e.LeftEyeCharacteristic)
                .WithOptional(e => e.LeftEyeBiometricLocation)
                .HasForeignKey(e => e.Left_Eye_Location_Id);

            modelBuilder.Entity<BiometricLocation>()
                .HasMany(e => e.RightEyeCharacteristic)
                .WithOptional(e => e.RightEyeBiometricLocation)
                .HasForeignKey(e => e.Right_Eye_Location_Id);

            modelBuilder.Entity<BiometricLocation>()
                .HasMany(e => e.FaceCharacteristic)
                .WithOptional(e => e.FaceBiometricLocation)
                .HasForeignKey(e => e.Biometric_Location_Id);

            modelBuilder.Entity<Criminal>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Criminal>()
                .HasMany(e => e.Person)
                .WithOptional(e => e.Criminal)
                .HasForeignKey(e => e.Criminal_Id);

            modelBuilder.Entity<EyesCharacteristic>()
                .HasMany(e => e.FaceCharacteristic)
                .WithOptional(e => e.EyesCharacteristic)
                .HasForeignKey(e => e.Eyes_Charachteristic_Id);

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
                .HasMany(e => e.PersonAccessCollection)
                .WithRequired(e => e.Location)
                .HasForeignKey(e => e.Location_Id)
                .WillCascadeOnDelete(false);

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
                .HasMany(e => e.PersonAccess)
                .WithRequired(e => e.Person)
                .HasForeignKey(e => e.Person_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.PhotoCollection)
                .WithOptional(e => e.Person)
                .HasForeignKey(e => e.Person_Id);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.Visitor)
                .WithOptional(e => e.Person)
                .HasForeignKey(e => e.Person_ID);

            modelBuilder.Entity<PersonAccess>()
                .HasMany(e => e.LocationCollection)
                .WithOptional(e => e.PersonAccess)
                .HasForeignKey(e => e.Access_Map_Id);

            modelBuilder.Entity<Photo>()
                .HasMany(e => e.PersonCollection)
                .WithOptional(e => e.Photo)
                .HasForeignKey(e => e.Photo_Id);

            modelBuilder.Entity<Photo>()
                .HasMany(e => e.Visitor)
                .WithOptional(e => e.CropedPhoto)
                .HasForeignKey(e => e.Croped_Photo_Id);

            modelBuilder.Entity<Photo>()
                .HasMany(e => e.VisitorCollection)
                .WithRequired(e => e.FullPhoto)
                .HasForeignKey(e => e.Full_Photo_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PortraitCharacteristic>()
                .HasMany(e => e.FaceCharacteristic)
                .WithOptional(e => e.PortraitCharacteristic)
                .HasForeignKey(e => e.Portrait_Charachteristic_Id);

            modelBuilder.Entity<PortraitCharacteristic>()
                .HasMany(e => e.Photo)
                .WithOptional(e => e.PortraitCharacteristic)
                .HasForeignKey(e => e.Portrait_Characteristics_Id);     


    }
  }
}
