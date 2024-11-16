namespace bookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VIETSACH")]
    public partial class VIETSACH
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaTG { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Masach { get; set; }

        [StringLength(50)]
        public string Vaitro { get; set; }

        [StringLength(50)]
        public string Vitri { get; set; }

        public virtual SACH SACH { get; set; }

        public virtual TACGIA TACGIA { get; set; }
    }
}
