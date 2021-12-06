using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Payee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public string PayeeName { get; set; }

        [Required]
        public int PayeeAccountNumber { get; set; }

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

        [NotMapped]
        public int FromAccountNo { get; set; }

        public Payee()
        {
            PaymentHistory = new HashSet<PaymentHistory>();
        }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistory { get; set; }
    }
}
