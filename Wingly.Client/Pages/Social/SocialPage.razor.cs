using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Collections.Generic;
using System.ComponentModel;

namespace LeitorNFe.App.Pages.Social;

public partial class SocialPage
{
    #region Variáveis
    [Inject]
    private IJSRuntime JSRunTime { get; set; }
    #endregion

    #region Breadcrumbs
    private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Menu", href: "/"),
        new BreadcrumbItem("Social", href: null, disabled: true)
    };
    #endregion

    #region Dados
    private readonly List<Social> listaRedesSociais = new()
    {
        #region GitHub
        new Social
        { 
            Image = "https://kinsta.com/wp-content/uploads/2018/04/what-is-github-1-1.png", 
            Title = "Github",
            Text = "Gustavo Moeska - (DwiaK)",
            Link = "https://github.com/DwiaK",
            TipoRedeSocial = TipoRedeSocial.Github
        },
        #endregion

        #region LinkedIn
        new Social
        { 
            Image = "https://i0.wp.com/www.alphr.com/wp-content/uploads/2023/08/1637655738-linkedin-101-hero2x.jpg?fit=1801%2C950&ssl=1", 
            Title = "LinkedIn", 
            Text = "Gustavo Gondin Moeska - Full Stack Developer | C# | ASP.NET | Blazor | MAUI | SQL Server",
            Link = "https://www.linkedin.com/in/gumoeska/",
            TipoRedeSocial = TipoRedeSocial.Linkedin
        },
        #endregion

        #region Discord
        new Social
        {
            Image = "https://t.ctcdn.com.br/eA2jD3UpV_e6Y-aJ5uzpuokXFKA=/1892x1064/smart/i525670.png",
            Title = "Discord",
            Text = "Gustavo Gondin Moeska - DwiaK",
            Link = "https://discord.com/users/317447152911646722",
            TipoRedeSocial = TipoRedeSocial.Discord
        },
        #endregion
    };
    #endregion

    #region Métodos
    private void ExecutarLink(Social itemSocial) =>
        JSRunTime.InvokeAsync<string>("open", $"{itemSocial.Link}", "_blank");
    #endregion
}

public class Social
{
    public string? Image { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
    public string? Link { get; set; }
    public TipoRedeSocial TipoRedeSocial { get; set; }
}

public enum TipoRedeSocial
{
    [Description("GitHub")]
    Github,

    [Description("LinkedIn")]
    Linkedin,

    [Description("Discord")]
    Discord,

    [Description("WhatsApp")]
    Whatsapp,

    [Description("Email")]
    Email
}