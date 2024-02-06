using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using Toolbelt.Blazor.HotKeys;

namespace LeitorNFe.App.Components.Shared;

public partial class CommandPalette : IDisposable
{
    private readonly Dictionary<string, string> _paginas = new();

    private HotKeysContext _hotKeysContext;

    private Dictionary<string, string> _paginasFiltradas = new();

    private string _busca;

    [Inject] 
    private HotKeys HotKeys { get; set; }

    [Inject] 
    private NavigationManager Navigation { get; set; }

    [CascadingParameter] 
    private MudDialogInstance MudDialog { get; set; }

    public void Dispose()
    {
        _hotKeysContext?.Dispose();
    }

    protected override void OnInitialized()
    {
        AdicionarPaginas();

        _hotKeysContext = HotKeys.CreateContext()
            .Add(ModKeys.None, Keys.ESC, () => MudDialog.Close(), "Fechar comandos");
    }

    private void BuscarPaginas(string value)
    {
        _paginasFiltradas = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(value))
            _paginasFiltradas = _paginas
                .Where(x => x.Key.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .ToDictionary(x => x.Key, x => x.Value);
        else
            _paginasFiltradas = _paginas;
    }

    private void AdicionarPaginas()
    {
        // Separar por Section, Parent e Child

        _paginas.Add("Início", "/");
        _paginas.Add("Meus Projetos > Concluídos", "/");
        _paginas.Add("Meus Projetos > Em Desenvolvimento", "/");
        _paginas.Add("Meus Projetos > Projetos Futuros", "/");
        _paginas.Add("Meus Projetos > Projetos ", "/");
        _paginas.Add("Meus Projeos > Pros ", "/");
        _paginas.Add("Meus Proos ", "/");
        _paginas.Add("Mes Projetoetos ", "/");
        _paginas.Add("Meus Pos > Projetos ", "/");
        _paginas.Add("Mes > Projetos ", "/");
        _paginas.Add("Meus Proetos ", "/");
        _paginas.Add("Meetos ", "/");
        _paginas.Add("Meus tos ", "/");
        _paginas.Add("us Projetos ", "/");

        _paginasFiltradas = _paginas;
    }

    private void TesteMetodo()
    {

    }
}