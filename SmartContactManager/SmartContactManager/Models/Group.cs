﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartContactManager.Models
{
    public class Group
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}