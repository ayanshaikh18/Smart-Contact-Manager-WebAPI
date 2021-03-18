using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Models.ViewModels
{
    public class CreateContact
    {
        public virtual int? Id { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name should be less than 100 characters.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [EmailAddress]
        [Display(Name = "Email (Username)")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile number")]
        public string PhoneNumber { get; set; }

        [StringLength(200, ErrorMessage = "Description should be less than 200 characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
