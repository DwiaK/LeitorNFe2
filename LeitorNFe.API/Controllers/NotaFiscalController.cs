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
        //await using (SqlConnection connection =
        //    new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LeitorNFe;Integrated Security=True"))
        //{
        //    var result = connection.Query<NotaFiscalResponse>(
        //        @"SELECT * 
        //            FROM NotaFiscal
        //            WHERE IdNotaFiscal = @Id", new { id })
        //        .FirstOrDefault();
        //    return Ok(result);
        //}

        // TODO: Realizar operações chamando o Handle
        //var query = new GetNotaFiscalByIdQuery(id);


        //return Results.Ok(new GetNotaFiscalByIdQueryHandler().Handle(new GetNotaFiscalByIdQuery(id), cancellationToken));

        return await new GetNotaFiscalByIdQueryHandler(_sqlConnection)
            .Handle(new GetNotaFiscalByIdQuery(id), cancellationToken);
    }

    [HttpPost("ImportarNotaFiscal")]
    public async Task<bool> ImportarNotaFiscal(NotaFiscal notaFiscal, CancellationToken cancellationToken)
    {
        await new CreateNotaFiscalQueryHandler(_sqlConnection).Handle(new CreateNotaFiscalCommand(notaFiscal), cancellationToken);

        return true;
    }

    [HttpGet("BuscarNotasFiscais")]
    public async Task<List<NotaFiscal>> BuscarNotasFiscais(CancellationToken cancellationToken)
    {
        return await new GetNotaFiscalQueryHandler(_sqlConnection)
            .Handle(new GetNotasFiscaisCommand(), cancellationToken);
    }
}