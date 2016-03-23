namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PortraitCharacteristic")]
    public partial class PortraitCharacteristic
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PortraitCharacteristic()
        {
            FaceCharacteristic = new HashSet<FaceCharacteristic>();
            Photo = new HashSet<Photo>();
        }

        public long Id { get; set; }

        public byte? Age { get; set; }

        [Column("Face Count")]
        public byte? Face_Count { get; set; }

        [Column("Fir Url")]
        public string Fir_Url { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FaceCharacteristic> FaceCharacteristic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Photo> Photo { get; set; }
    }
}
