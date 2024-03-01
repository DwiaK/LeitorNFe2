using LeitorNFe.App.Services.Utils;
using LeitorNFe.App.Services.NotaFiscal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using LeitorNFe.App.Models.NotaFiscal;
using System.Threading.Tasks;

namespace LeitorNFe.App.Pages.Importacao;

public partial class ImportacaoAutomaticaPage
{
    #region Props
    private const string _defaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full z-10";
    private string _dragClass = _defaultDragClass;

    private readonly List<string> _nomesArquivos = new();

    [Inject]
    private INotaFiscalService _notaFiscalService { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    IList<IBrowserFile> _arquivos = new List<IBrowserFile>();

    List<NotaFiscalModel> ListaNotasFiscaisAdicionadas = new List<NotaFiscalModel>();
    #endregion

    #region Breadcrumbs
    private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Nota Fiscal", href: "/"),
        new BreadcrumbItem("Importação Automática", href: null, disabled: true)
    };
    #endregion

    #region Métodos
    private void AdicionarNota(IReadOnlyList<IBrowserFile> arquivos)
    {
        // Adicionar objeto convertido p/ a Lista de Adicionados
        BuscarObjetoListaAdicionado(arquivos);

        // Adicionar item
        arquivos.ToList().ForEach(item => _arquivos.Add(item));
    }

    private async Task ImportarArquivos()
    {
        // Importar Arquivos
        var arquivos = _arquivos.ToList();

        if (arquivos.IsNullOrEmpty())
        {
            _snackbar.Add($"É Necessário adicionar pelo menos um arquivo para importação.", Severity.Error);
            return;
        }

        var listaNotasFiscais = new List<NotaFiscalModel>();

        await arquivos.ForEachAsync(async item =>
        {
            var notaFiscalMontada = await _notaFiscalService.MontarNotaFiscal(item);

            listaNotasFiscais.Add(notaFiscalMontada);
        });

        var resultado = await _notaFiscalService.ImportarMultiplasNotasFiscais(listaNotasFiscais);

        if (resultado is true)
            _snackbar.Add($"Nota(s) importada(s) com sucesso!", Severity.Success);
        else
            _snackbar.Add($"Ocorreu um erro ao Importar as Notas Fiscais.", Severity.Error);
    }

    private void BuscarObjetoListaAdicionado(IReadOnlyList<IBrowserFile> arquivos)
    {
        if (arquivos.IsNullOrEmpty())
            return;

        arquivos.ToList().ForEach(async item =>
        {
            var notaFiscal = await _notaFiscalService.MontarNotaFiscal(item);

            ListaNotasFiscaisAdicionadas.Add(notaFiscal);
        });

        StateHasChanged();
    }

    private void OnInputFileChanged(InputFileChangeEventArgs e)
    {
        ClearDragClass();
        var files = e.GetMultipleFiles();

        files.ToList().ForEach(file => _nomesArquivos.Add(file.Name));

        AdicionarNota(files);
    }

    private void RemoverNotas()
    {
        _arquivos.Clear();
        _nomesArquivos.Clear();
    }

    private void SetDragClass()
        => _dragClass = $"{_defaultDragClass} mud-border-primary";

    private void ClearDragClass()
        => _dragClass = _defaultDragClass;
    #endregion
}
