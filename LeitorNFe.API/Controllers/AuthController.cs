using LeitorNFe.API.Abstractions;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Dispatcher;
using LeitorNFe.Application.Authentication.Login;
using LeitorNFe.Application.Authentication.Register;
using LeitorNFe.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace LeitorNFe.API.Controllers;

[Route("api/[controller]")]
public class AuthController : ApiController
{
	public AuthController(IDbConnection dbConnection, IDispatcher dispatcher)
		: base(dbConnection, dispatcher)
	{
	}

	[HttpGet("Login")]
	public async Task<IActionResult> LoginAccount([FromBody] LoginRequest request, CancellationToken cancellationToken)
	{
		if (request is null)
			return null;

		var command = new LoginCommand(request.email, request.password);

		Result<string> tokenResult = await _dispatcher.Send<LoginCommand, string>(command, cancellationToken);

		if (tokenResult.IsFailure)
			return null;

		return Ok(tokenResult);
	}

	[HttpPost("Register")]
	public async Task<IActionResult> RegisterAccount([FromBody] RegisterRequest request, CancellationToken cancellationToken)
	{
		if (request is null)
			return null;

		var command = new RegisterCommand(request.email, request.password);

		Result<Guid> result = await _dispatcher.Send<RegisterCommand, Guid>(command, cancellationToken);

		if (result.IsFailure)
			return null;

		return Ok(result);
	}
}
