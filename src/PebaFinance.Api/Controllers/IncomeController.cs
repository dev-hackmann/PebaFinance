using MediatR;
using Microsoft.AspNetCore.Mvc;
using PebaFinance.Application.Commands;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Queries;

namespace PebaFinance.Api.Controllers
{
   [ApiController]
    [Route("api/[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IncomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetAll()
        {
            var query = new GetAllIncomesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeDto>> Get(int id)
        {
            var query = new GetIncomeByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateIncomeCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = result }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateIncomeCommand command)
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
            var command = new DeleteIncomeCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
