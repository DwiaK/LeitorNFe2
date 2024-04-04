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
		var result = await _dispatcher.Query<GetNotaFiscalByIdQuery, NotaFiscal>(new GetNotaFiscalByIdQuery(id), cancellationToken);

		return result.Value;
    }

    [HttpPost("ImportarNotaFiscal")]
    public async Task<bool> ImportarNotaFiscal([FromBody] NotaFiscal notaFiscal, CancellationToken cancellationToken)
    {
		await _dispatcher.Send(new CreateNotaFiscalCommand(notaFiscal), cancellationToken);

        return true; // try / catch result
    }

    [HttpPost("ImportarMultiplasNotasFiscais")]
    public async Task<bool> ImportarMultiplasNotasFiscais([FromBody] List<NotaFiscal> notasFiscais, CancellationToken cancellationToken)
    {
		await _dispatcher.Send(new CreateMultiplasNotasFiscaisCommand(notasFiscais), cancellationToken);

        return true;
    }

	[HttpPut("EditarNotaFiscal")]
	public async Task<bool> EditarNotaFiscal([FromBody] NotaFiscal notaFiscal, CancellationToken cancellationToken)
	{
		await _dispatcher.Send(new UpdateNotaFiscalCommand(notaFiscal), cancellationToken);

		return true; // try / catch result
	}

	[HttpDelete("DeletarNotaFiscal/{id}")]
    public async Task<bool> DeletarNotaFiscal(int id, CancellationToken cancellationToken)
    {
		await _dispatcher.Send(new DeleteNotaFiscalCommand(id), cancellationToken);

		return true; // try / catch result
    }

	[HttpGet("BuscarNotasFiscais")]
	public async Task<List<NotaFiscal>> BuscarNotasFiscais(CancellationToken cancellationToken)
	{
		var result = await _dispatcher.Query<GetNotaFiscalQuery, List<NotaFiscal>>(new GetNotaFiscalQuery(), cancellationToken);

		return result.Value;
	}
}
