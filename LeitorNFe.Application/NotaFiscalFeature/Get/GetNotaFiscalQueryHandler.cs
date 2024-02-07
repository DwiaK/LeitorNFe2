using Dapper;
using LeitorNFe.Application.Abstractions.Command;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.Application.NotaFiscalFeature.GetById;
using LeitorNFe.Domain.Entities.NotasFiscais;
using LeitorNFe.SharedKernel;
using NotaFiscalFeature.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeitorNFe.Application.NotaFiscalFeature.Get;

public sealed record GetNotasFiscaisCommand() : ICommand;

public sealed class GetNotaFiscalQueryHandler : ICommandHandler<GetNotasFiscaisCommand>
{
    #region Atributos
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    #endregion

    #region Construtor
    public GetNotaFiscalQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    #endregion

    #region Handle
    public async Task<List<NotaFiscal>> Handle(GetNotasFiscaisCommand command, CancellationToken cancellationToken)
    {
        await using var sqlConnection = _sqlConnectionFactory
            .CreateConnection();

        var sql = (@"SELECT 
                        nNF, chNFe, dhEmi, 
                        CNPJDest, xNomeDest, EmailDest, 
                        xLgr, nro, xBairro, 
                        xMun, UF, CEP
                    FROM NotaFiscal");

        List<NotaFiscal?> listaNotasFiscais = sqlConnection
            .Query<NotaFiscal?>(sql)
            .ToList();

        if (listaNotasFiscais is null)
        {
            // Tratar nf nulo
        }

        return listaNotasFiscais;
    }

    Task<Result> ICommandHandler<GetNotasFiscaisCommand>.Handle(GetNotasFiscaisCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    #endregion
}