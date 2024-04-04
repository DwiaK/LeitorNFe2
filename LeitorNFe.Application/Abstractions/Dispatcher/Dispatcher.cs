using Azure;
using LeitorNFe.Application.Abstractions.Command;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.SharedKernel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LeitorNFe.Application.Abstractions.Dispatcher;

public class Dispatcher : IDispatcher
{
	private readonly IServiceProvider _serviceProvider;

	public Dispatcher(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public async Task Send<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand
	{
		var handler = _serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) as ICommandHandler<TCommand>;

		if (handler == null)
			throw new InvalidOperationException($"Handler not found for command type '{typeof(TCommand)}'.");

		await handler.Handle(command, cancellationToken);
	}

	public async Task<Result<TResponse>> Query<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery<TResponse>
	{
		var handler = _serviceProvider.GetService(typeof(IQueryHandler<TQuery, TResponse>)) as IQueryHandler<TQuery, TResponse>;
		return await handler?.Handle(query, cancellationToken);
	}
}