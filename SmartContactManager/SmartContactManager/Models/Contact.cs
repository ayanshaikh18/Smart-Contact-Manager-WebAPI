using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smart_Contact_Manager.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}