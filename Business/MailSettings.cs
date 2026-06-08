namespace Business
{
    public class MailSettings
    {
        public string? Host { get; set; }
        public int SMTPPort { get; set; }
        public string? SenderName { get; set; }
        public string? SenderEmail { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int IMAPPort { get; set; }
        public bool EnableSsl { get; set; }
    }
}
