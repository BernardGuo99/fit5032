namespace Mel_Medicare_Location_Reservation_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Branch")]
    public partial class Branch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Branch()
        {
            Reservations = new HashSet<Reservation>();
        }

        public int branchId { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [StringLength(100)]
        public string address { get; set; }

        [Required]
        [StringLength(100)]
        public string suburb { get; set; }

        [Required]
        [StringLength(100)]
        public string state { get; set; }

        [Required]
        [StringLength(4)]
        public string postcode { get; set; }

        [Column(TypeName = "numeric")]
        public decimal latitude { get; set; }

        [Column(TypeName = "numeric")]
        public decimal longitude { get; set; }

        public DateTime open { get; set; }

        public DateTime close { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
