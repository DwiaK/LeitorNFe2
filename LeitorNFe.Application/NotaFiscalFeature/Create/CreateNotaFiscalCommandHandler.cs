﻿using Dapper;
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

internal sealed class CreateNotaFiscalCommandHandler : ICommandHandler<CreateNotaFiscalCommand, bool>
{
	private readonly IDbConnection _dbConnectionFactory;

	public CreateNotaFiscalCommandHandler(IDbConnection dbConnectionFactory) =>
		_dbConnectionFactory = dbConnectionFactory;

	public async Task<Result<bool>> Handle(CreateNotaFiscalCommand command, CancellationToken cancellationToken)
	{
		if (command is null)
			return Result.Failure<bool>(Error.NullValue);

		await using var dbConnection = _dbConnectionFactory.CreateConnection();

		var nfQuery = NotaFiscalStringQuery();
		var nfeQuery = NotaFiscalEnderecoStringQuery();

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
	}

	public string NotaFiscalStringQuery()
	{
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

		return nfQuery.ToString();
	}

	public string NotaFiscalEnderecoStringQuery()
	{
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

		return nfeQuery.ToString();
	}
}
