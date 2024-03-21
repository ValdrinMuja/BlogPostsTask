using Application.Abstractions;
using Application.Accounts.Commands;
using Application.Accounts.Commands.Login;
using Application.Accounts.Commands.Register;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Application.Accounts.Commands.Login.LoginRequest;
using RegisterRequest = Application.Accounts.Commands.Register.RegisterRequest;

namespace API.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IJwtProvider _jwtProvider;

        public AccountController(ISender sender, IJwtProvider jwtProvider)
            : base(sender)
        {
            _jwtProvider = jwtProvider;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            LoginCommand command = request.Adapt<LoginCommand>();

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            AuthenticationResponse response = result.Value.Adapt<AuthenticationResponse>();

            response.AccessToken = await _jwtProvider.GenerateAsync(result.Value);

            return Ok(response);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            RegisterCommand command = request.Adapt<RegisterCommand>();

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }
    }
}
