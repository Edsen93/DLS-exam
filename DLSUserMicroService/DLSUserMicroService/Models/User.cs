using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserMicroServiceASP.Models
{
    public class User
    {
        public bool IsAdmin { get; set; }

        public string Username { get; set; }

        public int UserId { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
    }
}