namespace TimeApp
{
    public sealed class AzureOptions
    {
        public const string SectionName = "Azure";


        public string CosmosKey { get; set; }

        public string BlobKey { get; set; }
    }
}
