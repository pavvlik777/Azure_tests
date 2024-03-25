namespace TimeApp.Foundation.TimeZone.Models
{
    public sealed class CreateTimeZoneInfo
    {
        public string DisplayName { get; set; }

        public int UtcOffsetMinutes { get; set; }

        public bool Ttl { get; set; }
    }
}
