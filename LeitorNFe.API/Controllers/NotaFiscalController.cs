using Dapper;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.NotaFiscalFeature.GetById;
using LeitorNFe.Application.NotaFiscalFeature.Create;
using LeitorNFe.Domain.Entities.NotasFiscais;
using Microsoft.AspNetCore.Mvc;
using LeitorNFe.Application.NotaFiscal.Create;

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

    [HttpGet("GetNotaFiscalById/{id}")]
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

    [HttpPost("ImportNotaFiscal")]
    public async Task<bool> ImportNotaFiscal(NotaFiscal notaFiscal, CancellationToken cancellationToken)
    {
        new CreateNotaFiscalQueryHandler(_sqlConnection).Handle(new CreateNotaFiscalCommand(notaFiscal), cancellationToken);

        return true;
    }
}