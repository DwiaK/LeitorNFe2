using LeitorNFe.Application.Abstractions.Command;
using LeitorNFe.Domain.Entities.NotasFiscais;
using LeitorNFe.SharedKernel;
using System.Collections.Generic;

namespace LeitorNFe.Application.NotaFiscalFeature.Create;

public sealed record CreateMultiplasNotasFiscaisCommand(List<NotaFiscal> notasFiscais) : ICommand<bool>;