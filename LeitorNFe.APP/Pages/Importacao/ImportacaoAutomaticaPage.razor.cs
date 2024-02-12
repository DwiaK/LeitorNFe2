using LeitorNFe.App.Services.Utils;
using LeitorNFe.App.Services.NotaFiscal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using static MudBlazor.CategoryTypes;

namespace LeitorNFe.App.Pages.Importacao;

public partial class ImportacaoAutomaticaPage
{
    #region Props
    private const string _defaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full z-10";
    private string _dragClass = _defaultDragClass;
    private bool _isValid;
    private bool _isTouched;

    private readonly List<string> _nomesArquivos = new();

    [Inject]
    private INotaFiscalService _notaFiscalService { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    IList<IBrowserFile> _arquivos = new List<IBrowserFile>();
    #endregion

    #region Breadcrumbs
    private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Nota Fiscal", href: "/"),
        new BreadcrumbItem("Importação Automática", href: null, disabled: true)
    };
    #endregion

    #region Métodos
    private void AdicionarNota(IReadOnlyList<IBrowserFile> arquivos) =>
        arquivos.ToList().ForEach(item => _arquivos.Add(item));

    private void ImportarArquivos()
    {
        // Importar Arquivos
        var arquivos = _arquivos.ToList();

        if (arquivos.IsNullOrEmpty())
        {
            _snackbar.Add($"É Necessário adicionar pelo menos um arquivo para importação.", Severity.Error);
            return;
        }

        var resultado = false;

        arquivos.ForEach(async item =>
        {
            var notaFiscal = await _notaFiscalService.MontarNotaFiscal(item);
            resultado = await _notaFiscalService.ImportarNotaFiscal(notaFiscal);
        });

        _snackbar.Add($"Nota(s) importada(s) com sucesso!", Severity.Success);
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
