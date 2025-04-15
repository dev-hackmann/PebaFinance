using MediatR;
using Microsoft.AspNetCore.Mvc;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Queries;
using PebaFinance.Application.DTOs;

namespace PebaFinance.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="command">The registration data.</param>
    /// <returns>The ID of the newly created user.</returns>
    /// <response code="201">User successfully registered.</response>
    /// <response code="400">Invalid input data.</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Register), new { id = result }, result);
    }


    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="query">The user login credentials.</param>
    /// <returns>JWT token and authenticated user info.</returns>
    /// <response code="200">Login successful.</response>
    /// <response code="401">Invalid credentials.</response>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] AuthenticateUserQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
