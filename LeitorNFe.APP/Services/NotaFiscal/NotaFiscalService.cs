using LeitorNFe.App.Models.NotaFiscal;
using LeitorNFe.App.Services.Utils;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;

namespace LeitorNFe.App.Services.NotaFiscal;

public class NotaFiscalService : INotaFiscalService
{
    private readonly HttpClient _httpClient;

    public NotaFiscalService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<NotaFiscalModel> BuscarNotaFiscalPorId(int id)
    {
        var request = await _httpClient.GetFromJsonAsync<NotaFiscalModel>($"api/NotaFiscal/BuscarNotaFiscalPorId/{id}");

        return request ?? throw new InvalidOperationException();
    }

    public async Task<bool> DeletarNotaFiscal(int id)
    {
        var request = await _httpClient.DeleteAsync($"api/NotaFiscal/DeletarNotaFiscal/{id}");

        if (request.StatusCode is HttpStatusCode.OK)
        {
            // Success
            return true;
        }
        else
        {
            // Error
            return false;
        }
    }

    public async Task<bool> ImportarNotaFiscal(NotaFiscalModel notaFiscal)
    {
        var request = await _httpClient.PostAsJsonAsync("api/NotaFiscal/ImportarNotaFiscal", notaFiscal);

        if (request.StatusCode is HttpStatusCode.OK)
        {
            // Success
            return true;
        }
        else
        {
            // Error
            return false;
        }
    }

    public async Task<List<NotaFiscalModel>> ListarNotasFiscais()
    {
        var request = await _httpClient.GetFromJsonAsync<List<NotaFiscalModel>>("api/NotaFiscal/BuscarNotasFiscais");

        return request ?? throw new InvalidOperationException();
    }

    public async Task<NotaFiscalModel> MontarNotaFiscal(IBrowserFile arquivo)
    {
        var extensions = new Extensions();

        // Converter de IBrowsable para XmlDocument
        var documentoXml = await extensions.ConverterArquivoXmlDocument(arquivo);
        var objetoNotaFiscal = extensions.RetornarObjetoXml(documentoXml);

        return objetoNotaFiscal;
    }
}
