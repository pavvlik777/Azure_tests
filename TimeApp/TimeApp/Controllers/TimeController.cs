using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeApp.Foundation.TimeData;
using TimeApp.Models;

namespace TimeApp.Controllers
{
    [ApiController]
    [Route("api/time")]
    public sealed class TimeController : ControllerBase
    {
        private readonly ITimeDataRepository _timeDataRepository;


        public TimeController(ITimeDataRepository timeDataRepository)
        {
            _timeDataRepository = timeDataRepository;
        }


        //static TimeController()
        //{
        //    Timezones = new List<TimeData>()
        //    {
        //        new ()
        //        {
        //            ZoneId = "minsk",
        //            DisplayName = "Minsk",
        //            UtcOffsetMinutes = 3 * 60,
        //        },
        //        new ()
        //        {
        //            ZoneId = "new_york",
        //            DisplayName = "New York",
        //            UtcOffsetMinutes = -5 * 60,
        //        },
        //        new ()
        //        {
        //            ZoneId = "warsaw",
        //            DisplayName = "Warsaw",
        //            UtcOffsetMinutes = 1 * 60,
        //        },
        //        new ()
        //        {
        //            ZoneId = "novosibirsk",
        //            DisplayName = "Novosibirsk",
        //            UtcOffsetMinutes = 7 * 60,
        //        },
        //    };
        //}


        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<TimeData>>> Get()
        {
            var timezones = await _timeDataRepository.GetAllAsync();

            return Ok(timezones);
        }

        [HttpPost]
        public ActionResult<TimeData> Post([FromBody] TimeDataRequest timeData)
        {
            //TODO

            var temp = new TimeData
            {
                ZoneId = "abc",
                DisplayName = timeData.DisplayName,
                UtcOffsetMinutes = timeData.UtcOffsetMinutes,
            };

            return Ok(temp);
        }

        [HttpPut("{id}")]
        public ActionResult<TimeData> Put(string id, [FromBody] TimeDataRequest timeData)
        {
            //TODO

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<TimeData> Delete(string id)
        {
            //TODO

            return Ok();
        }

        [HttpPost("{id}/ttl")]
        public IActionResult SetTtl(string id)
        {
            //TODO

            return Ok();
        }

        [HttpPost("diff/{first}/{second}")]
        public async Task<ActionResult<int>> Diff(string first, string second)
        {
            var timezones = await _timeDataRepository.GetAllAsync();
            var firstTimeZone = timezones.SingleOrDefault(z => z.ZoneId == first);
            if (firstTimeZone == null)
            {
                return BadRequest();
            }

            var secondTimeZone = timezones.SingleOrDefault(z => z.ZoneId == second);
            if (secondTimeZone == null)
            {
                return BadRequest();
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://time-diff-test.azurewebsites.net/api/timeDiff?first={firstTimeZone.UtcOffsetMinutes}&second={secondTimeZone.UtcOffsetMinutes}");
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            var result = await response.Content.ReadAsStringAsync();

            return int.Parse(result);
        }



        public sealed class TimeDataRequest
        {
            public string DisplayName { get; set; }

            public int UtcOffsetMinutes { get; set; }
        }
    }
}
