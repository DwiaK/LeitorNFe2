using LeitorNFe.SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace LeitorNFe.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}
