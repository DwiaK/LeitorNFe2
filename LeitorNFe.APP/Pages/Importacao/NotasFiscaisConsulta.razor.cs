using LeitorNFe.App.Components.NotaFiscal;
using LeitorNFe.App.Models.NotaFiscal;
using LeitorNFe.App.Services.NotaFiscal;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeitorNFe.App.Pages.Importacao;

public partial class NotasFiscaisConsulta
{
    #region Attributes
    [Inject]
    private INotaFiscalService _notaFiscalService { get; set; }

    [Inject]
    private NavigationManager _navigationManager { get; set; }

    [Inject]
    private IDialogService _dialogService { get; set; }
    #endregion

    #region Props
    public List<NotaFiscalModel> ListaNotasFiscais { get; set; } = null;
    #endregion

    #region Ctor
    public NotasFiscaisConsulta()
    {
    }

    public NotasFiscaisConsulta(INotaFiscalService notaFiscalService) =>
        _notaFiscalService = notaFiscalService;
    #endregion

    #region Métodos
    protected override async Task OnInitializedAsync()
    {
        ListaNotasFiscais = await _notaFiscalService.ListarNotasFiscais();
    }

    private async Task VisualizarNotaFiscal(int id)
    {
        var parameters = new DialogParameters<NotaFiscalModel> 
        { 
            { x => x.IdNotaFiscal, id } 
        };

        var options = new DialogOptions()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };

        var dialog = await _dialogService.ShowAsync<InformacoesNotaFiscal>("Informações da Nota Fiscal", parameters);
        var result = await dialog.Result;
    }

    private void EditarNotaFiscal(int id)
    {
        _navigationManager.NavigateTo($"/importacao/edicao/{id}");
    }

    private async void DeletarNotaFiscal(int id)
    {
        await _notaFiscalService.DeletarNotaFiscal(id);

		await _notaFiscalService.ListarNotasFiscais();

        StateHasChanged();
	}
    #endregion

    #region Breadcrumbs
    private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Nota Fiscal", href: "/"),
        new BreadcrumbItem("Consulta", href: null, disabled: true)
    };
    #endregion
}
