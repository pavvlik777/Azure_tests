using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using TimeApp.Options;

namespace TimeApp.Repositories.TimeZone
{
    public sealed class TimeZoneRepository : ITimeZoneRepository
    {
        private readonly AzureCosmosOptions _options;

        private readonly Container _container;


        public TimeZoneRepository(IOptions<AzureCosmosOptions> options)
        {
            _options = options.Value;

            _container = GetContainer();
        }


        public async Task<Models.TimeZone> GetByIdAsync(string zoneId)
        {
            try
            {
                var response = await _container.ReadItemAsync<Models.TimeZone>(zoneId, new PartitionKey(zoneId));

                return response.Resource;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IReadOnlyCollection<Models.TimeZone>> GetAllAsync()
        {
            var result = new List<Models.TimeZone>();
            var iterator = _container.GetItemQueryIterator<Models.TimeZone>();
            while (iterator.HasMoreResults)
            {
                var items = await iterator.ReadNextAsync();
                result.AddRange(items);
            }

            return result;
        }

        public async Task<Models.TimeZone> AddAsync(Models.TimeZone timeZone)
        {
            var response = await _container.CreateItemAsync(timeZone, new PartitionKey(timeZone.ZoneId));

            return response.Resource;
        }

        public async Task<Models.TimeZone> ReplaceAsync(string zoneId, Models.TimeZone timeZone)
        {
            var response = await _container.ReplaceItemAsync(timeZone, zoneId, new PartitionKey(zoneId));

            return response.Resource;
        }

        public async Task DeleteAsync(string zoneId)
        {
            await _container.DeleteItemAsync<Models.TimeZone>(zoneId, new PartitionKey(zoneId));
        }


        private Container GetContainer()
        {
            var client = new CosmosClient(_options.EndpointUri, _options.Key, new CosmosClientOptions { ApplicationName = "TimeWebApp" });
            var database = client.GetDatabase(_options.Database);
            var container = database.GetContainer("cityInfos");

            return container;
        }
    }
}