using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Models.ViewModels
{
    public class UpdateContact: CreateContact
    {
        [Required]
        public override int? Id { get; set; }
    }
}
