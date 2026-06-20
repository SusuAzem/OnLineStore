using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("OrderHeader")]
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public virtual OrderHeader? OrderHeader { get; set; }

        public int Fee { get; set; }

        public DateTime Date { get; set; }

        public DateTime DueDate { get; set; }

        public string? Status { get; set; }

        public decimal Amount { get; set; }

        public string? Currency { get; set; }

        public string? CallbackUrl { get; set; }

        public string? TransactionId { get; set; }

        public string? PaymentUrl { get; set; }
    }
}
