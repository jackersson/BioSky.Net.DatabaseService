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
            PersonAccess = new HashSet<PersonAccess>();
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

        [Required]
        public string MacAddress { get; set; }

        [Column("Access Device ID")]
        public long? Access_Device_ID { get; set; }

        [Column("Capture Device ID")]
        public long? Capture_Device_ID { get; set; }

        [Column("Fingerprint Device ID")]
        public long? Fingerprint_Device_ID { get; set; }

        public virtual AccessDevice AccessDevice { get; set; }

        public virtual CaptureDevice CaptureDevice { get; set; }

        public virtual FingerprintDevice FingerprintDevice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonAccess> PersonAccess { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visitor> Visitor { get; set; }
    }
}
