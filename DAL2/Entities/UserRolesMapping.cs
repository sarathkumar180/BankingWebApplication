using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enum;

namespace DAL.Entities
{
    public class UserRolesMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int RoleId { get; set; }
        [NotMapped]
        public int CustomerNo { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }
        [NotMapped]
        public string RoleName { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Roles Role { get; set; }
    }
}
