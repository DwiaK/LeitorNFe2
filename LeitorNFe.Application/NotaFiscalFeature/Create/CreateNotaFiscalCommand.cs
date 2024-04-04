using LeitorNFe.Application.Abstractions.Command;
using LeitorNFe.Domain.Entities.NotasFiscais;

namespace LeitorNFe.Application.NotaFiscalFeature.Create;

public sealed record CreateNotaFiscalCommand(NotaFiscal notaFiscal) : ICommand<bool>;