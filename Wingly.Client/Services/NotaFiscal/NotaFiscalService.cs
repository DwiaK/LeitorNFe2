using LeitorNFe.App.Models.NotaFiscal;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace LeitorNFe.App.Services.NotaFiscal;

public class NotaFiscalService : INotaFiscalService
{
    //private const string UriRequest = "sample-data/articles.json";
    private readonly HttpClient _httpClient;

    public NotaFiscalService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<NotaFiscalModel> GetNotaFiscalById(int id)
    {
        var request = await _httpClient.GetFromJsonAsync<NotaFiscalModel>($"api/NotaFiscal/GetNotaFiscalById/{id}");

        return request ?? throw new InvalidOperationException();
    }

    public async void ImportNotaFiscal(NotaFiscalModel notaFiscal)
    {
        // ESTRUTURA TEMPORÁRIA
        var response = await _httpClient.PostAsJsonAsync("api/NotaFiscal/ImportNotaFiscal", notaFiscal);

        //if (response.IsSuccessStatusCode)
        //{
        //    // Success
        //}
        //else
        //{
        //    // Error
        //}
    }
}
