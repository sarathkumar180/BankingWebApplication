using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Enum;

namespace DAL.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public int CustomerNo { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(63)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        
        [MaxLength(50)]
        public string Address { get; set; }
    

        public RoleEnum CustomerRole { get; set; }

     

        public List<Account> accounts = new List<Account>();
    }
}
