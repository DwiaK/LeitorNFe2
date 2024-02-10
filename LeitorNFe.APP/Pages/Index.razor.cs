using LeitorNFe.App.Models.NotaFiscal;
using LeitorNFe.App.Services;
using LeitorNFe.App.Services.NotaFiscal;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;

namespace LeitorNFe.App.Pages;

public partial class Index
{
    [Inject]
    public HttpClient _httpClient { get; set; }

    [Inject]
    private INotaFiscalService _notaFiscalService { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    public async void GetNotaFiscalId()
    {
        var result = await _notaFiscalService.BuscarNotaFiscalPorId(10);

        var teste = result;

        _snackbar.Add($"{result.xNomeEmit}", Severity.Error);
        //_snackbar.Add("Network connection error", Severity.Error);
    }

    public async void BuscarNotasFiscais()
    {
        var listaNotasFiscais = await _notaFiscalService.ListarNotasFiscais();

        var teste = listaNotasFiscais;
    }
}
