using System.Threading.Tasks;

namespace TimeApp.Foundation.Blobs
{
    public interface IBlobCleanupService
    {
        Task InitializeAsync();
    }
}