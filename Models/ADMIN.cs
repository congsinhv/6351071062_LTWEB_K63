namespace bookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADMIN")]
    public partial class ADMIN
    {
        [Key]
        [StringLength(30)]
        public string UserAdmin { get; set; }

        [Required]
        [StringLength(30)]
        public string PassAdmin { get; set; }

        [StringLength(50)]
        public string Hoten { get; set; }
    }
}
