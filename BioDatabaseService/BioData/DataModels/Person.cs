namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Person")]
    public partial class Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person()
        {
            Card = new HashSet<Card>();
            Photo = new HashSet<Photo>();
        }

        public long Id { get; set; }

        [Column("First Name ")]
        [Required]
        [StringLength(50)]
        public string First_Name_ { get; set; }

        [Column("Last Name ")]
        [Required]
        [StringLength(50)]
        public string Last_Name_ { get; set; }

        [Column("Date Of Birth")]
        public DateTime? Date_Of_Birth { get; set; }

        public byte Gender { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public long? Thumbnail { get; set; }

        public string Comments { get; set; }

        public byte Rights { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Card> Card { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Photo> Photo { get; set; }
    }
}
