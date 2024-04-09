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

	public async Task<IActionResult> LoginAccount([FromBody] LoginRequest request, CancellationToken cancellationToken)
	{
		#region Validação
		if (request is null)
			return null;
		#endregion

		#region Requisição
		var command = new LoginCommand(request.email, request.password);

		Result<string> tokenResult = await _dispatcher.Send<LoginCommand, string>(command, cancellationToken);
		#endregion

		#region Validação
		if (tokenResult.IsFailure)
			return null;
		#endregion

		return Ok(tokenResult);
	}

	[HttpPost]
	public async Task<IActionResult> RegisterAccount([FromBody] RegisterRequest request, CancellationToken cancellationToken)
	{
		#region Validação
		if (request is null)
			return null;
		#endregion

		#region Requisição
		var command = new RegisterCommand(request.email, request.password);

		Result<Guid> result = await _dispatcher.Send<RegisterCommand, Guid>(command, cancellationToken);
		#endregion

		#region Validação
		if (result.IsFailure)
			return null;
		#endregion

		return Ok(result);
	}
}
