namespace LeitorNFe.App.Models.NotaFiscal;

public class NotaFiscalProdutosModel
{
    public string NumeroItem { get; private set; }
    public string CodigoProduto { get; private set; }
    public string Nome { get; private set; }
    public decimal QtdeComprada { get; private set; }
    public decimal ValorUnitario { get; private set; }
    public decimal ValorTotal { get; private set; }
}
