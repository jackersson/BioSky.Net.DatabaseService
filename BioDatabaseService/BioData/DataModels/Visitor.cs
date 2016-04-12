namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Visitor")]
    public partial class Visitor
    {
        public long Id { get; set; }

        [Column("Person ID")]
        public long? Person_ID { get; set; }

        [Column("Full Photo Id")]
        public long? Full_Photo_Id { get; set; }

        [Column("Detection Time")]
        public DateTime Detection_Time { get; set; }

        [Column("Location Id")]
        public long Location_Id { get; set; }

        public byte Status { get; set; }

        [Column("Card Number")]
        [StringLength(50)]
        public string Card_Number { get; set; }

        [Column("Croped Photo Id")]
        public long? Croped_Photo_Id { get; set; }

        public virtual Location Location { get; set; }

        public virtual Person Person { get; set; }

        public virtual Photo CropedPhoto { get; set; }

        public virtual Photo FullPhoto { get; set; }
    }
}
