using Dapper;
using LeitorNFe.Application.Abstractions.Command;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.Application.NotaFiscalFeature.GetById;
using LeitorNFe.SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace LeitorNFe.Application.NotaFiscal.Create;

public sealed record CreateNotaFiscalCommand(LeitorNFe.Domain.Entities.NotasFiscais.NotaFiscal notaFiscal) : ICommand;

public class CreateNotaFiscalQueryHandler : ICommandHandler<CreateNotaFiscalCommand>
{
    #region Atributos
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    #endregion

    #region Construtor
    public CreateNotaFiscalQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    #endregion

    #region Handle
    public async Task<Result> Handle(CreateNotaFiscalCommand command, CancellationToken cancellationToken)
    {
        await using var sqlConnection = _sqlConnectionFactory
                    .CreateConnection();

        const string sql =
            @"INSERT INTO
                    NotaFiscal (
                        nNF, chNFe, dhEmi, 
                        CNPJEmit, xNomeEmit, CNPJDest, 
                        xNomeDest, EmailDest, xLgr, 
                        nro, xBairro, xMun, UF
                    )
                    Values (
                        @nNF, @chNFe, @dhEmi, 
                        @CNPJEmit, @xNomeEmit, @CNPJDest, 
                        @xNomeDest, @EmailDest, 'test', 
                        'test', 'test', 'test', 'test'       
                    )";

        var linhasAfetadas = await sqlConnection.ExecuteAsync(sql, command.notaFiscal);

        if (linhasAfetadas is > 0)
            return Result.Success(true);
        else
            return Result.Failure(Error.None);
    }
    #endregion
}
