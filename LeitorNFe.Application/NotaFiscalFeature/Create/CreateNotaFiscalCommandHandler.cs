using Dapper;
using LeitorNFe.Application.Abstractions.Command;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.Application.NotaFiscalFeature.GetById;
using LeitorNFe.Domain.Common.Enums;
using LeitorNFe.Domain.Entities.NotasFiscais;
using LeitorNFe.SharedKernel;
using Microsoft.Data.SqlClient;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace LeitorNFe.Application.NotaFiscalFeature.Create;

public class CreateNotaFiscalCommandHandler : ICommandHandler<CreateNotaFiscalCommand, bool>
{
	#region Atributos
	private readonly IDbConnection _dbConnectionFactory;
	#endregion

	#region Construtor
	public CreateNotaFiscalCommandHandler(IDbConnection dbConnectionFactory)
	{
		_dbConnectionFactory = dbConnectionFactory;
	}
	#endregion

	#region Handle
	public async Task<Result<bool>> Handle(CreateNotaFiscalCommand command, CancellationToken cancellationToken)
	{
		#region Validação
		if (command is null)
			return Result.Failure<bool>(Error.NullValue);
		#endregion

		#region Conexão
		await using var dbConnection = _dbConnectionFactory.CreateConnection();
		#endregion

		#region Queries
		var nfQuery = NotaFiscalStringQuery();
		var nfeQuery = NotaFiscalEnderecoStringQuery();
		#endregion

		#region Transaction
		using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
		{
			try
			{
				// Iniciar Conexão Assíncrona
				await dbConnection.OpenAsync();

				// Executar Inserção na NotaFiscal e buscar o ID
				var notaFiscalId = await dbConnection
					.ExecuteScalarAsync<int>(nfQuery, command.notaFiscal);

				// Se o ID for maior que 0
				if (notaFiscalId is > 0)
				{
					command.notaFiscal.EnderecoEmitente.IdNotaFiscal = notaFiscalId;
					command.notaFiscal.EnderecoDestinatario.IdNotaFiscal = notaFiscalId;
				}

				var linhasAfetadasEmit = await dbConnection
					.ExecuteAsync(nfeQuery, command.notaFiscal.EnderecoEmitente);

				var linhasAfetadasDest = await dbConnection
					.ExecuteAsync(nfeQuery, command.notaFiscal.EnderecoDestinatario);

				transaction.Complete();

				if (linhasAfetadasEmit is > 0 && linhasAfetadasDest is > 0)
				{
					return Result.Success<bool>(true);
				}
			}
			catch (Exception)
			{
				transaction.Dispose();

				return Result.Failure<bool>(Error.NullValue);
			}

			return Result.Failure<bool>(Error.NullValue);
		}
		#endregion
	}
	#endregion

	#region Database Queries
	public string NotaFiscalStringQuery()
	{
		#region Query NotaFiscal
		StringBuilder nfQuery = new StringBuilder();

		nfQuery.AppendLine("INSERT INTO")
			   .AppendLine("    [NotaFiscal] (")
			   .AppendLine("        [nNF], ")
			   .AppendLine("        [chNFe], ")
			   .AppendLine("        [dhEmi], ")
			   .AppendLine("        [CNPJEmit], ")
			   .AppendLine("        [xNomeEmit], ")
			   .AppendLine("        [CNPJDest], ")
			   .AppendLine("        [xNomeDest], ")
			   .AppendLine("        [EmailDest]")
			   .AppendLine("    )")
			   .AppendLine("    Values (")
			   .AppendLine("        @nNF, ")
			   .AppendLine("        @chNFe, ")
			   .AppendLine("        @dhEmi, ")
			   .AppendLine("        @CNPJEmit, ")
			   .AppendLine("        @xNomeEmit, ")
			   .AppendLine("        @CNPJDest, ")
			   .AppendLine("        @xNomeDest, ")
			   .AppendLine("        @EmailDest  ")
			   .AppendLine("    );")
			   .AppendLine("SELECT SCOPE_IDENTITY()"); // Busca o Valor Identity do que foi inserido
		#endregion

		return nfQuery.ToString();
	}

	public string NotaFiscalEnderecoStringQuery()
	{
		#region Query NotaFiscalEnderecos
		StringBuilder nfeQuery = new StringBuilder();

		nfeQuery.AppendLine("INSERT INTO")
				.AppendLine("    [NotaFiscalEnderecos] (")
				.AppendLine("        [IdNotaFiscal],")
				.AppendLine("        [IsEmit],")
				.AppendLine("        [xLgr],")
				.AppendLine("        [nro], ")
				.AppendLine("        [xBairro], ")
				.AppendLine("        [xMun], ")
				.AppendLine("        [UF], ")
				.AppendLine("        [CEP]")
				.AppendLine("    )")
				.AppendLine("    Values (")
				.AppendLine("        @IdNotaFiscal, ")
				.AppendLine("        @IsEmit, ")
				.AppendLine("        @xLgr, ")
				.AppendLine("        @nro, ")
				.AppendLine("        @xBairro, ")
				.AppendLine("        @xMun, ")
				.AppendLine("        @UF, ")
				.AppendLine("        @CEP ")
				.AppendLine("    )");
		#endregion

		return nfeQuery.ToString();
	}
	#endregion
}
