using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class User
    {
        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
