using LeitorNFe.Domain.Common;

namespace LeitorNFe.Domain.Entities.Produtos;

public class Produto : EntidadeBase
{
    public int Id { get; set; }
    public string NumeroItem { get; private set; }
    public string CodigoProduto { get; private set; }
    public string Nome { get; private set; }
    public decimal QtdeComprada { get; private set; }
    public decimal ValorUnitario { get; private set; }
    public decimal ValorTotal { get; private set; }
}
