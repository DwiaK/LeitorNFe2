using LeitorNFe.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeitorNFe.Application.NotaFiscalFeature;

public static class NotaFiscalErrors
{
    public static Error NotFound(int nfId) =>
        new("NotaFiscal.NotFound", $"A Nota fiscal com o ID = '{nfId}' não foi encontrada.");
}


