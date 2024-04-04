using LeitorNFe.Application.Abstractions.Command;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace LeitorNFe.Application.Abstractions.Dispatcher;

public interface IDispatcher
{
	#region Command
	Task Send<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand;
	Task<Result<TResponse>> Send<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand<TResponse>;
	#endregion

	#region Query
	Task<Result<TResponse>> Query<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResponse>;
	#endregion
}