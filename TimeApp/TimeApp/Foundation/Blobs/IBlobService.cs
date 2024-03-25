using System.IO;
using System.Threading.Tasks;

namespace TimeApp.Foundation.Blobs
{
    public interface IBlobService
    {
        Task UploadImageAsync(Stream image, string filename);
    }
}
