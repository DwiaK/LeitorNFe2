using LeitorNFe.App.Models.NotaFiscal;
using System.Threading.Tasks;

namespace LeitorNFe.App.Services.NotaFiscal;

public interface INotaFiscalService
{
    public Task<NotaFiscalModel> GetNotaFiscalById(int id);
    public void ImportNotaFiscal(NotaFiscalModel notaFiscal); // Task<Result>
}
