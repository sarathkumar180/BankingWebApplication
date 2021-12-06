using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DAL.Entities
{
    public partial class Roles
    {
        public Roles()
        {
            UserRolesMapping = new HashSet<UserRolesMapping>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRolesMapping> UserRolesMapping { get; set; }
    }
}
