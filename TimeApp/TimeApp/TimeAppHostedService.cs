using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TimeApp.Foundation.Blobs;

namespace TimeApp
{
    public sealed class TimeAppHostedService : IHostedService
    {
        private readonly IBlobCleanupService _blobCleanupService;


        public TimeAppHostedService(IBlobCleanupService blobCleanupService)
        {
            _blobCleanupService = blobCleanupService;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _blobCleanupService.InitializeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
