using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Queries;

namespace PebaFinance.Api.Controllers
{
    [Route("api/summary")]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SummaryController(IMediator mediator)
        {
            _mediator = mediator;
        }

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
