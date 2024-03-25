namespace TimeApp.DataContracts
{
    public sealed class TimeZoneDataContract
    {
        public string ZoneId { get; set; }

        public string DisplayName { get; set; }

        public int UtcOffsetMinutes { get; set; }

        public string ImageId { get; set; }

        public bool IsBuiltIn { get; set; }
    }
}