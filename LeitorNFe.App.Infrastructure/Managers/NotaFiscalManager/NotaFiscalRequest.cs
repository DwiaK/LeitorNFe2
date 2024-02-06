using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeitorNFe.App.Infrastructure.Managers.NotaFiscalManager;

public class NotaFiscalRequest
{
    #region Nota Fiscal
    public string IdNotaFiscal { get; private set; }
    public string nNF { get; private set; }
    public string chNFe { get; private set; }
    public string dhEmi { get; private set; }
    public string CNPJEmit { get; private set; }
    public string xNomeEmit { get; private set; }
    public string CNPJDest { get; private set; }
    public string xNomeDest { get; private set; }
    public string EmailDest { get; private set; }
    public string xLgr { get; private set; }
    public string nro { get; private set; }
    public string xBairro { get; private set; }
    public string xMun { get; private set; }
    public string UF { get; private set; }
    public string CEP { get; private set; }
    #endregion
}
