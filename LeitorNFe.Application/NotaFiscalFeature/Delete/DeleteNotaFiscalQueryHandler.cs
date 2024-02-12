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

public sealed record DeleteNotaFiscalCommand(int id) : IQuery<int>;

public sealed class DeleteNotaFiscalQueryHandler : IQueryHandler<DeleteNotaFiscalCommand, int>
{
    #region Atributos
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    #endregion

    #region Construtor
    public DeleteNotaFiscalQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    #endregion

    #region Handle
    public async Task<Result<int>> Handle(DeleteNotaFiscalCommand query, CancellationToken cancellationToken)
    {
        // Criar Conexão
        await using var sqlConnection = _sqlConnectionFactory
            .CreateConnection();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                return 1;
            }
            catch (Exception)
            {
                throw;
            }
        } 
    }

    #endregion

    #region Database Queries
    private string NotaFiscalStringQuery()
    {
        #region Query NotaFiscal
        StringBuilder sb = new StringBuilder();

        return sb.ToString();
        #endregion
    }

    private string NotaFiscalEnderecoStringQuery()
    {
        #region Query NotaFiscalEndereco
        StringBuilder sb = new StringBuilder();

        return sb.ToString();
        #endregion
    }
    #endregion
}