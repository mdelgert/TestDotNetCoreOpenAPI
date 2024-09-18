using System.ComponentModel;

namespace AzureTestApp.Models
{
    public class MessageBlobModel
    {
        [DefaultValue("hello1")]
        public string? Message { get; set; }
    }
}
