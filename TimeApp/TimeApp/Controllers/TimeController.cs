using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace TimeApp.Controllers
{
    [ApiController]
    [Route("api/time")]
    public sealed class TimeController : ControllerBase
    {
        private static readonly IReadOnlyCollection<TimeData> Timezones;


        static TimeController()
        {
            Timezones = new List<TimeData>()
            {
                new ()
                {
                    ZoneId = "minsk",
                    DisplayName = "Minsk",
                    UtcOffsetMinutes = 3 * 60,
                },
                new ()
                {
                    ZoneId = "new_york",
                    DisplayName = "New York",
                    UtcOffsetMinutes = -5 * 60,
                },
                new ()
                {
                    ZoneId = "warsaw",
                    DisplayName = "Warsaw",
                    UtcOffsetMinutes = 1 * 60,
                },
                new ()
                {
                    ZoneId = "novosibirsk",
                    DisplayName = "Novosibirsk",
                    UtcOffsetMinutes = 7 * 60,
                },
            };
        }


        [HttpGet]
        public ActionResult<IReadOnlyCollection<TimeData>> Get()
        {
            return Ok(Timezones);
        }

        [HttpPost("diff/{first}/{second}")]
        public ActionResult<int> Diff(string first, string second)
        {
            var firstTimeZone = Timezones.SingleOrDefault(z => z.ZoneId == first);
            if (firstTimeZone == null)
            {
                return BadRequest();
            }

            var secondTimeZone = Timezones.SingleOrDefault(z => z.ZoneId == second);
            if (secondTimeZone == null)
            {
                return BadRequest();
            }

            return Ok(secondTimeZone.UtcOffsetMinutes - firstTimeZone.UtcOffsetMinutes);
        }



        public sealed class TimeData
        {
            public string ZoneId { get; set; }

            public string DisplayName { get; set; }

            public int UtcOffsetMinutes { get; set; }
        }
    }
}
