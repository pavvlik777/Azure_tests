namespace TimeApp.Options
{
    public sealed class AzureCosmosOptions
    {
        public const string SectionName = "AzureCosmos";


        public string EndpointUri { get; set; }

        public string Key { get; set; }

        public string Database { get; set; }
    }
}
