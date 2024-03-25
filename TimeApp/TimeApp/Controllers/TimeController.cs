using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeApp.DataContracts;
using TimeApp.Foundation.TimeZone;
using TimeApp.Foundation.TimeZone.Models;

namespace TimeApp.Controllers
{
    [ApiController]
    [Route("api/time")]
    public sealed class TimeController : ControllerBase
    {
        private readonly ITimeZoneService _timeZoneService;


        public TimeController(ITimeZoneService timeZoneService)
        {
            _timeZoneService = timeZoneService;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<TimeZoneDataContract>>> Get()
        {
            var timeZones = await _timeZoneService.GetAllAsync();
            var dataContracts = timeZones.Select(CreateFrom).ToList();

            return Ok(dataContracts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TimeZoneDataContract>> Get(string id)
        {
            var timeZone = await _timeZoneService.GetByIdAsync(id);
            if (timeZone == null)
            {
                return NotFound();
            }

            var dataContract = CreateFrom(timeZone);

            return Ok(dataContract);
        }

        [HttpPost]
        public async Task<ActionResult<TimeZoneDataContract>> Post([FromBody] CreateTimeZoneRequest request)
        {
            var createTimeZoneInfo = CreateFrom(request);
            var createdTimeZone = await _timeZoneService.AddAsync(createTimeZoneInfo);
            var dataContract = CreateFrom(createdTimeZone);

            return Ok(dataContract);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TimeZoneDataContract>> Put(string id, [FromBody] UpdateTimeZoneRequest request)
        {
            var timeZone = await _timeZoneService.GetByIdAsync(id);
            if (timeZone == null)
            {
                return NotFound();
            }

            var updateTimeZoneInfo = CreateFrom(request);
            var updatedTimeZone = await _timeZoneService.UpdateAsync(timeZone, updateTimeZoneInfo);
            var dataContract = CreateFrom(updatedTimeZone);

            return dataContract;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TimeZoneDataContract>> Delete(string id)
        {
            var timeZone = await _timeZoneService.GetByIdAsync(id);
            if (timeZone == null)
            {
                return NotFound();
            }

            if (timeZone.IsBuiltIn)
            {
                return BadRequest();
            }

            await _timeZoneService.DeleteAsync(timeZone);

            return Ok();
        }

        [HttpPost("diff/{first}/{second}")]
        public async Task<ActionResult<int>> Diff(string first, string second)
        {
            var firstTimeZone = await _timeZoneService.GetByIdAsync(first);
            if (firstTimeZone == null)
            {
                return BadRequest();
            }

            var secondTimeZone = await _timeZoneService.GetByIdAsync(second);
            if (secondTimeZone == null)
            {
                return BadRequest();
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://time-diff-test.azurewebsites.net/api/TimeDiff?first={firstTimeZone.UtcOffsetMinutes}&second={secondTimeZone.UtcOffsetMinutes}");
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var result = await response.Content.ReadAsStringAsync();

            return int.Parse(result);
        }

        [HttpPatch("{id}/image")]
        public async Task<ActionResult<TimeZoneDataContract>> UpdateImage(string id, IFormFile image)
        {
            var timeZone = await _timeZoneService.GetByIdAsync(id);
            if (timeZone == null)
            {
                return NotFound();
            }

            await using var imageStream = image.OpenReadStream();

            var updatedTimeZone = await _timeZoneService.UpdateImageAsync(timeZone, imageStream, image.FileName);
            var dataContract = CreateFrom(updatedTimeZone);

            return dataContract;
        }


        private static TimeZoneDataContract CreateFrom(Repositories.Models.TimeZone timeZone)
        {
            return new TimeZoneDataContract
            {
                ZoneId = timeZone.ZoneId,
                DisplayName = timeZone.DisplayName,
                UtcOffsetMinutes = timeZone.UtcOffsetMinutes,
                ImageId = timeZone.ImageId,
                IsBuiltIn = timeZone.IsBuiltIn,
            };
        }

        private static CreateTimeZoneInfo CreateFrom(CreateTimeZoneRequest request)
        {
            return new CreateTimeZoneInfo
            {
                DisplayName = request.DisplayName,
                UtcOffsetMinutes = request.UtcOffsetMinutes,
                Ttl = request.Ttl,
            };
        }

        private static UpdateTimeZoneInfo CreateFrom(UpdateTimeZoneRequest request)
        {
            return new UpdateTimeZoneInfo
            {
                DisplayName = request.DisplayName,
                UtcOffsetMinutes = request.UtcOffsetMinutes
            };
        }


        public sealed class CreateTimeZoneRequest
        {
            public string DisplayName { get; set; }

            public int UtcOffsetMinutes { get; set; }

            public bool Ttl { get; set; }
        }

        public sealed class UpdateTimeZoneRequest
        {
            public string DisplayName { get; set; }

            public int UtcOffsetMinutes { get; set; }
        }
    }
}
