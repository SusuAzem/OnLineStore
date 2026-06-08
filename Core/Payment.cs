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
        public int OrderHeaderId { get; set; }
        public virtual OrderHeader? OrderHeader { get; set; }

        public int Fee { get; set; }

        public DateTime Date { get; set; }

        public DateTime DueDate { get; set; }

        public string? Status { get; set; }

        public decimal Amount { get; set; }
    }
}
