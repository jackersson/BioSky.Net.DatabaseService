namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EyesCharacteristic")]
    public partial class EyesCharacteristic
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EyesCharacteristic()
        {
            FaceCharacteristic = new HashSet<FaceCharacteristic>();
        }

        public long Id { get; set; }

        [Column("Left Eye Location Id")]
        public long? Left_Eye_Location_Id { get; set; }

        [Column("Right Eye Location Id")]
        public long? Right_Eye_Location_Id { get; set; }

        public virtual BiometricLocation LeftEyeBiometricLocation { get; set; }

        public virtual BiometricLocation RightEyeBiometricLocation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FaceCharacteristic> FaceCharacteristic { get; set; }
    }
}
