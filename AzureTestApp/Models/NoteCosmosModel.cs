using Newtonsoft.Json;

namespace AzureTestApp.Models
{
    public class NoteCosmosModel
    {
        [JsonProperty(PropertyName = "id")] public string Id { get; set; }

        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }

        public string Message { get; set; }
    }
}
