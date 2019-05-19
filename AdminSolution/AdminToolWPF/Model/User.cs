using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminToolWPF.Model
{
    public class User
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
