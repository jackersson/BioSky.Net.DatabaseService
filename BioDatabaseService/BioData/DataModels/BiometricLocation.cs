namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BiometricLocation")]
    public partial class BiometricLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BiometricLocation()
        {
            LeftEyeCharacteristic = new HashSet<EyesCharacteristic>();
            RightEyeCharacteristic = new HashSet<EyesCharacteristic>();
            FaceCharacteristic = new HashSet<FaceCharacteristic>();
        }

        public long Id { get; set; }

        public double? XPos { get; set; }

        public double? YPos { get; set; }

        public double? Confidence { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EyesCharacteristic> LeftEyeCharacteristic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EyesCharacteristic> RightEyeCharacteristic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FaceCharacteristic> FaceCharacteristic { get; set; }
    }
}
