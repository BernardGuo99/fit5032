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
        [Display(Name = "Branch Name")]
        public string name { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Address")]
        public string address { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Suburb")]
        public string suburb { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "State")]
        public string state { get; set; }

        [Required]
        [StringLength(4)]
        [Display(Name = "Postcode")]
        public string postcode { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "Latitude")]
        [Required]
        [Range(-90.0, 90.0)]
        public decimal latitude { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "Longitude")]
        [Required]
        [Range(-180.0, 180.0)]
        public decimal longitude { get; set; }

        [Display(Name = "Open Time")]
        [Required]
        [DataType(DataType.Time)]
        public DateTime open { get; set; }

        [Display(Name = "Close Time")]
        [Required]
        [DataType(DataType.Time)]
        public DateTime close { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
