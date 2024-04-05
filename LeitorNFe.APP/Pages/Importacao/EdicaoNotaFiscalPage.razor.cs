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

    [Inject]
    private NavigationManager _navigationManager { get; set; }

    private bool _isDrawerOpen = false;
    #endregion

    #region Props
    [Parameter]
    public int IdNotaFiscal { get; set; }

    private NotaFiscalModel NotaFiscal { get; set; }

    private string Descricao { get; set; }

	[Inject]
	private ISnackbar _snackbar { get; set; }
	#endregion

	#region Breadcrumbs
	private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Nota Fiscal", href: "/"),
        new BreadcrumbItem("Edição", href: null, disabled: true)
    };
    #endregion

    #region Métodos
    protected override async Task OnInitializedAsync() =>
        await BuscarInformacoesNotaFiscal(IdNotaFiscal);

    private void AbrirDrawer() =>
        _isDrawerOpen = !_isDrawerOpen;

    private async Task BuscarInformacoesNotaFiscal(int idNotaFiscal)
    {
        var retornoNotaFiscal = await _notaFiscalService.BuscarNotaFiscalPorId(idNotaFiscal);

        if (retornoNotaFiscal is not null)
        {
            NotaFiscal = retornoNotaFiscal;
            Descricao = NotaFiscal.Descricao;
        }
    }

    private void CancelarEdicao() =>
        _navigationManager.NavigateTo($"/importacao/consulta");

    private async void SalvarEdicao()
    {
        // Setar descrição
        if (!string.IsNullOrEmpty(Descricao))
            NotaFiscal.Descricao = Descricao;

        // Update
        var retorno = await _notaFiscalService.EditarNotaFiscal(NotaFiscal);

		if (retorno is true)
			_snackbar.Add($"Nota editada com sucesso!", Severity.Success);
		else
			_snackbar.Add($"Ocorreu um erro ao Editar a Nota Fiscal.", Severity.Error);

	}
    #endregion
}
