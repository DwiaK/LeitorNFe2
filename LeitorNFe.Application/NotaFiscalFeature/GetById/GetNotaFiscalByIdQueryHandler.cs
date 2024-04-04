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
using LeitorNFe.Domain.Entities.Enderecos;
using System.Linq;

namespace LeitorNFe.Application.NotaFiscalFeature.GetById;

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
                .QueryAsync<NotaFiscal, Endereco, Endereco, NotaFiscal>
                (nfQuery, (notaFiscal, emitente, destinatario) =>
                {
                    notaFiscal.EnderecoEmitente = emitente;
                    notaFiscal.EnderecoDestinatario = destinatario;

                    return notaFiscal;
                },
                new { IdNotaFiscal = query.id },
                splitOn: "IdNotaFiscalEnderecos");
            
            notaFiscal.FirstOrDefault();

            // Se a lista de notas fiscais for nula
            if (notaFiscal is null)
            {
                return Result.Failure<NotaFiscal>(Error.NullValue);
            }

            return Result.Success<NotaFiscal>(notaFiscal.FirstOrDefault());
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

        query.AppendLine($"SELECT ");
        query.AppendLine($"  [NF].*, ");
        query.AppendLine($"  [EnderecoEmitente].*, ");
        query.AppendLine($"  [EnderecoDestinatario].* ");
        query.AppendLine($"    FROM ");
        query.AppendLine($"        [NotaFiscal][NF] ");

        query.AppendLine($"    INNER JOIN ");
        query.AppendLine($"        [NotaFiscalEnderecos][EnderecoEmitente] ");
        query.AppendLine($"        ON ");
        query.AppendLine($"            [EnderecoEmitente].[IdNotaFiscal] = [NF].[IdNotaFiscal] ");
        query.AppendLine($"        AND ");
        query.AppendLine($"            [EnderecoEmitente].[IsEmit] = 1 ");

        query.AppendLine($"    INNER JOIN ");
        query.AppendLine($"        [NotaFiscalEnderecos][EnderecoDestinatario] ");
        query.AppendLine($"        ON ");
        query.AppendLine($"            [EnderecoDestinatario].[IdNotaFiscal] = [NF].[IdNotaFiscal] ");
        query.AppendLine($"        AND ");
        query.AppendLine($"            [EnderecoDestinatario].[IsEmit] = 0 ");
        query.AppendLine($"WHERE ");
        query.AppendLine($"    [NF].[IdNotaFiscal] = @IdNotaFiscal ");

        return query.ToString();
    }
    #endregion
}
