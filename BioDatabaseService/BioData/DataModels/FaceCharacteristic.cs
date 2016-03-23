namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FaceCharacteristic")]
    public partial class FaceCharacteristic
    {
        public long Id { get; set; }

        public double? Width { get; set; }

        [Column("Biometric Location Id")]
        public long? Biometric_Location_Id { get; set; }

        [Column("Eyes Charachteristic Id")]
        public long? Eyes_Charachteristic_Id { get; set; }

        [Column("Portrait Charachteristic Id")]
        public long? Portrait_Charachteristic_Id { get; set; }

        public virtual BiometricLocation FaceBiometricLocation { get; set; }

        public virtual EyesCharacteristic EyesCharacteristic { get; set; }

        public virtual PortraitCharacteristic PortraitCharacteristic { get; set; }
    }
}
