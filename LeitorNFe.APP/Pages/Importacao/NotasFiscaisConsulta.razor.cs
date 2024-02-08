using LeitorNFe.App.Models.NotaFiscal;
using LeitorNFe.App.Services.NotaFiscal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazor.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeitorNFe.App.Pages.Importacao;

public partial class NotasFiscaisConsulta
{
    #region Attributes
    [Inject]
    private INotaFiscalService NotaFiscalService { get; set; }
    #endregion

    #region Props
    public List<NotaFiscalModel> ListaNotasFiscais { get; set; } = null;
    #endregion

    #region Ctor
    public NotasFiscaisConsulta()
    {
    }

    public NotasFiscaisConsulta(INotaFiscalService notaFiscalService) =>
        NotaFiscalService = notaFiscalService;
    #endregion

    #region Métodos
    protected override async Task OnInitializedAsync()
    {
        ListaNotasFiscais = await NotaFiscalService.ListarNotasFiscais();
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