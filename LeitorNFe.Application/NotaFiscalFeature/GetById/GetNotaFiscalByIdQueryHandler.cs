using Dapper;
using LeitorNFe.SharedKernel;
using LeitorNFe.Domain.Entities.NotasFiscais;
using LeitorNFe.Application.Abstractions.Command;
using LeitorNFe.Application.Abstractions.Data;
using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.Application.NotaFiscalFeature.GetById;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace NotaFiscalFeature.GetById;

public sealed record GetNotaFiscalByIdCommand(int id) : ICommand;

public sealed class GetNotaFiscalByIdQueryHandler : ICommandHandler<GetNotaFiscalByIdCommand>
{
    #region Atributos
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    #endregion

    #region Repositórios/Serviços/UnitOfWork
    //private readonly INotaFiscalRepository _notaFiscalRepository;
    //private readonly NotaFiscalService _notaFiscalService;
    //private readonly IUnitOfWork _unitOfWork;
    #endregion

    #region Construtor
    //public GetNotaFiscalByIdQueryHandler(
    //    ISqlConnectionFactory sqlConnectionFactory, 
    //    INotaFiscalRepository notaFiscalRepository,
    //    NotaFiscalService notaFiscalService,
    //    IUnitOfWork unitOfWork)
    //{
    //    _sqlConnectionFactory = sqlConnectionFactory;
    //    _notaFiscalRepository = notaFiscalRepository;
    //    _notaFiscalService = notaFiscalService;
    //    _unitOfWork = unitOfWork;
    //}

    public GetNotaFiscalByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    #endregion

    #region Handler
    public async Task<NotaFiscal> Handle(GetNotaFiscalByIdQuery query, CancellationToken cancellationToken)
    {
        await using var sqlConnection = _sqlConnectionFactory
            .CreateConnection();

        NotaFiscal? notaFiscalResponse = await
            sqlConnection.QueryFirstOrDefaultAsync<NotaFiscal>(
                @"SELECT *
                  FROM [NotaFiscal]
                  WHERE IdNotaFiscal = @Id",
                new
                {
                    query.Id
                });

        if (notaFiscalResponse is null)
        {
            // Tratar nf nulo
        }

        return notaFiscalResponse;
    }

    public async Task<Result<NotaFiscal>> Handle(GetNotaFiscalByIdCommand command, CancellationToken cancellationToken)
    {
        await using var sqlConnection = _sqlConnectionFactory
            .CreateConnection();

        NotaFiscal? notaFiscalResponse = await
            sqlConnection.QueryFirstOrDefaultAsync<NotaFiscal>(
                @"SELECT *
                  FROM [NotaFiscal]
                  WHERE IdNotaFiscal = @Id",
                new
                {
                    command.id
                });

        if (notaFiscalResponse is null)
        {
            // Tratar nf nulo
            Result.Failure(Error.NullValue);
        }

        return notaFiscalResponse;
    }

    Task<Result> ICommandHandler<GetNotaFiscalByIdCommand>.Handle(GetNotaFiscalByIdCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // Teste
    //public async Task<Result> Handle(GetNotaFiscalByIdCommand command, CancellationToken cancellationToken)
    //{
    //    LeitorNFe.Domain.Entities.NotasFiscais.NotaFiscal? notaFiscal = await _notaFiscalRepository.GetByIdAsync(command.id, cancellationToken);

    //    if (notaFiscal is null)
    //        return NotaFiscalErrors.NotFound(command.id);

    //    Result result = await _notaFiscalService.BuscarNotaFiscal(notaFiscal, cancellationToken);

    //    if (result.IsFailure)
    //        return Result.Failure(result.Error);

    //    await _unitOfWork.SaveChangesAsync(cancellationToken);

    //    return Result.Success();
    //}
    #endregion
}

public interface INotaFiscalRepository
{
    Task<NotaFiscal?> GetByIdAsync(int id, CancellationToken cancellationToken);
}