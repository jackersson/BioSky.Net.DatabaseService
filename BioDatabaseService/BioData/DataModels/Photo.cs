namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Photo")]
    public partial class Photo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Photo()
        {
            PersonCollection = new HashSet<Person>();
            Visitor = new HashSet<Visitor>();
            VisitorCollection = new HashSet<Visitor>();
        }

        public long Id { get; set; }

        [Column("Person Id")]
        public long? Person_Id { get; set; }

        [Column("Photo Url")]
        [Required]
        public string Photo_Url { get; set; }

        [Column("Size Type")]
        public byte? Size_Type { get; set; }

        [Column("Origin Type")]
        public byte? Origin_Type { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        [Column("Portrait Characteristics Id")]
        public long? Portrait_Characteristics_Id { get; set; }

        public DateTime? Datetime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person> PersonCollection { get; set; }

        public virtual Person Person { get; set; }

        public virtual PortraitCharacteristic PortraitCharacteristic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visitor> Visitor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visitor> VisitorCollection { get; set; }
    }
}
