using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PebaFinance.Application.Commands;
using PebaFinance.Application.DTOs;
using PebaFinance.Application.Queries;

namespace PebaFinance.Api.Controllers
{
    [ApiController]
    [Route("api/v1/expenses")]
    public class ExpenseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all expenses.
        /// </summary>
        /// <param name="filter">Optional filter parameters.</param>
        /// <returns>A list of expense records.</returns>
        /// <response code="200">Expenses successfully retrieved.</response>
        /// <response code="401">Unauthorized access.</response>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetAll([FromQuery] BaseFilterDto filter)
        {
            var query = new GetAllExpensesQuery(filter);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves an expense by ID.
        /// </summary>
        /// <param name="id">The expense ID.</param>
        /// <returns>The expense details.</returns>
        /// <response code="200">Expense successfully retrieved.</response>
        /// <response code="404">Expense not found.</response>
        /// <response code="401">Unauthorized access.</response>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDto>> Get(int id)
        {
            var query = new GetExpenseByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Retrieves expenses for a specific month and year.
        /// </summary>
        /// <param name="year">The year (2025).</param>
        /// <param name="month">The month (1-12).</param>
        /// <returns>List of expenses for the specified period.</returns>
        /// <response code="200">Expenses successfully retrieved.</response>
        /// <response code="401">Unauthorized access.</response>
        [Authorize]
        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByYearAndMonth(int year, int month)
        {
            var query = new GetExpensesByYearAndMonthQuery(year, month);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Creates a new expense.
        /// </summary>
        /// <param name="command">The expense data.</param>
        /// <returns>The ID of the newly created expense.</returns>
        /// <response code="201">Expense successfully created.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="409">Duplicate description for the same month.</response>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateExpenseCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id = result }, result);
        }

        /// <summary>
        /// Updates an existing expense.
        /// </summary>
        /// <param name="id">The expense ID.</param>
        /// <param name="command">The updated expense data.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Expense successfully updated.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="404">Expense not found.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="409">Duplicate description for the same month.</response>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateExpenseCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes an expense.
        /// </summary>
        /// <param name="id">The expense ID to delete.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">Expense successfully deleted.</response>
        /// <response code="404">Expense not found.</response>
        /// <response code="401">Unauthorized access.</response>
        [Authorize]
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
