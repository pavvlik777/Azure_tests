namespace TimeApp.Options
{
    public sealed class AzureBlobOptions
    {
        public const string SectionName = "AzureBlob";


        public string AccountName { get; set; }

        public string SasKey { get; set; }
    }
}
