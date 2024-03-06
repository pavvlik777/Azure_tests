using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace TimeApp.Foundation.TimeData
{
    public sealed class TimeDataRepository: ITimeDataRepository
    {
        private const string EndpointUri = "";
        private const string PrimaryKey = "";


        public async Task<IReadOnlyCollection<Models.TimeData>> GetAllAsync()
        {
            using var client = GetClient();
            var container = GetContainer(client);

            var result = new List<Models.TimeData>();
            var iterator = container.GetItemQueryIterator<Models.TimeData>();
            while (iterator.HasMoreResults)
            {
                var items = await iterator.ReadNextAsync();
                result.AddRange(items);
            }

            return result;
        }


        private static CosmosClient GetClient()
        {
            var client = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions { ApplicationName = "TimeWebApp" });

            return client;
        }

        private static Container GetContainer(CosmosClient client)
        {
            var database = client.GetDatabase("time-app");
            var container = database.GetContainer("time-zones");

            return container;
        }
    }
}