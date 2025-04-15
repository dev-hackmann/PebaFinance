using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Queries;

namespace PebaFinance.Api.Controllers
{
    [Route("api/v1/summary")]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SummaryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a financial summary for a specific month and year.
        /// </summary>
        /// <param name="year">The year (2025).</param>
        /// <param name="month">The month (1-12).</param>
        /// <returns>Financial summary for the specified period.</returns>
        /// <response code="200">Summary successfully retrieved.</response>
        /// <response code="401">Unauthorized access.</response>
        [Authorize]
        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<SummaryDto>> GetSummaryByYearAndMonth(int year, int month)
        {
            var query = new GetSummaryByYearAndMonthQuery(year, month);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
