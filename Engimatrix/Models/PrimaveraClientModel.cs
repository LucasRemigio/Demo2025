// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;

namespace engimatrix.Models;

public static class PrimaveraClientModel
{
    public static readonly List<MFPrimaveraClientItem> mockClients =
        [
            new MFPrimaveraClientItem
            {
                Cliente = "015001",
                Nome = "Metalúrgica Portuguesa, Lda",
                Contribuinte = "507452361",
                Morada = "Rua da Indústria, 123",
                Morada1 = "Zona Industrial",
                Localidade = "Braga",
                CodPostal = "4700",
                CodPostalLocalidade = "4700-352 Braga",
                Pais = "PT",
                Distrito = "Braga",
                Telemovel = "916745231",
                Email = "geral@metalurgicapt.pt",
                CondPag = "30 dias",
                ModoPag = "Transferência",
                Moeda = "EUR",
                TipoPvp = 1,
                Desconto = 5.5,
                ClienteAnulado = false,
                Avaliacao = "A",
                CodCesce = "C001",
                PlafoundCesce = "25000",
                EmailCliente = "compras@metalurgicapt.pt",
                EmailQualidade = "qualidade@metalurgicapt.pt",
                TipoTerceiro = "Cliente",
                Carro = "Entrega no cliente"
            },

            new MFPrimaveraClientItem
            {
                Cliente = "014968",
                Nome = "Construções Silva & Filhos, SA",
                Contribuinte = "503198726",
                Morada = "Av. da Liberdade, 45",
                Morada1 = "3º Andar",
                Localidade = "Lisboa",
                CodPostal = "1250",
                CodPostalLocalidade = "1250-141 Lisboa",
                Pais = "PT",
                Distrito = "Lisboa",
                Telemovel = "939876543",
                Email = "info@silvafilhos.pt",
                CondPag = "60 dias",
                ModoPag = "Cheque",
                Moeda = "EUR",
                TipoPvp = 2,
                Desconto = 3.0,
                ClienteAnulado = false,
                Avaliacao = "B",
                CodCesce = "C023",
                PlafoundCesce = "15000",
                EmailCliente = "encomendas@silvafilhos.pt",
                EmailQualidade = null,
                TipoTerceiro = "Cliente",
                Carro = "Transporte próprio"
            },

            new MFPrimaveraClientItem
            {
                Cliente = "10064",
                Nome = "Estruturas Metálicas do Norte",
                Contribuinte = "508765432",
                Morada = "Parque Industrial, Lote 7",
                Morada1 = null,
                Localidade = "Porto",
                CodPostal = "4450",
                CodPostalLocalidade = "4450-231 Porto",
                Pais = "PT",
                Distrito = "Porto",
                Telemovel = "927654321",
                Email = "geral@emn.pt",
                CondPag = "Pronto pagamento",
                ModoPag = "Multibanco",
                Moeda = "EUR",
                TipoPvp = 1,
                Desconto = 0,
                ClienteAnulado = false,
                Avaliacao = "A+",
                CodCesce = "C145",
                PlafoundCesce = "50000",
                EmailCliente = "compras@emn.pt",
                EmailQualidade = "qualidade@emn.pt",
                TipoTerceiro = "Cliente",
                Carro = "Levantamento"
            }
        ];
    private static Dictionary<string, MFPrimaveraClientItem> cachedPrimaveraClients = [];
    private static DateTime cacheExpirationDate = DateTime.MinValue;

    public static bool IsCacheValid()
    {
        if (cachedPrimaveraClients.Count <= 0)
        {
            return false;
        }

        if (DateTime.Now > cacheExpirationDate)
        {
            return false;
        }

        return true;
    }

    public static void InvalidateCache()
    {
        cachedPrimaveraClients.Clear();
        cacheExpirationDate = DateTime.MinValue;
    }

    public static async Task<Dictionary<string, MFPrimaveraClientItem>> GetPrimaveraClients()
    {
        if (IsCacheValid())
        {
            return cachedPrimaveraClients;
        }


        // cache the primavera clients
        foreach (MFPrimaveraClientItem primaveraClient in mockClients)
        {
            if (primaveraClient.Cliente == null)
            {
                continue;
            }

            cachedPrimaveraClients[primaveraClient.Cliente] = primaveraClient;
        }

        cacheExpirationDate = DateTime.Now.AddMinutes(30);

        return cachedPrimaveraClients;
    }

    public static async Task<MFPrimaveraClientItem?> GetPrimaveraClient(string clientCode)
    {
        Dictionary<string, MFPrimaveraClientItem> primaveraClients = await GetPrimaveraClients();

        if (primaveraClients.TryGetValue(clientCode, out var client))
        {
            return client;
        }

        Log.Debug($"PrimaveraClient with client code {clientCode} not found");
        return null;
    }

}
