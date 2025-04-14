using MediatR;
using Microsoft.AspNetCore.Mvc;
using PebaFinance.Application.Commands;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Queries;

namespace PebaFinance.Api.Controllers
{
    [ApiController]
    [Route("api/expenses")]
    public class ExpenseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll([FromQuery] BaseFilterDto filter)
        {
            var query = new GetAllExpensesQuery(filter);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> Get(int id)
        {
            var query = new GetExpenseByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByYearAndMonth(int year, int month)
        {
            var query = new GetExpensesByYearAndMonthQuery(year, month);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateExpenseCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = result }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateExpenseCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteExpenseCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
