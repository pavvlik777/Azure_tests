using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeApp.Foundation.Blobs;
using TimeApp.Foundation.TimeData;
using TimeApp.Models;

namespace TimeApp.Controllers
{
    [ApiController]
    [Route("api/time")]
    public sealed class TimeController : ControllerBase
    {
        private readonly ITimeDataRepository _timeDataRepository;
        private readonly IBlobService _blobService;


        public TimeController(ITimeDataRepository timeDataRepository, IBlobService blobService)
        {
            _timeDataRepository = timeDataRepository;
            _blobService = blobService;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<TimeData>>> Get()
        {
            var timezones = await _timeDataRepository.GetAllAsync();

            return Ok(timezones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TimeData>> Get(string id)
        {
            var timeData = await _timeDataRepository.GetByIdAsync(id);
            if (timeData == null)
            {
                return NotFound();
            }

            return Ok(timeData);
        }

        [HttpPost]
        public async Task<ActionResult<TimeData>> Post([FromBody] CreateTimeDataRequest request)
        {
            var timeData = CreateFrom(request);
            var result = await _timeDataRepository.AddAsync(timeData, request.Ttl);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TimeData>> Put(string id, [FromBody] UpdateTimeDataRequest request)
        {
            var timeData = await _timeDataRepository.GetByIdAsync(id);
            if (timeData == null)
            {
                return NotFound();
            }

            var toTimeData = CreateFrom(request);
            var result = await _timeDataRepository.UpdateAsync(timeData, toTimeData);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TimeData>> Delete(string id)
        {
            var timeData = await _timeDataRepository.GetByIdAsync(id);
            if (timeData == null)
            {
                return NotFound();
            }

            await _timeDataRepository.DeleteAsync(id);

            return Ok();
        }

        [HttpPost("diff/{first}/{second}")]
        public async Task<ActionResult<int>> Diff(string first, string second)
        {
            var firstTimeZone = await _timeDataRepository.GetByIdAsync(first);
            if (firstTimeZone == null)
            {
                return BadRequest();
            }

            var secondTimeZone = await _timeDataRepository.GetByIdAsync(second);
            if (secondTimeZone == null)
            {
                return BadRequest();
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:7223/api/TimeDiff?first={firstTimeZone.UtcOffsetMinutes}&second={secondTimeZone.UtcOffsetMinutes}");
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var result = await response.Content.ReadAsStringAsync();

            return int.Parse(result);
        }

        [HttpPatch("{id}/image")]
        public async Task<ActionResult<TimeData>> UpdateImage(string id, IFormFile image)
        {
            var timeData = await _timeDataRepository.GetByIdAsync(id);
            if (timeData == null)
            {
                return NotFound();
            }

            await using (var imageStream = image.OpenReadStream())
            {
                var fileId = $"{Guid.NewGuid()}_{image.FileName}";
                await _blobService.UploadImageAsync(imageStream, fileId);

                var toTimeData = timeData.Clone();
                toTimeData.ImageId = fileId;
                var updatedTimeData = await _timeDataRepository.UpdateAsync(timeData, toTimeData);

                return updatedTimeData;
            }
        }


        private static TimeData CreateFrom(CreateTimeDataRequest request)
        {
            return new TimeData
            {
                DisplayName = request.DisplayName,
                UtcOffsetMinutes = request.UtcOffsetMinutes
            };
        }

        private static TimeData CreateFrom(UpdateTimeDataRequest request)
        {
            return new TimeData
            {
                DisplayName = request.DisplayName,
                UtcOffsetMinutes = request.UtcOffsetMinutes
            };
        }


        public sealed class CreateTimeDataRequest
        {
            public string DisplayName { get; set; }

            public int UtcOffsetMinutes { get; set; }

            public bool Ttl { get; set; }
        }

        public sealed class UpdateTimeDataRequest
        {
            public string DisplayName { get; set; }

            public int UtcOffsetMinutes { get; set; }
        }
    }
}
