using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazor.Utilities;
using System.Collections.Generic;

namespace LeitorNFe.App.Pages.Importacao;

public partial class ImportacaoManualPage
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