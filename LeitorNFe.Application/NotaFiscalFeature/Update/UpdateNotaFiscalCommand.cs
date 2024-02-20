using LeitorNFe.Application.Abstractions.Command;
using LeitorNFe.Domain.Entities.NotasFiscais;

namespace LeitorNFe.Application.NotaFiscalFeature.Update;

public sealed record UpdateNotaFiscalCommand(NotaFiscal notaFiscal) : ICommand;