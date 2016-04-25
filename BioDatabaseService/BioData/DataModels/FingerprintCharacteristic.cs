namespace BioData.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FingerprintCharacteristic")]
    public partial class FingerprintCharacteristic
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public byte Position { get; set; }

        [Column("Fir Url")]
        public string Fir_Url { get; set; }

        [Column("BiometricData Id")]
        public long? BiometricData_Id { get; set; }

        [Column("Photo Id")]
        public long Photo_Id { get; set; }

        public virtual BiometricData BiometricData { get; set; }

        public virtual Photo Photo { get; set; }
    }
}
