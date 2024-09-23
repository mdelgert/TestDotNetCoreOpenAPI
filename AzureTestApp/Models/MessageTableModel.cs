using Azure;
using Azure.Data.Tables;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace AzureTestApp.Models
{
    public class MessageTableModel : ITableEntity
    {
        [DefaultValue("DefaultPartition")]
        public string PartitionKey { get; set; }  // PartitionKey to segment data

        [DefaultValue("rowkey-default")]
        public string RowKey { get; set; }        // RowKey is a unique identifier per PartitionKey

        [DefaultValue("Hello1")]
        public string Message { get; set; }       // Custom property

        [DefaultValue(typeof(DateTimeOffset), "")]
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.UtcNow;  // Default to current UTC time

        [JsonIgnore]   // Ignore the ETag in the JSON input and output
        public ETag ETag { get; set; } = ETag.All;   // Default to ETag.All for inserts
    }
}
