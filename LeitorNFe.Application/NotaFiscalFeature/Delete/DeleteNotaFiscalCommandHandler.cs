using Dapper;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.Domain.Entities.NotasFiscais;
using LeitorNFe.SharedKernel;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace LeitorNFe.Application.NotaFiscalFeature.Delete;

internal sealed class DeleteNotaFiscalCommandHandler : ICommandHandler<DeleteNotaFiscalCommand, bool>
{
    private readonly IDbConnection _dbConnectionFactory;

    public DeleteNotaFiscalCommandHandler(IDbConnection dbConnectionFactory) =>
        _dbConnectionFactory = dbConnectionFactory;

    public async Task<Result<bool>> Handle(DeleteNotaFiscalCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            return Result.Failure<bool>(Error.NullValue);

		await using var dbConnection = _dbConnectionFactory.CreateConnection();

		var nfQuery = DeleteNotaFiscalStringQuery();
		var nfeQuery = DeleteNotaFiscalEnderecoStringQuery();

		using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                // Iniciar Conexão Assíncrona
                await dbConnection.OpenAsync();

                var linhasAfetadasNFE = await dbConnection
					.ExecuteAsync(nfeQuery, new { IdNotaFiscal = command.id });

                var linhasAfetadasNF = await dbConnection
					.ExecuteAsync(nfQuery, new { IdNotaFiscal = command.id });

                if (linhasAfetadasNF is not > 0 && linhasAfetadasNFE is not > 0)
                {
                    return Result.Failure<bool>(Error.NullValue);
                }

                transaction.Complete();

                return Result.Success<bool>(true);
            }
            catch (Exception)
            {
                transaction.Dispose();

                return Result.Failure<bool>(Error.NullValue);
            }
        }
    }

    private string DeleteNotaFiscalStringQuery()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"DELETE FROM")
          .AppendLine($"    [NotaFiscal]")
          .AppendLine($"WHERE")
          .AppendLine($"    [IdNotaFiscal] = @IdNotaFiscal");

        return sb.ToString();
    }

    private string DeleteNotaFiscalEnderecoStringQuery()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"DELETE FROM")
          .AppendLine($"    [NotaFiscalEnderecos]")
          .AppendLine($"WHERE")
          .AppendLine($"    [IdNotaFiscal] = @IdNotaFiscal");

        return sb.ToString();
    }
}
