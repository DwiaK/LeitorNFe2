using Dapper;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.NotaFiscalFeature.GetById;
using LeitorNFe.Application.NotaFiscalFeature.Create;
using LeitorNFe.Domain.Entities.NotasFiscais;
using Microsoft.AspNetCore.Mvc;
using NotaFiscalFeature.GetById;
using LeitorNFe.Application.NotaFiscalFeature.Get;

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
        var result = await new GetNotaFiscalByIdQueryHandler(_sqlConnection).Handle(new GetNotaFiscalByIdCommand(id), cancellationToken);

        return result.Value;
    }

    [HttpPost("ImportarNotaFiscal")]
    public async Task<bool> ImportarNotaFiscal([FromBody] NotaFiscal notaFiscal, CancellationToken cancellationToken)
    {
        var result = await new CreateNotaFiscalQueryHandler(_sqlConnection).Handle(new CreateNotaFiscalCommand(notaFiscal), cancellationToken);

        if (result.IsSuccess)
            return true;
        else
            return false;
    }

    [HttpGet("BuscarNotasFiscais")]
    public async Task<List<NotaFiscal>> BuscarNotasFiscais(CancellationToken cancellationToken)
    {
        var result = await new GetNotaFiscalQueryHandler(_sqlConnection).Handle(new GetNotaFiscalCommand(), cancellationToken);

        return result.Value;
    }
}
