using Dapper;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.Domain.Entities.Enderecos;
using LeitorNFe.Domain.Entities.NotasFiscais;
using LeitorNFe.SharedKernel;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace LeitorNFe.Application.NotaFiscalFeature.Update;

public class UpdateNotaFiscalCommandHandler : ICommandHandler<UpdateNotaFiscalCommand>
{
	#region Atributos
	private ISqlConnectionFactory _sqlConnectionFactory;
    #endregion

    #region Construtor
    public UpdateNotaFiscalCommandHandler(ISqlConnectionFactory sqlConnectionFactory) => 
		_sqlConnectionFactory = sqlConnectionFactory;
    #endregion

    #region Handle
    public async Task<Result> Handle(UpdateNotaFiscalCommand command, CancellationToken cancellationToken)
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
				var nfQuery = NotaFiscalStringQuery();

				// Executar Edição na NotaFiscal
				//var notaFiscalAlterada = (await sqlConnection
				//	.QueryAsync<NotaFiscal>(nfQuery, (nota) => 
				//	{
				//	}, 
				//	splitOn: ""))
				//	.ToList();

			//	var notasFiscais = (await sqlConnection.QueryAsync<NotaFiscal, Endereco, Endereco, NotaFiscal>(
			//	nfQuery,
			//	(notaFiscal, enderecoEmitente, enderecoDestinatario) =>
			//	{
			//		notaFiscal.EnderecoEmitente = enderecoEmitente;
			//		notaFiscal.EnderecoDestinatario = enderecoDestinatario;
			//		return notaFiscal;
			//	},
			//	splitOn: "IdNotaFiscalEnderecos, IdNotaFiscalEnderecos"
			//)).ToList();



				transaction.Complete();
			}
			catch (Exception)
			{
				transaction.Dispose();

				return Result.Failure(Error.NullValue);
			}

			return Result.Success(true);
		}
	}
	#endregion

	#region Database Queries
	public string NotaFiscalStringQuery()
	{
		#region Query NotaFiscal
		StringBuilder nfQuery = new StringBuilder();

		nfQuery.AppendLine($"UPDATE");
		nfQuery.AppendLine($"    [NotaFiscal]");
		nfQuery.AppendLine($"    SET");
		nfQuery.AppendLine($"        [Descricao] = @Descricao");
		nfQuery.AppendLine($"    WHERE");
		nfQuery.AppendLine($"        [IdNotaFiscal] = @IdNotaFiscal");
		#endregion

		return nfQuery.ToString();
	}
	#endregion
}
