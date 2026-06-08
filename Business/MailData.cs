using Core;

using Microsoft.AspNetCore.Http;

namespace Business
{
    public class MailData
    {
        public string? ToId { get; set; }
        public string? ToName { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public OrderHeader? Order { get; set; }
        public IFormFileCollection? EmailAttachments { get; set; }
    }
}
