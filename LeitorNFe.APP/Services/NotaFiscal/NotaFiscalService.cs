﻿using LeitorNFe.App.Models.NotaFiscal;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

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

    public async Task<bool> ImportarNotaFiscal(NotaFiscalModel notaFiscal, NotaFiscalEnderecosModel enderecoNotaFiscal)
    {
        // Montar Requisição
        var importarNotaFiscalRequest = new ImportarNotaFiscalRequest
        {
            // Nf
            nNF = "12345",
            chNFe = "13451358932462362346234623467",
            dhEmi = "05/02/2024",
            CNPJEmit = "12341341",
            xNomeEmit = "NomeEmit",
            CNPJDest = "125234952",
            xNomeDest = "NomeDest",
            EmailDest = "EmailDest",

            // Endereço
            xLgr = "wfeqef",
            nro = "23423",
            xBairro = "erwgerg",
            xMun = " gererwwger",
            UF = "qwegqgeqgwe",
            CEP = "34254622"
        };

        var request = await _httpClient.PostAsJsonAsync("api/NotaFiscal/ImportarNotaFiscal", importarNotaFiscalRequest);

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
}


public class ImportarNotaFiscalRequest
{
    public int IdNotaFiscal { get; set; }

    #region Base
    public string nNF { get; set; }
    public string chNFe { get; set; }
    public string dhEmi { get; set; }
    #endregion

    #region Emitente
    public string CNPJEmit { get; set; }
    public string xNomeEmit { get; set; }
    #endregion

    #region Destinatário
    public string CNPJDest { get; set; }
    public string xNomeDest { get; set; }
    public string EmailDest { get; set; }
    #endregion

    #region Endereço
    public string xLgr { get; set; }
    public string nro { get; set; }
    public string xBairro { get; set; }
    public string xMun { get; set; }
    public string UF { get; set; }
    public string CEP { get; set; }
    #endregion
}