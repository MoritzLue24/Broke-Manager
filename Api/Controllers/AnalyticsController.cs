using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/analytics")]
    public class AnalyticsController : ControllerBase
    {
        public AnalyticsController()
        {
            
        }

        [HttpGet("summary/{days}")]
        public async Task<ActionResult> GetSummary([FromRoute] int days)
        {
            return NoContent();
        }

        [HttpGet("forecast/{days}")]
        public async Task<ActionResult> GetForecast([FromRoute] int days)
        {
            return NoContent();
        }
    }
}