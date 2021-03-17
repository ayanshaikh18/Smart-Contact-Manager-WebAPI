using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Models
{
    public class GroupContact
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
