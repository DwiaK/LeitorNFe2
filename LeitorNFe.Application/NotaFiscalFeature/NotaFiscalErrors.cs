using LeitorNFe.SharedKernel;

namespace LeitorNFe.Application.NotaFiscalFeature;

public static class NotaFiscalErrors
{
    public static Error NotFound(int nfId) =>
        new("NotaFiscal.NotFound", $"A Nota fiscal com o ID = '{nfId}' não foi encontrada.");
}
