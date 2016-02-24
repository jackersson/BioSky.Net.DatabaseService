namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Card")]
    public partial class Card
    {
        public long Id { get; set; }

        [Column("Person Id")]
        public long? Person_Id { get; set; }

        [Column("Unique Number")]
        [Required]
        [StringLength(50)]
        public string Unique_Number { get; set; }

        public virtual Person Person { get; set; }
    }
}
