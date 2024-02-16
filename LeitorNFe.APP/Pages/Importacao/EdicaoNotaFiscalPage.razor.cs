using MudBlazor;
using System.Collections.Generic;

namespace LeitorNFe.App.Pages.Importacao;

public partial class EdicaoNotaFiscalPage
{
    #region Atributos
    
    #endregion

    #region Props
    #endregion

    #region Breadcrumbs
    private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Nota Fiscal", href: "/"),
        new BreadcrumbItem("Importação Manual", href: null, disabled: true)
    };
    #endregion

    #region Métodos
    
    #endregion
}