using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models.ViewModels
{
    public class AddGroupContactsViewModel
    {
        public int? GroupId { get; set; }
        public int[] ContactIds { get; set; }
    }
}
