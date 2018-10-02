namespace Mel_Medicare_Location_Reservation_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;


    [Table("Reservation")]
    public partial class Reservation
    {
        public int reservationId { get; set; }

        [Display(Name = "Branch Name")]
        public int branchId { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        public string customerId { get; set; }


        //[DataType(DataType.Time)]
        
        [Required]
        [CustomDateRange(ErrorMessage = "Your reservation time should be at least 24 hours and at most 15 days in advance.")]
        [CustomTimeRange]
        [Display(Name = "Reservation Time")]
        public DateTime? date { get; set; }

        public virtual Branch Branch { get; set; }
    }

    
    public class CustomDateRangeAttribute : RangeAttribute
    {
        public CustomDateRangeAttribute() : base(typeof(DateTime), DateTime.Now.AddDays(1).ToString(), DateTime.Now.AddDays(15).ToString())
        { }
    }

    public class CustomTimeRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                DateTime dt = (DateTime)value;
                TimeSpan ts = dt.TimeOfDay;
                TimeSpan start = new TimeSpan(8, 30, 0);
                TimeSpan end = new TimeSpan(16, 30, 0);
                if (ts >= start && ts <= end)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Your reservation time should be with in the openning hours, which is from 8.30am to 4:30pm.");
                }
            }
            catch (Exception e)
            {
                return new ValidationResult("Invalid time input!");
            }
        }
    }

    
}
