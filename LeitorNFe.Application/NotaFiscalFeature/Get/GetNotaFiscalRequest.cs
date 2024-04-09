using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.Domain.Entities.NotasFiscais;
using System.Collections.Generic;

namespace LeitorNFe.Application.NotaFiscalFeature.Get;

public sealed record GetNotaFiscalRequest() : IQuery<List<NotaFiscal>>;
