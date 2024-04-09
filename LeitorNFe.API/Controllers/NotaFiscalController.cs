using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.NotaFiscalFeature.GetById;
using LeitorNFe.Application.NotaFiscalFeature.Create;
using LeitorNFe.Domain.Entities.NotasFiscais;
using Microsoft.AspNetCore.Mvc;
using LeitorNFe.Application.NotaFiscalFeature.Get;
using LeitorNFe.Application.NotaFiscalFeature.Delete;
using LeitorNFe.Application.NotaFiscalFeature.Update;
using LeitorNFe.API.Abstractions;
using LeitorNFe.Application.Abstractions.Dispatcher;
using Microsoft.IdentityModel.Tokens;

namespace LeitorNFe.API.Controllers;

[Route("api/[controller]")]
public class NotaFiscalController : ApiController
{
	public NotaFiscalController(IDbConnection dbConnection, IDispatcher dispatcher)
		: base(dbConnection, dispatcher)
	{
	}

	[HttpGet("BuscarNotaFiscalPorId/{id}")]
	public async Task<NotaFiscal> GetNotaFiscalById(int id, CancellationToken cancellationToken)
	{
		if (Equals(id, 0))
			return new NotaFiscal();

		var query = new GetNotaFiscalByIdQuery(id);

		var result = await _dispatcher.Query<GetNotaFiscalByIdQuery, NotaFiscal>(query, cancellationToken);

		return result.Value;
	}

	[HttpPost("ImportarNotaFiscal")]
	public async Task<bool> ImportarNotaFiscal([FromBody] NotaFiscal notaFiscal, CancellationToken cancellationToken)
	{
		if (notaFiscal is null)
			return false;

		var command = new CreateNotaFiscalCommand(notaFiscal);

		var result = await _dispatcher.Send<CreateNotaFiscalCommand, bool>(command, cancellationToken);

		if (result.IsSuccess)
			return true;

		return false;
	}

	[HttpPost("ImportarMultiplasNotasFiscais")]
	public async Task<bool> ImportarMultiplasNotasFiscais([FromBody] List<NotaFiscal> notasFiscais, CancellationToken cancellationToken)
	{
		if (notasFiscais.IsNullOrEmpty())
			return false;

		var command = new CreateMultiplasNotasFiscaisCommand(notasFiscais);

		var result = await _dispatcher.Send<CreateMultiplasNotasFiscaisCommand, bool>(command, cancellationToken);

		if (result.IsSuccess)
			return true;

		return false;
	}

	[HttpPut("EditarNotaFiscal")]
	public async Task<bool> EditarNotaFiscal([FromBody] NotaFiscal notaFiscal, CancellationToken cancellationToken)
	{
		if (notaFiscal is null)
			return false;

		var command = new UpdateNotaFiscalCommand(notaFiscal);

		var result = await _dispatcher.Send<UpdateNotaFiscalCommand, bool>(command, cancellationToken);

		if (result.IsSuccess)
			return true;

		return false;
	}

	[HttpDelete("DeletarNotaFiscal/{id}")]
	public async Task<bool> DeletarNotaFiscal(int id, CancellationToken cancellationToken)
	{
		if (Equals(id, 0))
			return false;

		var command = new DeleteNotaFiscalCommand(id);

		var result = await _dispatcher.Send<DeleteNotaFiscalCommand, bool>(command, cancellationToken);

		if (result.IsSuccess)
			return true;

		return false;
	}

	[HttpGet("BuscarNotasFiscais")]
	public async Task<List<NotaFiscal>> BuscarNotasFiscais(CancellationToken cancellationToken)
	{
		var query = new GetNotaFiscalQuery();

		var result = await _dispatcher.Query<GetNotaFiscalQuery, List<NotaFiscal>>(query, cancellationToken);

		return result.Value;
	}
}
