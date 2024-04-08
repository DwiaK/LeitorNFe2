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

public class UpdateNotaFiscalCommandHandler : ICommandHandler<UpdateNotaFiscalCommand, bool>
{
	#region Atributos
	private IDbConnection _dbConnectionFactory;
	#endregion

	#region Construtor
	public UpdateNotaFiscalCommandHandler(IDbConnection dbConnectionFactory) =>
		_dbConnectionFactory = dbConnectionFactory;
	#endregion

	#region Handler
	public async Task<Result<bool>> Handle(UpdateNotaFiscalCommand command, CancellationToken cancellationToken)
	{
		#region Validação
		if (command is null)
			return Result.Failure<bool>(Error.NullValue);
		#endregion

		#region Conexão
		await using var sqlConnection = _dbConnectionFactory.CreateConnection();
		#endregion

		#region Query
		var nfQuery = NotaFiscalStringQuery();
		#endregion

		#region Transaction
		using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
		{
			try
			{
				// Iniciar Conexão Assíncrona
				await sqlConnection.OpenAsync();

				// Executar Edição na NotaFiscal
				await sqlConnection.ExecuteAsync(nfQuery, new { command.notaFiscal.Descricao, command.notaFiscal.IdNotaFiscal });

				transaction.Complete();

				return Result.Success<bool>(true);
			}
			catch (Exception)
			{
				transaction.Dispose();

				return Result.Failure<bool>(Error.NullValue);
			}
		}
		#endregion
	}
	#endregion

	#region Database Queries
	public string NotaFiscalStringQuery()
	{
		StringBuilder nfQuery = new StringBuilder();

		#region Query NotaFiscal
		nfQuery.AppendLine($"UPDATE")
			   .AppendLine($"    [NotaFiscal]")
			   .AppendLine($"    SET")
			   .AppendLine($"        [Descricao] = @Descricao")
			   .AppendLine($"    WHERE")
			   .AppendLine($"        [IdNotaFiscal] = @IdNotaFiscal");
		#endregion

		return nfQuery.ToString();
	}
	#endregion
}
