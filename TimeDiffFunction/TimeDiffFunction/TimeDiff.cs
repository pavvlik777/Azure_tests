using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace TimeDiffFunction
{
    public static class TimeDiff
    {
        private static readonly IReadOnlyCollection<TimeData> Timezones;


        static TimeDiff()
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


        [FunctionName("TimeDiff")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            var first = req.Query["first"];
            var second = req.Query["second"];
            var firstTimeZone = Timezones.SingleOrDefault(z => z.ZoneId == first);
            if (firstTimeZone == null)
            {
                return new BadRequestObjectResult("Zone not found.");
            }

            var secondTimeZone = Timezones.SingleOrDefault(z => z.ZoneId == second);
            if (secondTimeZone == null)
            {
                return new BadRequestObjectResult("Zone not found.");
            }

            return new OkObjectResult(secondTimeZone.UtcOffsetMinutes - firstTimeZone.UtcOffsetMinutes);
        }



        public sealed class TimeData
        {
            public string ZoneId { get; set; }

            public string DisplayName { get; set; }

            public int UtcOffsetMinutes { get; set; }
        }
    }
}
