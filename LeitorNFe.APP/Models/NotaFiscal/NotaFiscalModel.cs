namespace LeitorNFe.App.Models.NotaFiscal;

public class NotaFiscalModel
{
    public int IdNotaFiscal { get; set; }

    #region Base
    public string nNF { get; set; }
    public string chNFe { get; set; }
    public string dhEmi { get; set; }
    #endregion

    #region Emitente
    public string CNPJEmit { get; set; }
    public string xNomeEmit { get; set; }
    public NotaFiscalEnderecosModel EnderecoEmitente { get; set; }
    #endregion

    #region Destinatário
    public string CNPJDest { get; set; }
    public string xNomeDest { get; set; }
    public string EmailDest { get; set; }
    public NotaFiscalEnderecosModel EnderecoDestinatario { get; set; }
    #endregion
}