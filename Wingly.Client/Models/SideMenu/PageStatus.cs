using System.ComponentModel;

namespace LeitorNFe.App.Models.SideMenu;

public enum PageStatus
{
    [Description("Em Breve")] 
    EmBreve,

    [Description("Em Progresso")] 
    EmProgresso,

    [Description("Novo")] 
    Novo,

    [Description("Completo")] 
    Concluido
}