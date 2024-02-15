using LeitorNFe.App.Models;
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
    public Teste _TESTE { get; set; }

    [Inject]
    public HttpClient _httpClient { get; set; }

    [Inject]
    private INotaFiscalService _notaFiscalService { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    public Index()
    {

    }

    protected override void OnInitialized()
    {
        _TESTE.Nome = "A";
        _TESTE.Nome2 = "B";
    }

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
