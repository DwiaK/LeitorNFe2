using Dapper;
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



public class CreateNotaFiscalCommandHandler : ICommandHandler<CreateNotaFiscalCommand>
{
    #region Atributos
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    #endregion

    #region Construtor
    public CreateNotaFiscalCommandHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    #endregion

    #region Handle
    public async Task<Result> Handle(CreateNotaFiscalCommand command, CancellationToken cancellationToken)
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

                // Verificar se a nota Já existe no banco
                //var notaExiste = await VerificaNotaExistente(sqlConnection, command);

                //if (notaExiste is true)
                //{
                //    return Result.Failure(Error.NullValue);
                //}

                // Buscar queries
                var nfQuery = NotaFiscalStringQuery();
                var nfeQuery = NotaFiscalEnderecoStringQuery();

                // Executar Inserção na NotaFiscal e buscar o ID
                var notaFiscalId = await sqlConnection
                    .ExecuteScalarAsync<int>(nfQuery, command.notaFiscal);

                // Se o ID for maior que 0
                if (notaFiscalId is > 0)
                {
                    command.notaFiscal.EnderecoEmitente.IdNotaFiscal = notaFiscalId;
                    command.notaFiscal.EnderecoDestinatario.IdNotaFiscal = notaFiscalId;
                }

                var linhasAfetadasEmit = await sqlConnection
                    .ExecuteAsync(nfeQuery, command.notaFiscal.EnderecoEmitente);

                var linhasAfetadasDest = await sqlConnection
                    .ExecuteAsync(nfeQuery, command.notaFiscal.EnderecoDestinatario);

                transaction.Complete();

                if (linhasAfetadasEmit is > 0 && linhasAfetadasDest is > 0)
                {
                    return Result.Success(true);
                }
            }
            catch (Exception)
            {
                transaction.Dispose();

                return Result.Failure(Error.NullValue);
            }

            return Result.Success(true);
        }
    }

    //public async Task<bool> VerificaNotaExistente(SqlConnection sqlConnection, CreateNotaFiscalCommand command)
    //{
    //    var nfQuery = NotaDuplicadaStringQuery();

    //    var notaFiscal = await sqlConnection
    //        .QueryFirstOrDefaultAsync<NotaFiscal>
    //        (nfQuery, new
    //        {
    //            NumeroNotaFiscal = command.notaFiscal.nNF
    //        });

    //    // Se a lista de notas fiscais for nula
    //    if (notaFiscal is null)
    //    {
    //        return false;
    //    }

    //    return true;
    //}
    #endregion

    #region Database Queries
    public string NotaDuplicadaStringQuery()
    {
        #region Query NotaFiscal
        StringBuilder query = new StringBuilder();

        query.AppendLine("SELECT ");
        query.AppendLine("  [NF].[nNF] ");
        query.AppendLine("FROM ");
        query.AppendLine("  [NotaFiscal][NF] ");

        query.AppendLine("WHERE");
        query.AppendLine(@"  [NF].[nNF] = @NumeroNotaFiscal");

        return query.ToString();
        #endregion
    }

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
