using LeitorNFe.App.Infrastructure.Wrapper;
using LeitorNFe.App.Models.NotaFiscal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeitorNFe.App.Infrastructure.Managers.NotaFiscal;

public class NotaFiscalManager : INotaFiscalManager
{
    public Task<IResult<bool>> ImportarNotaFiscal(NotaFiscalModel notaFiscal)
    {
        throw new NotImplementedException();
    }
}
