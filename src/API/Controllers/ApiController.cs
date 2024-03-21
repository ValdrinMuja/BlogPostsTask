using Domain.Shared;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    protected readonly ISender _sender;

    protected ApiController(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    protected IActionResult HandleFailure(Result result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                BadRequest(
                CreateProblemDetails(
                    "Validation Error", StatusCodes.Status400BadRequest,
                    validationResult.Errors)),
            _ =>
               BadRequest(
               CreateProblemDetails(
                   "Error", StatusCodes.Status400BadRequest,
                   new[] { result.Error }))
        };
    }

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error[] errors) =>
        new()
        {
            Title = title,
            Status = status,
            Extensions = { { "errors", errors.Select(e => new { e.Code, e.Message }) } }
        };
}