using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using LeitorNFe.App.Components.Shared;
using LeitorNFe.App.Models;
using Toolbelt.Blazor.HotKeys;
using LeitorNFe.App.Styles;
using System;
using System.Threading.Tasks;

namespace LeitorNFe.App.Shared;

public partial class MainLayout : IDisposable
{
    private readonly Temas _temas = new();

    private readonly Palette _temaSelecionado = new();

    private readonly MudTheme _temaMud = new()
    {
        Palette = new Palette
        {
            Primary = Colors.Purple.Default
        },
        LayoutProperties = new LayoutProperties
        {
            AppbarHeight = "80px",
            DefaultBorderRadius = "12px"
        },
        Typography = new Typography
        {
            Default = new Default
            {
                FontSize = "0.9rem",
            }
        }
    };

    private bool _canMiniSideMenuDrawer = true;
    private bool _paletaComandosAberta;

    private HotKeysContext? _hotKeysContext;
    private bool _sideMenuDrawerOpen;

    private GerenciaTemaModel _gerenciadorTema = new()
    {
        ModoEscuro = false,
        CorPrimaria = Colors.Green.Default
    };

    [Inject] 
    private IDialogService _dialogService { get; set; }

    [Inject] 
    private HotKeys _hotKeys { get; set; }

    [Inject] 
    private ILocalStorageService _localStorage { get; set; }

    public void Dispose() =>
        _hotKeysContext?.Dispose();

    protected override async Task OnInitializedAsync()
    {
        // Inicializar Tema
        _temas.GetTema(Tema.LeitorNFe);

        // Buscar tema armazenado localmente
        if (await _localStorage.ContainKeyAsync("themeManager"))
            _gerenciadorTema = await _localStorage.GetItemAsync<GerenciaTemaModel>("themeManager");

        // Gerenciar Tema Alterado
        await GerenciadorTemaAlterado(_gerenciadorTema);

        // Criar Contexto p/ as Hotkeys
        _hotKeysContext = _hotKeys.CreateContext()
            .Add(ModKeys.Alt, Keys.P, AbrirPaletaComandos, "Abrir comandos.");
    }

    private void ToggleSideMenuDrawer() =>
        _sideMenuDrawerOpen = !_sideMenuDrawerOpen;

    private async Task GerenciadorTemaAlterado(GerenciaTemaModel gerenciadorTema)
    {
        // Alternar Temas
        _gerenciadorTema = gerenciadorTema;
        _temaMud.Palette = _gerenciadorTema.ModoEscuro ? 
            _temas.GetTema(Tema.LeitorNFe) : 
            new Palette();

        // Salvar Tema
        await AtualizarArmazenamentoGerenciadorTemas();
    }

    private async Task AbrirPaletaComandos()
    {
        if (!_paletaComandosAberta)
        {
            var options = new DialogOptions
            {
                NoHeader = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Small,
                Position = DialogPosition.TopCenter,
            };

            var commandPalette = _dialogService.Show<CommandPalette>("", options);
            _paletaComandosAberta = true;

            await commandPalette.Result;
            _paletaComandosAberta = false;
        }
    }

    private async Task AtualizarArmazenamentoGerenciadorTemas() =>
        await _localStorage.SetItemAsync("themeManager", _gerenciadorTema);
}