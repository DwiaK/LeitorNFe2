using Dapper;
using LeitorNFe.SharedKernel;
using LeitorNFe.Domain.Entities.NotasFiscais;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Text;
using LeitorNFe.Application.NotaFiscalFeature.GetById;

namespace NotaFiscalFeature.GetById;

public sealed class GetNotaFiscalByIdQueryHandler : IQueryHandler<GetNotaFiscalByIdQuery, NotaFiscal>
{
    #region Atributos
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    #endregion

    #region Construtor
    public GetNotaFiscalByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    #endregion

    #region Handler
    public async Task<Result<NotaFiscal>> Handle(GetNotaFiscalByIdQuery query, CancellationToken cancellationToken)
    {
        await using var sqlConnection = _sqlConnectionFactory
            .CreateConnection();

        try
        {
            // Iniciar Conexão Assíncrona
            await sqlConnection.OpenAsync();

            // Buscar query
            var nfQuery = GetNotaFiscalByIdStringQuery();

            var notaFiscal = await sqlConnection
                .QueryFirstOrDefaultAsync<NotaFiscal>
                (nfQuery, new 
                { 
                    IdNotaFiscal = query.id 
                });

            // Se a lista de notas fiscais for nula
            if (notaFiscal is null)
            {
                return Result.Failure<NotaFiscal>(Error.NullValue);
            }

            return Result.Success<NotaFiscal>(notaFiscal);
        }
        catch (Exception)
        {
            return Result.Failure<NotaFiscal>(Error.None);
        }
    }

    #endregion

    #region Database Queries
    public string GetNotaFiscalByIdStringQuery()
    {
        StringBuilder query = new StringBuilder();

        query.AppendLine("SELECT ");
        query.AppendLine("  [NF].* ");
        query.AppendLine("FROM ");
        query.AppendLine("  [NotaFiscal][NF] ");

        query.AppendLine("WHERE");
        query.AppendLine(@"  [NF].[IdNotaFiscal] = @IdNotaFiscal");

        return query.ToString();
    }
    #endregion
}
