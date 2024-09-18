using System.ComponentModel;

namespace AzureTestApp.Models
{
    public class MessageModel
    {
        [DefaultValue("hello1")]
        public string? Message { get; set; }
    }
}
