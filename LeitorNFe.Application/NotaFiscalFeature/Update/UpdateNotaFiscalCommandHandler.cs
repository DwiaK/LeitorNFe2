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
	private ISqlConnectionFactory _sqlConnectionFactory;
	#endregion

	#region Construtor
	public UpdateNotaFiscalCommandHandler(ISqlConnectionFactory sqlConnectionFactory) =>
		_sqlConnectionFactory = sqlConnectionFactory;
	#endregion

	#region Handle
	public async Task<Result<bool>> Handle(UpdateNotaFiscalCommand command, CancellationToken cancellationToken)
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
				await sqlConnection.ExecuteAsync(nfQuery, new { command.notaFiscal.Descricao, command.notaFiscal.IdNotaFiscal });

				transaction.Complete();

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
