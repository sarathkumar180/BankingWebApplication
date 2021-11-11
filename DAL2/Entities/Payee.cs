using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Payee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [MinLength(6)]
        [MaxLength(20)]
        public string BankCode { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }
    }
}
