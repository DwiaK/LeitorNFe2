using LeitorNFe.Application.Abstractions.Command;

namespace LeitorNFe.Application.NotaFiscalFeature.Create;

public sealed record CreateNotaFiscal(int Id) : ICommand;
