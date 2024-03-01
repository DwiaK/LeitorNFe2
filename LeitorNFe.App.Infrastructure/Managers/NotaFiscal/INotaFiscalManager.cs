using LeitorNFe.App.Models.NotaFiscal;
using LeitorNFe.App.Infrastructure.Wrapper;

namespace LeitorNFe.App.Infrastructure.Managers.NotaFiscal;

internal interface INotaFiscalManager
{
    public Task<IResult<bool>> ImportarNotaFiscal(NotaFiscalModel notaFiscal);
}
