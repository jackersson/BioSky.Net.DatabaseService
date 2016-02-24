namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Photo")]
    public partial class Photo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Photo()
        {
            Visitor = new HashSet<Visitor>();
        }

        public long Id { get; set; }

        [Column("Person Id")]
        public long? Person_Id { get; set; }

        [Column("Fir Pathway")]
        public string Fir_Pathway { get; set; }

        [Column("Image Pathway")]
        [Required]
        public string Image_Pathway { get; set; }

        [Column("Size Type")]
        public byte? Size_Type { get; set; }

        [Column("Origin Type")]
        public byte? Origin_Type { get; set; }

        public virtual Person Person { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visitor> Visitor { get; set; }
    }
}
