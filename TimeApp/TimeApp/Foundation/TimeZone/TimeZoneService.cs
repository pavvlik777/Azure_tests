using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TimeApp.Foundation.Blobs;
using TimeApp.Foundation.TimeZone.Models;
using TimeApp.Repositories.TimeZone;

namespace TimeApp.Foundation.TimeZone
{
    public sealed class TimeZoneService : ITimeZoneService
    {
        private const int TtlLength = 60;

        private readonly ITimeZoneRepository _timeZoneRepository;
        private readonly IBlobService _blobService;


        public TimeZoneService(ITimeZoneRepository timeZoneRepository, IBlobService blobService)
        {
            _timeZoneRepository = timeZoneRepository;
            _blobService = blobService;
        }


        public async Task<Repositories.Models.TimeZone> GetByIdAsync(string zoneId)
        {
            var timeZone = await _timeZoneRepository.GetByIdAsync(zoneId);

            return timeZone;
        }

        public async Task<IReadOnlyCollection<Repositories.Models.TimeZone>> GetAllAsync()
        {
            var timeZones = await _timeZoneRepository.GetAllAsync();

            return timeZones;
        }

        public async Task<Repositories.Models.TimeZone> AddAsync(CreateTimeZoneInfo createTimeZoneInfo)
        {
            var id = Guid.NewGuid().ToString();
            var model = new Repositories.Models.TimeZone
            {
                Id = id,
                ZoneId = id,
                DisplayName = createTimeZoneInfo.DisplayName,
                UtcOffsetMinutes = createTimeZoneInfo.UtcOffsetMinutes,
                ImageId = null,
                IsBuiltIn = false,
                Ttl = createTimeZoneInfo.Ttl ? TtlLength : -1,
            };

            var createdTimeZone = await _timeZoneRepository.AddAsync(model);

            return createdTimeZone;
        }

        public async Task<Repositories.Models.TimeZone> UpdateAsync(Repositories.Models.TimeZone timeZone, UpdateTimeZoneInfo updateTimeZoneInfo)
        {
            timeZone.DisplayName = updateTimeZoneInfo.DisplayName;
            timeZone.UtcOffsetMinutes  = updateTimeZoneInfo.UtcOffsetMinutes;
            UpdateTtl(timeZone);

            var updatedTimeZone = await _timeZoneRepository.ReplaceAsync(timeZone.ZoneId, timeZone);

            return updatedTimeZone;
        }

        public async Task<Repositories.Models.TimeZone> UpdateImageAsync(Repositories.Models.TimeZone timeZone, Stream imageStream, string fileName)
        {
            var imageId = $"{Guid.NewGuid()}_{fileName}";
            var oldImageId = timeZone.ImageId;
            if (oldImageId != null)
            {
                await _blobService.DeleteImageAsync(oldImageId);
            }

            await _blobService.UploadImageAsync(imageStream, imageId);

            timeZone.ImageId = imageId;
            UpdateTtl(timeZone);

            var updatedTimeZone = await _timeZoneRepository.ReplaceAsync(timeZone.ZoneId, timeZone);

            return updatedTimeZone;
        }

        public async Task DeleteAsync(Repositories.Models.TimeZone timeZone)
        {
            var oldImageId = timeZone.ImageId;
            if (oldImageId != null)
            {
                await _blobService.DeleteImageAsync(oldImageId);
            }

            await _timeZoneRepository.DeleteAsync(timeZone.ZoneId);
        }


        private static void UpdateTtl(Repositories.Models.TimeZone timeZone)
        {
            if (timeZone.Ttl == -1)
            {
                return;
            }

            var timeStampNow = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
            var timeStampTtl = timeZone.TimeStamp + timeZone.Ttl;
            var newTtl = (int)(timeStampTtl - timeStampNow);
            if (newTtl > 0)
            {
                timeZone.Ttl = newTtl;
            }
        }
    }
}