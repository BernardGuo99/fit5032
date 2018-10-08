namespace Mel_Medicare_Location_Reservation_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Engagement")]
    public partial class Engagement
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string comment { get; set; }

        [Required]
        [Display(Name = "Added Date")]
        public DateTime? generatedDate { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Initiator")]
        public string initiator { get; set; }

        public int reservationId { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
