using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using TimeApp.Repositories.TimeZone;

namespace TimeApp.Foundation.Blobs
{
    public sealed class BlobCleanupService : IBlobCleanupService
    {
        private static readonly TimeSpan CleanupTimerInterval = TimeSpan.FromHours(1);

        private readonly ITimeZoneRepository _timeZoneRepository;
        private readonly IBlobService _blobService;

        private readonly Timer _cleanupTimer;


        public BlobCleanupService(ITimeZoneRepository timeZoneRepository, IBlobService blobService)
        {
            _timeZoneRepository = timeZoneRepository;
            _blobService = blobService;

            _cleanupTimer = new Timer(CleanupTimerInterval.TotalMilliseconds);
            _cleanupTimer.Elapsed += CleanupTimerOnElapsed;
        }


        public Task InitializeAsync()
        {
            _cleanupTimer.Start();
            Task.Run(CleanupBlobsAsync);

            return Task.CompletedTask;
        }


        private async void CleanupTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            await CleanupBlobsAsync();
        }

        private async Task CleanupBlobsAsync()
        {
            var timeZones = await _timeZoneRepository.GetAllAsync();
            var imageIds = timeZones.Where(z => z.ImageId != null).Select(z => z.ImageId).ToList();

            await _blobService.DeleteAllImagesExceptAsync(imageIds);
        }
    }
}
