using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class PaymentHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int AccountId { get; set; }

        public  decimal Amount { get; set; }

        [Required]
        public int FromAccount { get; set; }

        [Required]
        public int ToAccount { get; set; }

        [Required]
        public int PayeeId { get; set; }

        [MaxLength(100)]
        public string Reference { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public virtual Account Account { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Payee Payee { get; set; }
    }
}
