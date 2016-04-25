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

        public byte? Age { get; set; }

        public byte? Gender { get; set; }

        public string FirUrl { get; set; }

        [Column("BiometricData Id")]
        public long? BiometricData_Id { get; set; }

        [Column("Photo Id")]
        public long Photo_Id { get; set; }

        public virtual BiometricData BiometricData { get; set; }

        public virtual BiometricLocation BiometricLocation { get; set; }

        public virtual EyesCharacteristic EyesCharacteristic { get; set; }

        public virtual Photo Photo { get; set; }
    }
}
