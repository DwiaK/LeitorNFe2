using LeitorNFe.Application.Abstractions.Messaging;
using LeitorNFe.Domain.Entities.NotasFiscais;

namespace LeitorNFe.Application.NotaFiscalFeature.GetById;

public sealed record GetNotaFiscalByIdQuery(int id) : IQuery<NotaFiscal>;
