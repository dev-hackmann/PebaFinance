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

        /// <summary>
        /// Retrieves all income records.
        /// </summary>
        /// <param name="filter">Optional filter parameters.</param>
        /// <returns>A list of income records.</returns>
        /// <response code="200">Income records successfully retrieved.</response>
        /// <response code="401">Unauthorized access.</response>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeDto>>> GetAll([FromQuery] BaseFilterDto filter)
        {
            var query = new GetAllIncomesQuery(filter);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves an income record by ID.
        /// </summary>
        /// <param name="id">The income ID.</param>
        /// <returns>The income details.</returns>
        /// <response code="200">Income record successfully retrieved.</response>
        /// <response code="404">Income record not found.</response>
        /// <response code="401">Unauthorized access.</response>
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

        /// <summary>
        /// Retrieves income records for a specific month and year.
        /// </summary>
        /// <param name="year">The year (2025).</param>
        /// <param name="month">The month (1-12).</param>
        /// <returns>List of income records for the specified period.</returns>
        /// <response code="200">Income records successfully retrieved.</response>
        /// <response code="401">Unauthorized access.</response>
        [Authorize]
        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetIncomesByYearAndMonth(int year, int month)
        {
            var query = new GetIncomesByYearAndMonthQuery(year, month);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Creates a new income record.
        /// </summary>
        /// <param name="command">The income data.</param>
        /// <returns>The ID of the newly created income record.</returns>
        /// <response code="201">Income record successfully created.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="409">Duplicate description for the same month.</response>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateIncomeCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = result }, result);
        }

        /// <summary>
        /// Updates an existing income record.
        /// </summary>
        /// <param name="id">The income ID.</param>
        /// <param name="command">The updated income data.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Income record successfully updated.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">Income record not found.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="409">Duplicate description for the same month.</response>
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

        /// <summary>
        /// Deletes an income record.
        /// </summary>
        /// <param name="id">The income ID to delete.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Income record successfully deleted.</response>
        /// <response code="404">Income record not found.</response>
        /// <response code="401">Unauthorized access.</response>
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
