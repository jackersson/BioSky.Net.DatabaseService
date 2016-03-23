namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Location")]
    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            AccessDevice = new HashSet<AccessDevice>();
            CaptureDevice = new HashSet<CaptureDevice>();
            PersonAccessCollection = new HashSet<PersonAccess>();
            Visitor = new HashSet<Visitor>();
        }

        public long Id { get; set; }

        [Column("Location Name")]
        [Required]
        [StringLength(50)]
        public string Location_Name { get; set; }

        public string Description { get; set; }

        [Column("Access Type")]
        public byte? Access_Type { get; set; }

        [Column("Access Map Id")]
        public long? Access_Map_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessDevice> AccessDevice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CaptureDevice> CaptureDevice { get; set; }

        public virtual PersonAccess PersonAccess { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonAccess> PersonAccessCollection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visitor> Visitor { get; set; }
    }
}
