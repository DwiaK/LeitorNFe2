using Dapper;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.Application.Extensions;
using LeitorNFe.SharedKernel;
using Microsoft.Data.SqlClient;
using System;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace LeitorNFe.Application.NotaFiscalFeature.Create;

internal sealed class CreateMultiplasNotasFiscaisCommandHandler : ICommandHandler<CreateMultiplasNotasFiscaisCommand, bool>
{
    private readonly IDbConnection _dbConnectionFactory;

    public CreateMultiplasNotasFiscaisCommandHandler(IDbConnection dbConnectionFactory) =>
		_dbConnectionFactory = dbConnectionFactory;

    public async Task<Result<bool>> Handle(CreateMultiplasNotasFiscaisCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            return Result.Failure<bool>(Error.NullValue);

        await using var dbConnection = _dbConnectionFactory.CreateConnection();

		var nfQuery = NotaFiscalStringQuery();
		var nfeQuery = NotaFiscalEnderecoStringQuery();

		int linhasAfetadasEmit = 0;
		int linhasAfetadasDest = 0;

		using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                await dbConnection.OpenAsync();

				await command.notasFiscais.ForEachAsync(async item =>
				{
					var notaFiscalId = await dbConnection
						.ExecuteScalarAsync<int>(nfQuery, item);

					if (notaFiscalId is > 0)
					{
						item.EnderecoEmitente.IdNotaFiscal = notaFiscalId;
						item.EnderecoDestinatario.IdNotaFiscal = notaFiscalId;
					}

					linhasAfetadasEmit = await dbConnection
						.ExecuteAsync(nfeQuery, item.EnderecoEmitente);

					linhasAfetadasDest = await dbConnection
						.ExecuteAsync(nfeQuery, item.EnderecoDestinatario);

					transaction.Complete();
				});

				if (linhasAfetadasEmit is > 0 && linhasAfetadasDest is > 0)
					return Result.Success<bool>(true);
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
