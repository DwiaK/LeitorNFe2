using Dapper;
using LeitorNFe.Application.Abstractions.Data;
using System.Threading;
using System.Threading.Tasks;

namespace LeitorNFe.Application.NotaFiscalFeature.Get;

public class GetNotaFiscalQueryHandler
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
    public async Task<NotaFiscalResponse> Handle(GetNotaFiscalQuery query, CancellationToken cancellationToken)
    {
        await using var sqlConnection = _sqlConnectionFactory
            .CreateConnection();

        NotaFiscalResponse? notaFiscalResponse = await 
            sqlConnection.QueryFirstOrDefaultAsync<NotaFiscalResponse>(
                @"SELECT 
                    nNF, chNFe, dhEmi, 
                    CNPJDest, xNomeDest, EmailDest, 
                    xLgr, nro, xBairro, 
                    xMun, UF, CEP
                 FROM NotaFiscal
                 WHERE Id = @Id",
                new
                {
                    Id = query.Id
                });

        if (notaFiscalResponse is null)
        {
            // Tratar nf nulo
        }

        return notaFiscalResponse;
    }
    #endregion
}
