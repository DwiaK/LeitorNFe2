using LeitorNFe.App.Models.NotaFiscal;
using LeitorNFe.App.Services;
using LeitorNFe.App.Services.NotaFiscal;
using Microsoft.AspNetCore.Components;
using MudBlazor;
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
        var result = await _notaFiscalService.GetNotaFiscalById(5);

        var teste = result;

        _snackbar.Add($"{result.xNomeEmit}", Severity.Error);
        //_snackbar.Add("Network connection error", Severity.Error);
    }

    public void ImportarNotaFiscal()
    {
        // Teste

        _notaFiscalService.ImportNotaFiscal(new NotaFiscalModel()
        {
            nNF = "12345",
            chNFe = "13451358932462362346234623467",
            dhEmi = "05/02/2024",
            CNPJEmit = "12341341",
            xNomeEmit = "NomeEmit",
            CNPJDest = "125234952",
            xNomeDest = "NomeDest",
            EmailDest = "EmailDest",
            xLgr = "wfeqef",
            nro = "23423",
            xBairro = "erwgerg",
            xMun = " gererwwger",
            UF = "qwegqgeqgwe",
            CEP = "34254622"
        });
    }
}