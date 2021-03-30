using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models.ViewModels
{
    public class UpdateContact: CreateContact
    {
        public override int? Id { get; set; }
    }
}
