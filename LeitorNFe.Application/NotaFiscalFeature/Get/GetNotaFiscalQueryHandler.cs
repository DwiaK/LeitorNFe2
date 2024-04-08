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
    private readonly IDbConnection _dbConnectionFactory;
    #endregion

    #region Construtor
    public GetNotaFiscalQueryHandler(IDbConnection dbConnectionFactory)
    {
		_dbConnectionFactory = dbConnectionFactory;
    }
    #endregion

    #region Handler
    public async Task<Result<List<NotaFiscal>>> Handle(GetNotaFiscalQuery query, CancellationToken cancellationToken)
    {
        #region Validação
        if (query is null)
            return Result.Failure<List<NotaFiscal>>(Error.NullValue);
		#endregion

		#region Conexão
		await using var sqlConnection = _dbConnectionFactory.CreateConnection();
		await sqlConnection.OpenAsync();
		#endregion

		#region Query
		var nfQuery = GetNotasFiscaisStringQuery();
		#endregion

		#region Buscas Database
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
		#endregion

		#region Validações
		if (notasFiscais.IsNullOrEmpty())
			return Result.Failure<List<NotaFiscal>>(Error.NullValue);

		return Result.Success<List<NotaFiscal>>(notasFiscais);
		#endregion
    }
    #endregion

    #region Database Queries
    public string GetNotasFiscaisStringQuery()
    {
        StringBuilder query = new StringBuilder();

		query.AppendLine("SELECT ")
			 .AppendLine("  [NF].*, ")
			 .AppendLine("  [NFEemit].*, ")
			 .AppendLine("  [NFEdest].* ")
			 .AppendLine("FROM ")
			 .AppendLine("  [NotaFiscal][NF] ")

			 .AppendLine("CROSS APPLY (")
			 .AppendLine("    SELECT *")
			 .AppendLine("    FROM")
			 .AppendLine("        [NotaFiscalEnderecos][NFEemit]")
			 .AppendLine("    WHERE")
			 .AppendLine("        [NF].[IdNotaFiscal] = [NFEemit].[IdNotaFiscal]")
			 .AppendLine("    AND ")
			 .AppendLine("        [NFEemit].[IsEmit] = 1")
			 .AppendLine(") [NFEemit]")

			 .AppendLine("CROSS APPLY (")
			 .AppendLine("    SELECT *")
			 .AppendLine("    FROM")
			 .AppendLine("        [NotaFiscalEnderecos][NFEdest]")
			 .AppendLine("    WHERE")
			 .AppendLine("        [NF].[IdNotaFiscal] = [NFEdest].[IdNotaFiscal]")
			 .AppendLine("    AND ")
			 .AppendLine("        [NFEdest].[IsEmit] = 0")
			 .AppendLine(") [NFEdest]");

		return query.ToString();
    }
    #endregion
}
