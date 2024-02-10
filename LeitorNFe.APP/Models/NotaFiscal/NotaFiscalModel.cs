using System.Xml.Serialization;

namespace LeitorNFe.App.Models.NotaFiscal;

[XmlRoot("Root")]
public class NotaFiscalModel
{
    public int IdNotaFiscal { get; set; }

    #region Base
    [XmlElement("nNF")]
    public string nNF { get; set; }

    [XmlElement("chNFe")]
    public string chNFe { get; set; }

    [XmlElement("dhEmi")]
    public string dhEmi { get; set; }
    #endregion

    #region Emitente
    [XmlElement("CNPJ")]
    public string CNPJEmit { get; set; }

    [XmlElement("xNome")]
    public string xNomeEmit { get; set; }

    public NotaFiscalEnderecosModel EnderecoEmitente { get; set; }
    #endregion

    #region Destinatário
    [XmlElement("CNPJ")]
    public string CNPJDest { get; set; }

    [XmlElement("xNome")]
    public string xNomeDest { get; set; }

    [XmlElement("Email")]
    public string EmailDest { get; set; }

    public NotaFiscalEnderecosModel EnderecoDestinatario { get; set; }
    #endregion
}
