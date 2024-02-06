using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazor.Utilities;
using System.Collections.Generic;

namespace LeitorNFe.App.Pages.Importacao;

public partial class ImportacaoAutomaticaPage
{
    #region Props
    IList<IBrowserFile> files = new List<IBrowserFile>();
    #endregion

    #region Breadcrumbs
    private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Nota Fiscal", href: "/"),
        new BreadcrumbItem("Importa��o Autom�tica", href: null, disabled: true)
    };
    #endregion

    #region M�todos
    private void ImportarArquivo(IBrowserFile file)
    {
        files.Add(file);
    }
    #endregion
}