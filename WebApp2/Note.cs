using Newtonsoft.Json;

namespace WebApp2
{
    public class Note
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }
        public string Message { get; set; }
    }
}
