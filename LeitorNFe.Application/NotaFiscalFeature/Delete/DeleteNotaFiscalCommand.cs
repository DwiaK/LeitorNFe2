using LeitorNFe.Application.Abstractions.Command;
using LeitorNFe.Application.Abstractions.Messaging;

namespace LeitorNFe.Application.NotaFiscalFeature.Delete;

public sealed record DeleteNotaFiscalCommand(int id) : ICommand<bool>;
