using System.Security.Claims;
using Api.DTOs.Analytics;
using Api.Exceptions;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("summary/{days}")]
        public async Task<ActionResult<SummaryResponseDto>> GetSummary([FromRoute] int days)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }

        [Authorize]
        [HttpGet("forecast/{days}")]
        public async Task<ActionResult<ForecastResponseDto>> GetForecast([FromRoute] int days)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new SecurityClaimNotFoundException(ClaimTypes.NameIdentifier)
            );
            await Task.CompletedTask;
            return NoContent();
        }
    }
}