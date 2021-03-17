using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartContactManager.Models
{
    public class Group
    {
        public int Id { get; set; }
       
        [Required]
        public int? UserId { get; set; }
        public User User { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<GroupContact> GroupContacts { get; set; }
    }
}