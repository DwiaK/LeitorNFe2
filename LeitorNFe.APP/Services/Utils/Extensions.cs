using LeitorNFe.App.Models.NotaFiscal;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace LeitorNFe.App.Services.Utils;

public class Extensions
{
    #region Métodos Externos p/ uso Geral
    public async Task<XmlDocument> ConverterArquivoXmlDocument(IBrowserFile arquivo)
    {
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                var xmlDoc = new XmlDocument();

                // Copia p/ a memória
                await arquivo.OpenReadStream().CopyToAsync(memoryStream);

                // Cria e popula um XmlDocument
                memoryStream.Seek(0, SeekOrigin.Begin); // Volta para o início do MemoryStream
                xmlDoc.Load(memoryStream);

                memoryStream.Close();

                return xmlDoc;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public NotaFiscalModel RetornarObjetoXml(XmlDocument xmlDoc)
    {
        try
        {
            var nfObj = new NotaFiscalModel();

            #region Base
            nfObj.nNF = LerDadosXml(xmlDoc, "nNF", "ide");
            nfObj.dhEmi = LerDadosXml(xmlDoc, "dhEmi", "ide");
            nfObj.chNFe = LerDadosXml(xmlDoc, "chNFe", "infProt");
            #endregion

            #region Emitente
            // Emitente Base
            nfObj.xNomeEmit = LerDadosXml(xmlDoc, "xNome", "emit");
            nfObj.CNPJEmit = LerDadosXml(xmlDoc, "CNPJ", "emit");

            // Endereço Emitente
            nfObj.EnderecoEmitente = new NotaFiscalEnderecosModel();
            nfObj.EnderecoEmitente.IsEmit = true;
            nfObj.EnderecoEmitente.xLgr = LerDadosXml(xmlDoc, "xLgr", "enderEmit");
            nfObj.EnderecoEmitente.nro = LerDadosXml(xmlDoc, "nro", "enderEmit");
            nfObj.EnderecoEmitente.xBairro = LerDadosXml(xmlDoc, "xBairro", "enderEmit");
            nfObj.EnderecoEmitente.xMun = LerDadosXml(xmlDoc, "xMun", "enderEmit");
            nfObj.EnderecoEmitente.UF = LerDadosXml(xmlDoc, "UF", "enderEmit");
            nfObj.EnderecoEmitente.CEP = LerDadosXml(xmlDoc, "CEP", "enderEmit");
            #endregion

            #region Destinatário
            // Destinatário Base
            nfObj.xNomeDest = LerDadosXml(xmlDoc, "xNome", "dest");
            nfObj.CNPJDest = LerDadosXml(xmlDoc, "CNPJ", "dest");
            nfObj.EmailDest = LerDadosXml(xmlDoc, "email", "dest");

            // Endereço Destinatário
            nfObj.EnderecoDestinatario = new NotaFiscalEnderecosModel();
            nfObj.EnderecoDestinatario.IsEmit = false;
            nfObj.EnderecoDestinatario.xLgr = LerDadosXml(xmlDoc, "xLgr", "enderDest");
            nfObj.EnderecoDestinatario.nro = LerDadosXml(xmlDoc, "nro", "enderDest");
            nfObj.EnderecoDestinatario.xBairro = LerDadosXml(xmlDoc, "xBairro", "enderDest");
            nfObj.EnderecoDestinatario.xMun = LerDadosXml(xmlDoc, "xMun", "enderDest");
            nfObj.EnderecoDestinatario.UF = LerDadosXml(xmlDoc, "UF", "enderDest");
            nfObj.EnderecoDestinatario.CEP = LerDadosXml(xmlDoc, "CEP", "enderDest");
            #endregion
        
            return nfObj;
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region Métodos Internos de Controle
    private string _dadosXml = string.Empty;

    private string LerDadosXml(XmlDocument xmlDoc, string xmlTag, string xmlNodeParent)
    {
        try
        {
            foreach (XmlNode item in xmlDoc.ChildNodes)
                BuscarTagXml(item, xmlTag, xmlNodeParent);
        }
        catch (Exception) { throw; }

        return _dadosXml;
    }

    private void BuscarTagXml(XmlNode node, string tagXml, string nodePai)
    {
        if (Equals(node.LocalName, tagXml) && 
            Equals(node.ParentNode.LocalName, nodePai))
            _dadosXml = node.InnerText;
        else
            foreach (XmlNode item in node.ChildNodes)
                BuscarTagXml(item, tagXml, nodePai);
    }
    #endregion
}

public static class BasicExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null)
        {
            return true;
        }

        var collection = enumerable as ICollection<T>;
        if (collection != null)
        {
            return collection.Count < 1;
        }
        return !enumerable.Any();
    }
}
