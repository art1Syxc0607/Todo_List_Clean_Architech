using Application.Commands.CreateTask;
using Application.DTOs.Task;
using Application.Queries.GetTask;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
        => _mediator = mediator;

    private int GetCurrentUserId()
    {
        var claim = User.FindFirst("personId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
        return int.Parse(claim!.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTask), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var userId = GetCurrentUserId();  // получаем из JWT токена

        var query = new GetTaskQuery
        {
            Id = id,
            UserId = userId
        };

        var result = await _mediator.Send(query);
        return Ok(result); 
    }


}