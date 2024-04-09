using Dapper;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.Application.Extensions;
using LeitorNFe.SharedKernel;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace LeitorNFe.Application.NotaFiscalFeature.Create;

internal sealed class CreateMultiplasNotasFiscaisCommandHandler : ICommandHandler<CreateMultiplasNotasFiscaisCommand, bool>
{
    #region Atributos
    private readonly IDbConnection _dbConnectionFactory;
    #endregion

    #region Construtor
    public CreateMultiplasNotasFiscaisCommandHandler(IDbConnection dbConnectionFactory) =>
		_dbConnectionFactory = dbConnectionFactory;
    #endregion

    #region Handler
    public async Task<Result<bool>> Handle(CreateMultiplasNotasFiscaisCommand command, CancellationToken cancellationToken)
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

		#region Variáveis
		int linhasAfetadasEmit = 0;
		int linhasAfetadasDest = 0;
		#endregion

		#region Transaction
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
