namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CaptureDevice")]
    public partial class CaptureDevice
    {
        public long Id { get; set; }

        [Column("Location Id")]
        public long? Location_Id { get; set; }

        [Column("Device Name")]
        [Required]
        [StringLength(50)]
        public string Device_Name { get; set; }

        public virtual Location Location { get; set; }
    }
}
