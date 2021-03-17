using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContactManager.Models
{
    public class GroupContact
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public int? ContactId { get; set; }
    }
}