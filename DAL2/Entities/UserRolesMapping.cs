using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class UserRolesMapping
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
