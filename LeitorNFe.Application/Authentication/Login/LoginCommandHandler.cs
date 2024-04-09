using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.SharedKernel;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace LeitorNFe.Application.Authentication.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, string>
{
	#region Atributos
	private readonly IDbConnection _dbConnectionFactory;
    #endregion

    #region Construtor
    public LoginCommandHandler(IDbConnection dbConnectionFactory)
    {
		_dbConnectionFactory = dbConnectionFactory;
	}
    #endregion

    #region Handler
    public async Task<Result<string>> Handle(LoginCommand command, CancellationToken cancellationToken)
	{
		#region Validação
		if (command is null)
			return Result.Failure<string>(Error.NullValue);
		#endregion

		#region Conexão
		await using var dbConnection = _dbConnectionFactory.CreateConnection();
		#endregion

		try
		{
			await dbConnection.OpenAsync();

			return Result.Success<string>(string.Empty);
		}
		catch (Exception)
		{
			return Result.Failure<string>(Error.NullValue);
		}

		return Result.Failure<string>(Error.NullValue);
	}
	#endregion
}
