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

namespace LeitorNFe.App.Components.NotaFiscal;

public partial class InformacoesNotaFiscal
{
    #region Atributos
    [CascadingParameter] 
    MudDialogInstance MudDialog { get; set; }

    [Inject]
    private INotaFiscalService _notaFiscalService { get; set; }

    [Parameter]
    public int IdNotaFiscal { get; set; }
    #endregion

    #region Propriedades
    private NotaFiscalModel NotaFiscal { get; set; }
    #endregion

    #region Métodos
    protected override async Task OnInitializedAsync() => 
        await BuscarNotaFiscal(IdNotaFiscal);

    private async Task BuscarNotaFiscal(int id) =>
        NotaFiscal = await _notaFiscalService.BuscarNotaFiscalPorId(id);

    private void FecharDialog() =>
        MudDialog.Cancel();
    #endregion
}
