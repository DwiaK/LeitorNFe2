using System.ComponentModel;

namespace LeitorNFe.Domain.Common.Enums;

public enum TipoDestEmit
{
    [Description("Emitente")]
    Emitente,

    [Description("Destinatário")]
    Destinatario,
}
