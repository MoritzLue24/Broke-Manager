using Api.DTOs.Analytics;
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
        public async Task<ActionResult<SummaryResponseDto>> GetSummary([FromRoute] int days)
        {
            return NoContent();
        }

        [HttpGet("forecast/{days}")]
        public async Task<ActionResult<ForecastResponseDto>> GetForecast([FromRoute] int days)
        {
            return NoContent();
        }
    }
}