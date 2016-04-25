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
        public long Id { get; set; }

        public byte? Age { get; set; }

        [Column("Face Count")]
        public byte? Face_Count { get; set; }

        [Column("Fir Url")]
        public string Fir_Url { get; set; }
    }
}
