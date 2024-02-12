using LeitorNFe.Application.Abstractions.Command;

namespace LeitorNFe.Application.NotaFiscalFeature.Delete;

public sealed record DeleteNotaFiscalQuery(int Id) : ICommand;