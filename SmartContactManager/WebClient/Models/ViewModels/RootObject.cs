using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebClient.Models.ViewModels
{
    public class RootObject
    {
        public int status { get; set; }
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public Object data { get; set; }
    }
}