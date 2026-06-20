using Core;

namespace OnLineStore.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        public int OrderHeaderId { get; set; }

        public OrderHeaderViewModel? OrderHeader { get; set; }

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
