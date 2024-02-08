using LeitorNFe.App.Models.NotaFiscal;
using LeitorNFe.App.Services.NotaFiscal;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbelt.Blazor.HotKeys;

namespace LeitorNFe.App.Components.Shared;

public partial class CommandPalette : IDisposable
{
    private readonly Dictionary<string, string> _paginas = new();

    private HotKeysContext _hotKeysContext;

    private Dictionary<string, string> _paginasFiltradas = new();

    private List<NotaFiscalModel> ListaNotasFiscais { get; set; }

    private string _busca;

    [Inject]
    private INotaFiscalService NotaFiscalService { get; set; }

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

    protected override async Task OnInitializedAsync()
    {
        ListaNotasFiscais = await NotaFiscalService.ListarNotasFiscais();

        AdicionarNotasFiscais(ListaNotasFiscais);

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

    private void AdicionarNotasFiscais(List<NotaFiscalModel> listaNotasFiscais)
    {
        //_paginas.Add("us Projetos ", "/");

        listaNotasFiscais.ForEach(item =>
        {
            var informacoesExibicaoNf = $"{item.xNomeEmit} {item.xNomeDest}";

            _paginas.Add(informacoesExibicaoNf, "/");
        });

        _paginasFiltradas = _paginas;
    }

    private void TesteMetodo()
    {

    }
}