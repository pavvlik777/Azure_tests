using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeApp.Foundation.TimeData
{
    public interface ITimeDataRepository
    {
        Task<Models.TimeData> GetByIdAsync(string zoneId);

        Task<IReadOnlyCollection<Models.TimeData>> GetAllAsync();

        Task<Models.TimeData> AddAsync(Models.TimeData timeData, bool ttl);

        Task<Models.TimeData> UpdateAsync(Models.TimeData fromTimeData, Models.TimeData toTimeData);

        Task DeleteAsync(string zoneId);
    }
}