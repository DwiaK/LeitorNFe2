using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Dispatcher;
using Microsoft.AspNetCore.Mvc;

namespace LeitorNFe.API.Abstractions;

[ApiController]
public abstract class ApiController : ControllerBase
{
	#region Injeções
	protected readonly ISqlConnectionFactory _sqlConnection;
	protected readonly IDispatcher _dispatcher;
	#endregion

	protected ApiController(ISqlConnectionFactory sqlConnection, IDispatcher dispatcher)
    {
		_sqlConnection = sqlConnection;
		_dispatcher = dispatcher;
    }
}
