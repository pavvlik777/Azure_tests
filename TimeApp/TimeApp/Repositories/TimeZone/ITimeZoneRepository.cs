using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeApp.Repositories.TimeZone
{
    public interface ITimeZoneRepository
    {
        Task<Models.TimeZone> GetByIdAsync(string zoneId);

        Task<IReadOnlyCollection<Models.TimeZone>> GetAllAsync();

        Task<Models.TimeZone> AddAsync(Models.TimeZone timeZone);

        Task<Models.TimeZone> ReplaceAsync(string zoneId, Models.TimeZone timeZone);

        Task DeleteAsync(string zoneId);
    }
}