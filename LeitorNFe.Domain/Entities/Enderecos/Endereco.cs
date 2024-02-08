namespace LeitorNFe.Domain.Entities.Enderecos;

public class Endereco
{
    #region Ctor
    public Endereco(string xlogr, string num, string bairro, string mun, string uf, string cep)
    {
        xLgr = xlogr;
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
    public string xLgr { get; set; }
    public string nro { get; set; }
    public string xBairro { get; set; }
    public string xMun { get; set; }
    public string UF { get; set; }
    public string CEP { get; set; }
    #endregion

    #region Methods
    public static Endereco Create
        (string xLgr, string nro, string xBairro, string xMun, string uf, string cep)
    {
        Endereco endereco = new Endereco(xLgr, nro, xBairro, xMun, uf, cep);

        return endereco;
    }
    #endregion
}