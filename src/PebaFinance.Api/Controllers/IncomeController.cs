using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PebaFinance.Application.Commands;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Queries;

namespace PebaFinance.Api.Controllers
{
    [ApiController]
    [Route("api/v1/income")]
    public class IncomeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IncomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetAll([FromQuery] BaseFilterDto filter)
        {
            var query = new GetAllIncomesQuery(filter);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeDto>> Get(int id)
        {
            var query = new GetIncomeByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetIncomesByYearAndMonth(int year, int month)
        {
            var query = new GetIncomesByYearAndMonthQuery(year, month);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateIncomeCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = result }, result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateIncomeCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteIncomeCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
