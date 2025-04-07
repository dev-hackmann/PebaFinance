using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PebaFinance.Application.Commands;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Queries;

namespace PebaFinance.Api.Controllers
{
[ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll()
        {
            var query = new GetAllExpensesQuery();
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

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateExpenseCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = result }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateExpenseCommand command)
        {
            if (id != command.Id)
                return BadRequest();

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
