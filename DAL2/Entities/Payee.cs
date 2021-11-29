using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Payee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public string PayeeName { get; set; }

        [Required]
        public int AccountNumber { get; set; }

        [MinLength(6)]
        [MaxLength(20)]
        public string BankCode { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        [NotMapped]
        public int CustomerNo { get; set; }

        [NotMapped]
        public  bool IsChecked { get; set; }

        [NotMapped]
        public decimal AmountToPay { get; set; }
    }
}
