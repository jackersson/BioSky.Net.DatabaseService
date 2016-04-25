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
            FaceCharacteristic = new HashSet<FaceCharacteristic>();
            FingerprintCharacteristic = new HashSet<FingerprintCharacteristic>();
            Person = new HashSet<Person>();
        }

        public long Id { get; set; }

        [Column("Photo Url")]
        [Required]
        public string Photo_Url { get; set; }

        [Column("Size Type")]
        public byte? Size_Type { get; set; }

        [Column("Origin Type")]
        public byte? Origin_Type { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public DateTime? Datetime { get; set; }

        [Column("Owner Id")]
        public long? Owner_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FaceCharacteristic> FaceCharacteristic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FingerprintCharacteristic> FingerprintCharacteristic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person> Person { get; set; }

        public virtual Person Owner { get; set; }
    }
}
