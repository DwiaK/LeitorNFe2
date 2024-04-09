using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.SharedKernel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LeitorNFe.Application.Authentication.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, Guid>
{
    #region Atributos
    private readonly IDbConnection _dbConnectionFactory;
    #endregion

    #region Construtor
    public RegisterCommandHandler(IDbConnection dbConnectionFactory)
    {
		_dbConnectionFactory = dbConnectionFactory;
	}
    #endregion

    #region Handler
    public Task<Result<Guid>> Handle(RegisterCommand command, CancellationToken cancellationToken)
	{
		throw new System.NotImplementedException();
	}
	#endregion
}
