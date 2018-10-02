using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mel_Medicare_Location_Reservation_System.Models
{
    public class UserViewModel
    {
        public UserViewModel() { }

        public UserViewModel(ApplicationUser user)
        {
            Email = user.Email;
            UserName = user.UserName;
            PhoneNumber = user.PhoneNumber;
            Id = user.Id;
           

        }


        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        public string Id { get; set; }
        
        
        
    }
}