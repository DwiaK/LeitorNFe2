using LeitorNFe.Domain.Common.Enums;
using System.Collections.Generic;

namespace LeitorNFe.Domain.Entities.Enderecos;

public class Endereco
{
    #region Ctor
    public Endereco(int id, int idNf, bool isEmit, string xlgr, string num, string bairro, string mun, string uf, string cep)
    {
        IdNotaFiscalEnderecos = id;
        IdNotaFiscal = idNf;
        IsEmit = isEmit;
        xLgr = xlgr;
        nro = num;
        xBairro = bairro;
        xMun = mun;
        UF = uf;
        CEP = cep;
    }

    public Endereco()
    {
    }
    #endregion

    #region Props
    public int IdNotaFiscalEnderecos { get; set; }
    public int IdNotaFiscal { get; set; } // Foreign Key -> NotaFiscal

    public bool IsEmit { get; set; }

    public string xLgr { get; set; }
    public string nro { get; set; }
    public string xBairro { get; set; }
    public string xMun { get; set; }
    public string UF { get; set; }
    public string CEP { get; set; }
    #endregion

    #region Methods
    public static Endereco Create
        (int id, int idNf, bool isEmit, string xLgr, string nro, string xBairro, string xMun, string uf, string cep)
    {
        Endereco endereco = new Endereco(id, idNf, isEmit, xLgr, nro, xBairro, xMun, uf, cep);

        return endereco;
    }
    #endregion
}
