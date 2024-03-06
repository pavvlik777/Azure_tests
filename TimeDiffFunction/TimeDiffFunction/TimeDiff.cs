using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace TimeDiffFunction
{
    public static class TimeDiff
    {
        [FunctionName("TimeDiff")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")]HttpRequest req)
        {
            if (int.TryParse(req.Query["first"], out var firstUtcOffsetMinutes))
            {
                return new BadRequestObjectResult("Invalid first offset.");
            }

            if (int.TryParse(req.Query["second"], out var secondUtcOffsetMinutes))
            {
                return new BadRequestObjectResult("Invalid second offset.");
            }

            return new OkObjectResult(secondUtcOffsetMinutes - firstUtcOffsetMinutes);
        }
    }
}
