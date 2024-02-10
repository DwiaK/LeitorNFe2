using LeitorNFe.App.Models.NotaFiscal;
using LeitorNFe.App.Services.NotaFiscal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using MudBlazor.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static MudBlazor.CategoryTypes;

namespace LeitorNFe.App.Pages.Importacao;

public partial class ImportacaoAutomaticaPage
{
    #region Props
    [Inject]
    private INotaFiscalService _notaFiscalService { get; set; }

    [Inject]
    private ISnackbar _snackbar { get; set; }

    IList<IBrowserFile> _files = new List<IBrowserFile>();
    #endregion

    #region Breadcrumbs
    private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
    {
        new BreadcrumbItem("Nota Fiscal", href: "/"),
        new BreadcrumbItem("Importação Automática", href: null, disabled: true)
    };
    #endregion

    #region Métodos
    private async void ImportarArquivo(IBrowserFile file)
    {
        // Importar Arquivo
        _files.Add(file);

        var notaFiscal = await _notaFiscalService.MontarNotaFiscal(file);

        var resultado = await _notaFiscalService.ImportarNotaFiscal(notaFiscal);
    }

    private async Task<XmlDocument> LerArquivoBrowser(IBrowserFile arquivo)
    {
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                // Copie o conteúdo do arquivo para a memória
                await arquivo.OpenReadStream().CopyToAsync(memoryStream);

                // Crie um novo XmlDocument e carregue os bytes do arquivo
                XmlDocument xmlDoc = new XmlDocument();
                memoryStream.Seek(0, SeekOrigin.Begin); // Volte para o início do MemoryStream
                xmlDoc.Load(memoryStream);

                memoryStream.Close();

                return xmlDoc;
            }
        }
        catch (Exception ex)
        {
            // Trate exceções
            Console.WriteLine($"Erro ao converter para XmlDocument: {ex.Message}");
            return null;
        }
    }

    private void LerXml(XmlDocument xmlDoc)
    {
        try
        {
            NotaFiscalModel nf = new NotaFiscalModel();
            nf.nNF = LerDadosXml(xmlDoc, "xNome", "emit");

            _snackbar.Add($"{nf.nNF}", Severity.Error);
            //_arquivoModel.Nome = _leitorXmlService.LerDadosXml(arquivos[i], "xNome", "emit");
        }
        catch (Exception ex)
        {
        }
    }

    // Função que recebe o item xml, especifica a tag e o nó pai
    private string XmlValueData = string.Empty;
    private void GetXmlTag(XmlNode node, string xmlTag, string nodeParent)
    {
        if (node.LocalName == xmlTag) // nome da tag <>
        {
            if (node.ParentNode.LocalName == nodeParent) // nome do nó pai
            {
                this.XmlValueData = node.InnerText;
            }
        }
        else
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                GetXmlTag(item, xmlTag, nodeParent);
            }
        }
    }

    // Função que recebe o arquivo XML, a tag <> e a tag do nó pai
    public string LerDadosXml(XmlDocument xmlDoc, string xmlTag, string xmlNodeParent)
    {
        try
        {
            foreach (XmlNode item in xmlDoc.ChildNodes)
            {
                GetXmlTag(item, xmlTag, xmlNodeParent);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }

        return XmlValueData;
    }
    #endregion
}
