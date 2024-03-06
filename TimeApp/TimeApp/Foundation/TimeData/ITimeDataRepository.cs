using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeApp.Foundation.TimeData
{
    public interface ITimeDataRepository
    {
        Task<IReadOnlyCollection<Models.TimeData>> GetAllAsync();
    }
}