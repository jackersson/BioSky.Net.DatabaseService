namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccessDevice")]
    public partial class AccessDevice
    {
        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string PortName { get; set; }

        [Column("Location Id")]
        public long? Location_Id { get; set; }

        [Column("Location Type")]
        public byte? Location_Type { get; set; }

        public virtual Location Location { get; set; }
    }
}
