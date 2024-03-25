namespace TimeApp.Foundation.TimeZone.Models
{
    public sealed class UpdateTimeZoneInfo
    {
        public string DisplayName { get; set; }

        public int UtcOffsetMinutes { get; set; }
    }
}
