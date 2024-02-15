using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using LeitorNFe.App;
using LeitorNFe.App.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using LeitorNFe.App.Services.NotaFiscal;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;
using LeitorNFe.App.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7042/") });

builder.Services.AddMudServices();
builder.Services.AddHotKeys();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddTransient<INotaFiscalService, NotaFiscalService>();

await builder.Build().RunAsync();
