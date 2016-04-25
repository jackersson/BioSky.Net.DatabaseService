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

        [Column("Detection Time")]
        public DateTime Detection_Time { get; set; }

        [Column("Location Id")]
        public long Location_Id { get; set; }

        public byte Status { get; set; }

        [Column("Card Number")]
        [StringLength(50)]
        public string Card_Number { get; set; }

        [Column("BiometricData Id")]
        public long? BiometricData_Id { get; set; }

        public virtual BiometricData BiometricData { get; set; }

        public virtual Location Location { get; set; }

        public virtual Person Person { get; set; }
    }
}
