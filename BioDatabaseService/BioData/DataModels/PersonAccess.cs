namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PersonAccess")]
    public partial class PersonAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PersonAccess()
        {
            LocationCollection = new HashSet<Location>();
        }

        public long Id { get; set; }

        [Column("Location Id")]
        public long Location_Id { get; set; }

        [Column("Person Id")]
        public long Person_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Location> LocationCollection { get; set; }

        public virtual Location Location { get; set; }

        public virtual Person Person { get; set; }
    }
}
