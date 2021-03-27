using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models.ViewModels
{
    public class ResetPassword
    {
        public int UserId { get; set; }

        public string OldPassword { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
