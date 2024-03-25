namespace TimeApp.Models
{
    public sealed class TimeData
    {
        public string ZoneId { get; set; }

        public string DisplayName { get; set; }

        public int UtcOffsetMinutes { get; set; }

        //public string ImageBlobId { get; set; }

        //public int Ttl { get; set; }
    }
}