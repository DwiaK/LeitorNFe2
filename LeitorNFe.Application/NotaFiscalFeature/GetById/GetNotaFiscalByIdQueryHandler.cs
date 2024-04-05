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
using Microsoft.Data.SqlClient;

namespace LeitorNFe.Application.NotaFiscalFeature.GetById;

public sealed class GetNotaFiscalByIdQueryHandler : IQueryHandler<GetNotaFiscalByIdQuery, NotaFiscal>
{
	#region Atributos
	private readonly IDbConnection _dbConnectionFactory;
	#endregion

	#region Construtor
	public GetNotaFiscalByIdQueryHandler(IDbConnection dbConnectionFactory)
	{
		_dbConnectionFactory = dbConnectionFactory;
	}
	#endregion

	#region Handler
	public async Task<Result<NotaFiscal>> Handle(GetNotaFiscalByIdQuery query, CancellationToken cancellationToken)
	{
		#region Validação
		if (query is null)
			return Result.Failure<NotaFiscal>(Error.NullValue);
		#endregion

		#region Conexão
		await using var dbConnection = _dbConnectionFactory.CreateConnection();
		await dbConnection.OpenAsync();
		#endregion

		#region Query
		var nfQuery = GetNotaFiscalByIdStringQuery();
		#endregion

		#region Buscas
		var notaFiscal = (await dbConnection
			.QueryAsync<NotaFiscal, Endereco, Endereco, NotaFiscal>
				(nfQuery, (notaFiscal, emitente, destinatario) =>
					{
						notaFiscal.EnderecoEmitente = emitente;
						notaFiscal.EnderecoDestinatario = destinatario;

						return notaFiscal;
					},
					new { IdNotaFiscal = query.id },
					splitOn: "IdNotaFiscalEnderecos"))
			.FirstOrDefault();
		#endregion

		#region Validações
		if (notaFiscal is null)
		{
			return Result.Failure<NotaFiscal>(Error.NullValue);
		}

		return Result.Success<NotaFiscal>(notaFiscal);
		#endregion
	}

	#endregion

	#region Database Queries
	public string GetNotaFiscalByIdStringQuery()
	{
		StringBuilder query = new StringBuilder();

		query.AppendLine($"SELECT ")
			 .AppendLine($"  [NF].*, ")
			 .AppendLine($"  [EnderecoEmitente].*, ")
			 .AppendLine($"  [EnderecoDestinatario].* ")
			 .AppendLine($"    FROM ")
			 .AppendLine($"        [NotaFiscal][NF] ")

			 .AppendLine($"    INNER JOIN ")
			 .AppendLine($"        [NotaFiscalEnderecos][EnderecoEmitente] ")
			 .AppendLine($"        ON ")
			 .AppendLine($"            [EnderecoEmitente].[IdNotaFiscal] = [NF].[IdNotaFiscal] ")
			 .AppendLine($"        AND ")
			 .AppendLine($"            [EnderecoEmitente].[IsEmit] = 1 ")

			 .AppendLine($"    INNER JOIN ")
			 .AppendLine($"        [NotaFiscalEnderecos][EnderecoDestinatario] ")
			 .AppendLine($"        ON ")
			 .AppendLine($"            [EnderecoDestinatario].[IdNotaFiscal] = [NF].[IdNotaFiscal] ")
			 .AppendLine($"        AND ")
			 .AppendLine($"            [EnderecoDestinatario].[IsEmit] = 0 ")
			 .AppendLine($"WHERE ")
			 .AppendLine($"    [NF].[IdNotaFiscal] = @IdNotaFiscal ");

		return query.ToString();
	}
	#endregion
}
