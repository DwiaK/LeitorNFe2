namespace LeitorNFe.Domain.Entities.NotasFiscais;

public interface INotaFiscalRepository
{
    void Insert(NotaFiscal notaFiscal);

    Task<NotaFiscal> GetById(int id);
}
