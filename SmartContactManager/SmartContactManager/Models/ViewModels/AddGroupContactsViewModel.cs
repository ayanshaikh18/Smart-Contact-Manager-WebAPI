using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Models.ViewModels
{
    public class AddGroupContactsViewModel
    {
        [Required]
        public int? GroupId { get; set; }
        [Required]
        public int[]? ContactIds { get; set; }
    }
}
