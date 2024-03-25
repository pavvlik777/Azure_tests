namespace TimeApp.Repositories.Models
{
    public sealed class TimeZone
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string ZoneId { get; set; }

        public string DisplayName { get; set; }

        public int UtcOffsetMinutes { get; set; }

        public string ImageId { get; set; }

        public bool IsBuiltIn { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "ttl")]
        public int Ttl { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "_ts")]
        public long TimeStamp { get; set; }
    }
}