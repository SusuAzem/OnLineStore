using System.Net.Mail;

namespace Business
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(MailData mailData);
        Task<bool> ReceiveEmailAsync(MailData mailData);
    }
}
