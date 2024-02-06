namespace LeitorNFe.Application.NotaFiscalFeature;

public sealed record NotaFiscalResponse
{
    #region Base
    public int IdNotaFiscal { get; init; }

    public string nNF { get; init; }
    public string chNFe { get; init; }
    public string dhEmi { get; init; }
    #endregion

    #region Dados Emitente/Destinatario
    public string CNPJEmit { get; private set; }
    public string xNomeEmit { get; private set; }

    public string CNPJDest { get; init; }
    public string xNomeDest { get; init; }
    public string EmailDest { get; init; }
    #endregion

    #region Dados Endereço Emitente/Destinatario
    public string xLgr { get; init; }
    public string nro { get; init; }
    public string xBairro { get; init; }
    public string xMun { get; init; }
    public string UF { get; init; }
    public string CEP { get; init; }
    #endregion
}