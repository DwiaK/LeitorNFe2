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
using LeitorNFe.SharedKernel;
using LeitorNFe.Application.Abstractions.Messaging;
using Microsoft.IdentityModel.Tokens;

namespace LeitorNFe.API.Controllers;

[Route("api/[controller]")]
public class NotaFiscalController : ApiController
{
	public NotaFiscalController(ISqlConnectionFactory sqlConnection, IDispatcher dispatcher)
		: base(sqlConnection, dispatcher)
	{
	}

	[HttpGet("BuscarNotaFiscalPorId/{id}")]
	public async Task<NotaFiscal> GetNotaFiscalById(int id, CancellationToken cancellationToken)
	{
		#region Validação
		if (Equals(id, 0))
			return new NotaFiscal();
		#endregion

		#region Requisição
		var result = await _dispatcher.Query<GetNotaFiscalByIdQuery, NotaFiscal>(new GetNotaFiscalByIdQuery(id), cancellationToken);
		#endregion

		return result.Value;
	}

	[HttpPost("ImportarNotaFiscal")]
	public async Task<bool> ImportarNotaFiscal([FromBody] NotaFiscal notaFiscal, CancellationToken cancellationToken)
	{
		#region Validação
		if (notaFiscal is null)
			return false;
		#endregion

		#region Requisição
		var result = await _dispatcher.Send<CreateNotaFiscalCommand, bool>(new CreateNotaFiscalCommand(notaFiscal), cancellationToken);
		#endregion

		#region Validações
		if (result.IsSuccess)
			return true;
		#endregion

		return false; // try / catch result
	}

	[HttpPost("ImportarMultiplasNotasFiscais")]
	public async Task<bool> ImportarMultiplasNotasFiscais([FromBody] List<NotaFiscal> notasFiscais, CancellationToken cancellationToken)
	{
		#region Validação
		if (notasFiscais.IsNullOrEmpty())
			return false;
		#endregion

		#region Requisição
		var result = await _dispatcher.Send<CreateMultiplasNotasFiscaisCommand, bool>(new CreateMultiplasNotasFiscaisCommand(notasFiscais), cancellationToken);
		#endregion

		#region Validações
		if (result.IsSuccess)
			return true;
		#endregion

		return false;
	}

	[HttpPut("EditarNotaFiscal")]
	public async Task<bool> EditarNotaFiscal([FromBody] NotaFiscal notaFiscal, CancellationToken cancellationToken)
	{
		#region Validação
		if (notaFiscal is null)
			return false;
		#endregion

		#region Requisição
		var result = await _dispatcher.Send<UpdateNotaFiscalCommand, bool>(new UpdateNotaFiscalCommand(notaFiscal), cancellationToken);
		#endregion

		#region Validações
		if (result.IsSuccess)
			return true;
		#endregion

		return false;
	}

	[HttpDelete("DeletarNotaFiscal/{id}")]
	public async Task<bool> DeletarNotaFiscal(int id, CancellationToken cancellationToken)
	{
		#region Validação
		if (Equals(id, 0))
			return false;
		#endregion

		#region Requisição
		var result = await _dispatcher.Send<DeleteNotaFiscalCommand, bool>(new DeleteNotaFiscalCommand(id), cancellationToken);
		#endregion

		#region Validações
		if (result.IsSuccess)
			return true;
		#endregion

		return false;
	}

	[HttpGet("BuscarNotasFiscais")]
	public async Task<List<NotaFiscal>> BuscarNotasFiscais(CancellationToken cancellationToken)
	{
		#region Requisição
		var result = await _dispatcher.Query<GetNotaFiscalQuery, List<NotaFiscal>>(new GetNotaFiscalQuery(), cancellationToken);
		#endregion

		return result.Value;
	}
}
