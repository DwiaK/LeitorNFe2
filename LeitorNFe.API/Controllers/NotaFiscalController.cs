﻿using Dapper;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.NotaFiscalFeature.GetById;
using LeitorNFe.Application.NotaFiscalFeature.Create;
using LeitorNFe.Domain.Entities.NotasFiscais;
using Microsoft.AspNetCore.Mvc;
using NotaFiscalFeature.GetById;
using LeitorNFe.Application.NotaFiscalFeature.Get;
using LeitorNFe.Application.NotaFiscalFeature.Delete;
using LeitorNFe.Application.NotaFiscalFeature.Update;

namespace LeitorNFe.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotaFiscalController : ControllerBase
{
    private readonly ISqlConnectionFactory _sqlConnection;

    public NotaFiscalController(ISqlConnectionFactory sqlConnection)
    {
        _sqlConnection = sqlConnection;
    }

    [HttpGet("BuscarNotaFiscalPorId/{id}")]
    public async Task<NotaFiscal> GetNotaFiscalById(int id, CancellationToken cancellationToken)
    {
        var result = await new GetNotaFiscalByIdQueryHandler(_sqlConnection).Handle(new GetNotaFiscalByIdQuery(id), cancellationToken);

        return result.Value;
    }

    [HttpPost("ImportarNotaFiscal")]
    public async Task<bool> ImportarNotaFiscal([FromBody] NotaFiscal notaFiscal, CancellationToken cancellationToken)
    {
        var result = await new CreateNotaFiscalCommandHandler(_sqlConnection).Handle(new CreateNotaFiscalCommand(notaFiscal), cancellationToken);

        if (result.IsSuccess)
            return true;
        else
            return false;
    }

    [HttpPost("ImportarMultiplasNotasFiscais")]
    public async Task<bool> ImportarMultiplasNotasFiscais([FromBody] List<NotaFiscal> notasFiscais, CancellationToken cancellationToken)
    {
        var result = await new CreateMultiplasNotasFiscaisCommandHandler(_sqlConnection).Handle(new CreateMultiplasNotasFiscaisCommand(notasFiscais), cancellationToken);

        if (result.IsSuccess)
            return true;
        else
            return false;
    }

	[HttpPut("EditarNotaFiscal")]
	public async Task<bool> EditarNotaFiscal([FromBody] NotaFiscal notaFiscal, CancellationToken cancellationToken)
	{
		var result = await new UpdateNotaFiscalCommandHandler(_sqlConnection).Handle(new UpdateNotaFiscalCommand(notaFiscal), cancellationToken);

		if (result.IsSuccess)
			return true;
		else
			return false;
	}

	[HttpDelete("DeletarNotaFiscal/{id}")]
    public async Task<bool> DeletarNotaFiscal(int id, CancellationToken cancellationToken)
    {
        var result = await new DeleteNotaFiscalCommandHandler(_sqlConnection).Handle(new DeleteNotaFiscalCommand(id), cancellationToken);

        return true;
    }

    [HttpGet("BuscarNotasFiscais")]
    public async Task<List<NotaFiscal>> BuscarNotasFiscais(CancellationToken cancellationToken)
    {
        var result = await new GetNotaFiscalQueryHandler(_sqlConnection).Handle(new GetNotaFiscalQuery(), cancellationToken);

        return result.Value;
    }
}
