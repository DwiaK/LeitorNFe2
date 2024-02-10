using System.Xml.Serialization;

namespace LeitorNFe.App.Models.NotaFiscal;

[XmlRoot("Root")]
public class NotaFiscalEnderecosModel
{
    #region Endereço
    public bool IsEmit { get; set; }

    [XmlElement("xLgr")]
    public string xLgr { get; set; }

    [XmlElement("nro")]
    public string nro { get; set; }

    [XmlElement("xBairro")]
    public string xBairro { get; set; }

    [XmlElement("xMun")]
    public string xMun { get; set; }

    [XmlElement("UF")]
    public string UF { get; set; }

    [XmlElement("CEP")]
    public string CEP { get; set; }

    #endregion
}
