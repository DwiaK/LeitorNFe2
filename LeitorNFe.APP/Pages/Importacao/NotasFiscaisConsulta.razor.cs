using LeitorNFe.App.Models;
using LeitorNFe.App.Models.NotaFiscal;
using LeitorNFe.App.Services.NotaFiscal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazor.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeitorNFe.App.Pages.Importacao;

public partial class NotasFiscaisConsulta
{
    [Parameter]
    public int Codigo { get; set; }
    
    [Parameter]
    public int Codigo2 { get; set; }

    #region Attributes
    [Inject]
    public Teste _TESTE { get; set; }

    [Inject]
    private INotaFiscalService _notaFiscalService { get; set; }
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

    private void DeletarNotaFiscal(int id)
    {
        _notaFiscalService.DeletarNotaFiscal(id);
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