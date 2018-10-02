using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mel_Medicare_Location_Reservation_System.Models
{
    public class RoleViewModel
    {
        public RoleViewModel() { }

        public RoleViewModel(ApplicationRole role)
        {
            Id = role.Id;
            Name = role.Name;
        }
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name="Role Name")]
        public string Name { get; set; }
    }
}