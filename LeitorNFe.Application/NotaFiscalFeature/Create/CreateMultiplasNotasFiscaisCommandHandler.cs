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

public class CreateMultiplasNotasFiscaisCommandHandler : ICommandHandler<CreateMultiplasNotasFiscaisCommand, bool>
{
    #region Atributos
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    #endregion

    #region Construtor
    public CreateMultiplasNotasFiscaisCommandHandler(ISqlConnectionFactory sqlConnectionFactory) =>
        _sqlConnectionFactory = sqlConnectionFactory;
    #endregion

    #region Handler
    public async Task<Result<bool>> Handle(CreateMultiplasNotasFiscaisCommand command, CancellationToken cancellationToken)
    {
        // Criar Conexão
        await using var sqlConnection = _sqlConnectionFactory
            .CreateConnection();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                await sqlConnection.OpenAsync();

                var nfQuery = NotaFiscalStringQuery();
                var nfeQuery = NotaFiscalEnderecoStringQuery();

                int linhasAfetadasEmit = 0;
                int linhasAfetadasDest = 0;

                await command.notasFiscais.ForEachAsync(async item =>
                {
                    var notaFiscalId = await sqlConnection
                        .ExecuteScalarAsync<int>(nfQuery, item);

                    if (notaFiscalId is > 0)
                    {
                        item.EnderecoEmitente.IdNotaFiscal = notaFiscalId;
                        item.EnderecoDestinatario.IdNotaFiscal = notaFiscalId;
                    }

                    linhasAfetadasEmit = await sqlConnection
                        .ExecuteAsync(nfeQuery, item.EnderecoEmitente);

                    linhasAfetadasDest = await sqlConnection
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
    #endregion

    #region Database Queries
    public string NotaFiscalStringQuery()
    {
        #region Query NotaFiscal
        StringBuilder nfQuery = new StringBuilder();

        nfQuery.AppendLine("INSERT INTO");
        nfQuery.AppendLine("    [NotaFiscal] (");
        nfQuery.AppendLine("        [nNF], ");
        nfQuery.AppendLine("        [chNFe], ");
        nfQuery.AppendLine("        [dhEmi], ");
        nfQuery.AppendLine("        [CNPJEmit], ");
        nfQuery.AppendLine("        [xNomeEmit], ");
        nfQuery.AppendLine("        [CNPJDest], ");
        nfQuery.AppendLine("        [xNomeDest], ");
        nfQuery.AppendLine("        [EmailDest]");
        nfQuery.AppendLine("    )");
        nfQuery.AppendLine("    Values (");
        nfQuery.AppendLine("        @nNF, ");
        nfQuery.AppendLine("        @chNFe, ");
        nfQuery.AppendLine("        @dhEmi, ");
        nfQuery.AppendLine("        @CNPJEmit, ");
        nfQuery.AppendLine("        @xNomeEmit, ");
        nfQuery.AppendLine("        @CNPJDest, ");
        nfQuery.AppendLine("        @xNomeDest, ");
        nfQuery.AppendLine("        @EmailDest  ");
        nfQuery.AppendLine("    );");
        nfQuery.AppendLine("SELECT SCOPE_IDENTITY()"); // Buscar o Valor Identity do que foi inserido
        #endregion

        return nfQuery.ToString();
    }

    public string NotaFiscalEnderecoStringQuery()
    {
        #region Query NotaFiscalEnderecos
        StringBuilder nfeQuery = new StringBuilder();

        nfeQuery.AppendLine("INSERT INTO");
        nfeQuery.AppendLine("    [NotaFiscalEnderecos] (");
        nfeQuery.AppendLine("        [IdNotaFiscal],");
        nfeQuery.AppendLine("        [IsEmit],");
        nfeQuery.AppendLine("        [xLgr],");
        nfeQuery.AppendLine("        [nro], ");
        nfeQuery.AppendLine("        [xBairro], ");
        nfeQuery.AppendLine("        [xMun], ");
        nfeQuery.AppendLine("        [UF], ");
        nfeQuery.AppendLine("        [CEP]");
        nfeQuery.AppendLine("    )");
        nfeQuery.AppendLine("    Values (");
        nfeQuery.AppendLine("        @IdNotaFiscal, ");
        nfeQuery.AppendLine("        @IsEmit, ");
        nfeQuery.AppendLine("        @xLgr, ");
        nfeQuery.AppendLine("        @nro, ");
        nfeQuery.AppendLine("        @xBairro, ");
        nfeQuery.AppendLine("        @xMun, ");
        nfeQuery.AppendLine("        @UF, ");
        nfeQuery.AppendLine("        @CEP ");
        nfeQuery.AppendLine("    )");
        #endregion

        return nfeQuery.ToString();
    }
    #endregion
}
