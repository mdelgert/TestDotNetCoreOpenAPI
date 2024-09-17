using System.ComponentModel;

namespace WebApp3.Models
{
    public class MessageModel
    {
        [DefaultValue("hello1")]
        public string? Message { get; set; }
    }
}