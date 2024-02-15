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



public sealed class DeleteNotaFiscalCommandHandler : ICommandHandler<DeleteNotaFiscalCommand, bool>
{
    #region Atributos
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    #endregion

    #region Construtor
    public DeleteNotaFiscalCommandHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    #endregion

    #region Handle
    public async Task<Result<bool>> Handle(DeleteNotaFiscalCommand query, CancellationToken cancellationToken)
    {
        // Criar Conexão
        await using var sqlConnection = _sqlConnectionFactory
            .CreateConnection();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                // Iniciar Conexão Assíncrona
                await sqlConnection.OpenAsync();

                // Buscar queries
                var nfQuery = DeleteNotaFiscalStringQuery();
                var nfeQuery = DeleteNotaFiscalEnderecoStringQuery();

                var linhasAfetadasNFE = await sqlConnection
                    .ExecuteAsync(nfeQuery, new { IdNotaFiscal = query.id });

                var linhasAfetadasNF = await sqlConnection
                    .ExecuteAsync(nfQuery, new { IdNotaFiscal = query.id });

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

    #endregion

    #region Database Queries
    private string DeleteNotaFiscalStringQuery()
    {
        #region Query NotaFiscal
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"DELETE FROM");
        sb.AppendLine($"    [NotaFiscal]");
        sb.AppendLine($"WHERE");
        sb.AppendLine($"    [IdNotaFiscal] = @IdNotaFiscal");

        return sb.ToString();
        #endregion
    }

    private string DeleteNotaFiscalEnderecoStringQuery()
    {
        #region Query NotaFiscalEndereco
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"DELETE FROM");
        sb.AppendLine($"    [NotaFiscalEnderecos]");
        sb.AppendLine($"WHERE");
        sb.AppendLine($"    [IdNotaFiscal] = @IdNotaFiscal");

        return sb.ToString();
        #endregion
    }
    #endregion
}
