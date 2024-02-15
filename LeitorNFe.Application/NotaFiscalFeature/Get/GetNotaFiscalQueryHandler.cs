using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.Domain.Entities.Enderecos;
using LeitorNFe.Domain.Entities.NotasFiscais;
using LeitorNFe.SharedKernel;
using Microsoft.IdentityModel.Tokens;
using Dapper;
using System.Linq;

namespace LeitorNFe.Application.NotaFiscalFeature.Get;



public class GetNotaFiscalQueryHandler : IQueryHandler<GetNotaFiscalQuery, List<NotaFiscal>>
{
    #region Atributos
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    #endregion

    #region Construtor
    public GetNotaFiscalQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    #endregion

    #region Handle
    public async Task<Result<List<NotaFiscal>>> Handle(GetNotaFiscalQuery command, CancellationToken cancellationToken)
    {
        await using var sqlConnection = _sqlConnectionFactory
            .CreateConnection();

        try
        {
            // Iniciar Conexão Assíncrona
            await sqlConnection.OpenAsync();

            // Buscar query
            var nfQuery = GetNotasFiscaisStringQuery();

            var notasFiscais = (await sqlConnection.QueryAsync<NotaFiscal, Endereco, Endereco, NotaFiscal>(
                nfQuery,
                (notaFiscal, enderecoEmitente, enderecoDestinatario) =>
                {
                    notaFiscal.EnderecoEmitente = enderecoEmitente;
                    notaFiscal.EnderecoDestinatario = enderecoDestinatario;
                    return notaFiscal;
                },
                splitOn: "IdNotaFiscalEnderecos, IdNotaFiscalEnderecos"
            )).ToList();

            // Se a lista de notas fiscais for nula
            if (notasFiscais.IsNullOrEmpty())
            {
                return Result.Failure<List<NotaFiscal>>(Error.NullValue);
            }

            return Result.Success<List<NotaFiscal>>(notasFiscais);
        }
        catch (Exception)
        {
            return Result.Failure<List<NotaFiscal>>(Error.None);
        }

    }
    #endregion

    #region Database Queries
    public string GetNotasFiscaisStringQuery()
    {
        StringBuilder query = new StringBuilder();

        query.AppendLine("SELECT ");
        query.AppendLine("  [NF].*, ");
        query.AppendLine("  [NFEemit].*, ");
        query.AppendLine("  [NFEdest].* ");
        query.AppendLine("FROM ");
        query.AppendLine("  [NotaFiscal][NF] ");

        query.AppendLine("INNER JOIN");
        query.AppendLine("  [NotaFiscalEnderecos][NFEemit]");
        query.AppendLine("  ON ");
        query.AppendLine("    [NF].[IdNotaFiscal] = [NFEemit].[IdNotaFiscal]");
        query.AppendLine("  AND ");
        query.AppendLine("    [NFEemit].[IsEmit] = 1");

        query.AppendLine("INNER JOIN");
        query.AppendLine("  [NotaFiscalEnderecos][NFEdest]");
        query.AppendLine("  ON ");
        query.AppendLine("    [NF].[IdNotaFiscal] = [NFEdest].[IdNotaFiscal]");
        query.AppendLine("  AND ");
        query.AppendLine("    [NFEdest].[IsEmit] = 0");

        return query.ToString();
    }
    #endregion
}
