using LeitorNFe.Domain.Common;
using LeitorNFe.Domain.Entities.Enderecos;

namespace LeitorNFe.Domain.Entities.NotasFiscais;

public class NotaFiscal : EntidadeBase
{
    private NotaFiscal(
        int id, string numeroNf, string chaveNFe, string DhEmi, 
        string CNPJEmitente, string xNomeEmitente,
        string CNPJDestinatario, string xNomeDestinatario, string EmailDestinatario)
    {
        IdNotaFiscal = id;
        nNF = numeroNf;
        chNFe = chaveNFe;
        dhEmi = DhEmi;
        CNPJEmit = CNPJEmitente;
        xNomeEmit = xNomeEmitente;
        CNPJDest = CNPJDestinatario;
        xNomeDest = xNomeDestinatario;
        EmailDest = EmailDestinatario;
    }

    public NotaFiscal()
    {
    }

    public int IdNotaFiscal { get; set; }

    #region Base
    public string nNF { get; set; }
    public string chNFe { get; set; }
    public string dhEmi { get; set; }
    #endregion

    #region Emitente
    public string CNPJEmit { get; set; }
    public string xNomeEmit { get; set; }

    #region Destinatário
    public string CNPJDest { get; set; }
    public string xNomeDest { get; set; }
    public string EmailDest { get; set; }
    #endregion

    public static NotaFiscal Create(int id, string numeroNf, string chaveNFe, string DhEmi,
        string CNPJEmitente, string xNomeEmitente,
        string CNPJDestinatario, string xNomeDestinatario, string EmailDestinatario)
    {
        var notaFiscal = new NotaFiscal(id, numeroNf, chaveNFe, DhEmi,
            CNPJEmitente, xNomeEmitente,
            CNPJDestinatario, xNomeDestinatario, EmailDestinatario);

        return notaFiscal;
    }
}
