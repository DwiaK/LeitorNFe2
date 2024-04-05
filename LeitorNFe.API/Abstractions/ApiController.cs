using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Dispatcher;
using Microsoft.AspNetCore.Mvc;

namespace LeitorNFe.API.Abstractions;

[ApiController]
public abstract class ApiController : ControllerBase
{
	#region Injeções
	protected readonly IDbConnection _dbConnection;
	protected readonly IDispatcher _dispatcher;
	#endregion

	protected ApiController(IDbConnection dbConnection, IDispatcher dispatcher)
    {
		_dbConnection = dbConnection;
		_dispatcher = dispatcher;
    }
}
