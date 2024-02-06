using LeitorNFe.Application.Abstractions.Command;

namespace LeitorNFe.Application.NotaFiscalFeature.GetById;

public sealed record GetNotaFiscalByIdQuery(int Id) : ICommand;
