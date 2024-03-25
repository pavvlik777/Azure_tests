using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeApp.Foundation.TimeData
{
    public sealed class InMemoryTimeDataRepository : ITimeDataRepository
    {
        private static readonly IList<Models.TimeData> Timezones;


        static InMemoryTimeDataRepository()
        {
            Timezones = new List<Models.TimeData>()
            {
                new ()
                {
                    ZoneId = "minsk",
                    DisplayName = "Minsk",
                    UtcOffsetMinutes = 3 * 60,
                },
                new ()
                {
                    ZoneId = "new_york",
                    DisplayName = "New York",
                    UtcOffsetMinutes = -5 * 60,
                },
                new ()
                {
                    ZoneId = "warsaw",
                    DisplayName = "Warsaw",
                    UtcOffsetMinutes = 1 * 60,
                },
                new ()
                {
                    ZoneId = "novosibirsk",
                    DisplayName = "Novosibirsk",
                    UtcOffsetMinutes = 7 * 60,
                },
            };
        }


        public Task<Models.TimeData> GetByIdAsync(string zoneId)
        {
            var result = Timezones.SingleOrDefault(d => d.ZoneId == zoneId);

            return Task.FromResult(result);
        }

        public Task<IReadOnlyCollection<Models.TimeData>> GetAllAsync()
        {
            IReadOnlyCollection<Models.TimeData> result = Timezones.ToList();

            return Task.FromResult(result);
        }

        public Task<Models.TimeData> AddAsync(Models.TimeData timeData, bool ttl)
        {
            timeData.ZoneId = Guid.NewGuid().ToString();
            Timezones.Add(timeData);

            return Task.FromResult(timeData);
        }

        public Task<Models.TimeData> UpdateAsync(Models.TimeData fromTimeData, Models.TimeData toTimeData)
        {
            var timeDataToUpdate = Timezones.Single(d => d.ZoneId == fromTimeData.ZoneId);
            timeDataToUpdate.DisplayName = toTimeData.DisplayName;
            timeDataToUpdate.UtcOffsetMinutes = toTimeData.UtcOffsetMinutes;

            return Task.FromResult(timeDataToUpdate);
        }

        public Task DeleteAsync(string zoneId)
        {
            var timeDataToRemove = Timezones.SingleOrDefault(d => d.ZoneId == zoneId);
            if (timeDataToRemove != null)
            {
                Timezones.Remove(timeDataToRemove);
            }

            return Task.CompletedTask;
        }
    }
}