namespace TimeApp.Models
{
    public sealed class TimeData
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string ZoneId { get; set; }

        public string DisplayName { get; set; }

        public int UtcOffsetMinutes { get; set; }

        public string ImageId { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "ttl")]
        public int Ttl { get; set; }


        public TimeData Clone()
        {
            return (TimeData)MemberwiseClone();
        }
    }
}