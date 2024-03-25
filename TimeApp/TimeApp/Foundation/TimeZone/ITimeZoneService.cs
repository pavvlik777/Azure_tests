using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TimeApp.Foundation.TimeZone.Models;

namespace TimeApp.Foundation.TimeZone
{
    public interface ITimeZoneService
    {
        Task<Repositories.Models.TimeZone> GetByIdAsync(string zoneId);

        Task<IReadOnlyCollection<Repositories.Models.TimeZone>> GetAllAsync();

        Task<Repositories.Models.TimeZone> AddAsync(CreateTimeZoneInfo createTimeZoneInfo);

        Task<Repositories.Models.TimeZone> UpdateAsync(Repositories.Models.TimeZone timeZone, UpdateTimeZoneInfo updateTimeZoneInfo);

        Task<Repositories.Models.TimeZone> UpdateImageAsync(Repositories.Models.TimeZone timeZone, Stream imageStream, string fileName);

        Task DeleteAsync(Repositories.Models.TimeZone timeZone);
    }
}