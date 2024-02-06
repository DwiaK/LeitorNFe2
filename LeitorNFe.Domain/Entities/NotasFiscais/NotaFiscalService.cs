using LeitorNFe.SharedKernel;

namespace LeitorNFe.Domain.Entities.NotasFiscais;

public sealed class NotaFiscalService
{
    private readonly INotaFiscalRepository _notaFiscalRepository;

    public NotaFiscalService(INotaFiscalRepository notaFiscalRepository)
    {
        _notaFiscalRepository = notaFiscalRepository;
    }

    public async Task<Result> BuscarNotaFiscal(NotaFiscal notaFiscal, CancellationToken cancellationToken)
    {
        // Definir Regra de Negócio

        if (notaFiscal.Id is > 0)
        {
            throw new Exception("O Id deve ser maior que Zero.");
        }

        if (notaFiscal.xNomeEmit is null)
        {
            throw new Exception("O nome do emitente não pode ser nulo.");
        }

        //var notaFiscal = NotaFiscal.Create();

        await _notaFiscalRepository.GetById(notaFiscal.Id);

        return Result.Success();
    }
}
