using Microsoft.Extensions.FileProviders;
using MudBlazor;
using System.ComponentModel;

namespace LeitorNFe.App.Styles;

public class Temas
{
    #region Tema LeitorNFe
    private readonly Palette LeitorNFeTema = new()
    {
        Black = "#2f2f36",
        Background = "#121214",
        BackgroundGrey = "#2f2f36",
        Surface = "#17171a",
        DrawerBackground = "rgb(18, 18, 20)",
        DrawerText = "rgba(255,255,255, 0.50)",
        DrawerIcon = "rgba(255,255,255, 0.50)",
        AppbarBackground = "#2f2f36",
        AppbarText = "rgba(255,255,255, 0.70)",
        TextPrimary = "rgba(255,255,255, 0.70)",
        TextSecondary = "rgba(255,255,255, 0.50)",
        ActionDefault = "#adadb1",
        ActionDisabled = "rgba(255,255,255, 0.26)",
        ActionDisabledBackground = "rgba(255,255,255, 0.12)",
        Divider = "rgba(255,255,255, 0.12)",
        DividerLight = "rgba(255,255,255, 0.06)",
        TableLines = "rgba(255,255,255, 0.12)",
        LinesDefault = "rgba(255,255,255, 0.12)",
        LinesInputs = "rgba(255,255,255, 0.3)",
        TextDisabled = "rgba(255,255,255, 0.2)"
    };
    #endregion

    public Palette GetTema(Tema temaSelecionado) =>
        temaSelecionado switch
        {
            Tema.LeitorNFe => LeitorNFeTema,
            _ => LeitorNFeTema
        };
}

public enum Tema
{
    [Description("Wingly")]
    Wingly,

    [Description("LeitorNFe")]
    LeitorNFe
}