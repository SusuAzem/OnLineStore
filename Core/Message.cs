using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Message
    {
        public Message()
        {                
        }

        public Message(string name, string email, string content, string phoneNumber)
        {
            Name = name;
            Email = email;
            Content = content;
            PhoneNumber = phoneNumber;
            TimeSend = DateTime.Now;
        }
        public int Id { get; set; }
        
        public string? Name { get; set; }
       
        public string? Email { get; set; }

        public string? Content { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime TimeSend { get; set; }
    }
}
