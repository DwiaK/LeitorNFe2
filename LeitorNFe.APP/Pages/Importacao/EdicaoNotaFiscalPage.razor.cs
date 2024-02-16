using LeitorNFe.App.Models.NotaFiscal;
using LeitorNFe.App.Services.NotaFiscal;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeitorNFe.App.Pages.Importacao;

public partial class EdicaoNotaFiscalPage
{
    #region Atributos
    [Inject]
    private INotaFiscalService _notaFiscalService { get; set; }
    #endregion

    #region Props
    [Parameter]
    public int IdNotaFiscal { get; set; }

    private NotaFiscalModel NotaFiscal { get; set; }
    #endregion

    #region Breadcrumbs
    private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Nota Fiscal", href: "/"),
        new BreadcrumbItem("Importação Manual", href: null, disabled: true)
    };
    #endregion

    #region Métodos
    protected override void OnInitialized() =>
        BuscarInformacoesNotaFiscal(IdNotaFiscal);

    private async void BuscarInformacoesNotaFiscal(int idNotaFiscal)
    {
        var retornoNotaFiscal = await _notaFiscalService.BuscarNotaFiscalPorId(idNotaFiscal);

        if (retornoNotaFiscal is not null)
            NotaFiscal = retornoNotaFiscal;
    }
    #endregion
}