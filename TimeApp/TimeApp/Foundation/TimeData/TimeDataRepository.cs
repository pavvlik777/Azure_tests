using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace TimeApp.Foundation.TimeData
{
    public sealed class TimeDataRepository: ITimeDataRepository
    {
        private const string EndpointUri = "https://twilight-sparkle-azure-db-time-app.documents.azure.com:443";

        private string PrimaryKey => _options.CosmosKey;
        private readonly AzureOptions _options;


        public TimeDataRepository(IOptions<AzureOptions> options)
        {
            _options = options.Value;
        }


        public async Task<Models.TimeData> GetByIdAsync(string zoneId)
        {
            using var client = GetClient();
            var container = GetContainer(client);

            try
            {
                var response = await container.ReadItemAsync<Models.TimeData>(zoneId, new PartitionKey(zoneId));

                return response.Resource;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

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

        public async Task<Models.TimeData> AddAsync(Models.TimeData timeData, bool ttl)
        {
            using var client = GetClient();
            var container = GetContainer(client);

            timeData.ZoneId = Guid.NewGuid().ToString();
            timeData.Id = timeData.ZoneId;
            timeData.Ttl = ttl ? 60 : -1;
            var response = await container.CreateItemAsync(timeData, new PartitionKey(timeData.ZoneId));

            return response.Resource;
        }

        public async Task<Models.TimeData> UpdateAsync(Models.TimeData fromTimeData, Models.TimeData toTimeData)
        {
            using var client = GetClient();
            var container = GetContainer(client);

            fromTimeData.DisplayName = toTimeData.DisplayName;
            fromTimeData.UtcOffsetMinutes = toTimeData.UtcOffsetMinutes;
            fromTimeData.ImageId = toTimeData.ImageId;
            var response = await container.ReplaceItemAsync(fromTimeData, fromTimeData.ZoneId, new PartitionKey(fromTimeData.ZoneId));

            return response.Resource;
        }

        public async Task DeleteAsync(string zoneId)
        {
            using var client = GetClient();
            var container = GetContainer(client);

            await container.DeleteItemAsync<Models.TimeData>(zoneId, new PartitionKey(zoneId));
        }


        private CosmosClient GetClient()
        {
            var client = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions { ApplicationName = "TimeWebApp" });

            return client;
        }

        private static Container GetContainer(CosmosClient client)
        {
            var database = client.GetDatabase("time-app-db");
            var container = database.GetContainer("cityInfos");

            return container;
        }
    }
}