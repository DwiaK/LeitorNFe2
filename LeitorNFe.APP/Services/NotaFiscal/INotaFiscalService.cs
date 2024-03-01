using LeitorNFe.App.Models.NotaFiscal;
using LeitorNFe.App.Wrapper;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeitorNFe.App.Services.NotaFiscal;

public interface INotaFiscalService
{
    public Task<NotaFiscalModel> BuscarNotaFiscalPorId(int id);
    public Task<List<NotaFiscalModel>> ListarNotasFiscais();
    public Task<bool> ImportarNotaFiscal(NotaFiscalModel notaFiscal);
    public Task<bool> ImportarMultiplasNotasFiscais(List<NotaFiscalModel> notaFiscal);
    public Task<NotaFiscalModel> MontarNotaFiscal(IBrowserFile arquivo);
    public Task<bool> DeletarNotaFiscal(int idNotaFiscal);
    public Task<bool> EditarNotaFiscal(NotaFiscalModel notaFiscal);
}
