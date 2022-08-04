using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThePeejayAPI.Models
{
    public class EmailConfirm
    {
        public string UserId { get; set; }
        public string UserToken { get; set; }
    }
}
