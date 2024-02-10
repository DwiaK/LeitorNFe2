using LeitorNFe.Application.Abstractions.Command;
using LeitorNFe.Application.NotaFiscalFeature.GetById;
using LeitorNFe.SharedKernel;
using LeitorNFe.Domain.Entities.NotasFiscais;
using System.Threading.Tasks;
using System.Threading;

namespace LeitorNFe.Application.Abstractions.Messaging;

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}
