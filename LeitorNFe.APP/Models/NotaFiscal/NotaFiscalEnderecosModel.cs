namespace LeitorNFe.App.Models.NotaFiscal;

public class NotaFiscalEnderecosModel
{
    #region Endereço
    public bool IsEmit { get; set; }
    public string xLgr { get; set; }
    public string nro { get; set; }
    public string xBairro { get; set; }
    public string xMun { get; set; }
    public string UF { get; set; }
    public string CEP { get; set; }
    #endregion
}
