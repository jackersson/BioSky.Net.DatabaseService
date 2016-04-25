namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FingerprintDevice")]
    public partial class FingerprintDevice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FingerprintDevice()
        {
            Location = new HashSet<Location>();
        }

        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DeviceName { get; set; }

        //[Required]
        [StringLength(50)]
        public string SerialNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Location> Location { get; set; }
    }
}
