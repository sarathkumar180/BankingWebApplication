using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class User
    {
        [Required]
        [MinLength(5)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
