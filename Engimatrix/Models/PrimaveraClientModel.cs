// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;

namespace engimatrix.Models;

public static class PrimaveraClientModel
{
    public static string Contribuinte = "999999990";
    public static readonly List<MFPrimaveraClientItem> mockClients =
        [
            new MFPrimaveraClientItem
            {
                Cliente = "015001",
                Nome = "Metalúrgica Portuguesa, Lda",
                Contribuinte = Contribuinte,
                Morada = "Rua Afonso de Albuquerque",
                Morada1 = null,
                Localidade = "Leiria",
                CodPostal = "2400-080",
                CodPostalLocalidade = "LEIRIA",
                Pais = "PT",
                Distrito = "Leiria",
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
                Carro = "001"
            },

            new MFPrimaveraClientItem
            {
                Cliente = "014968",
                Nome = "Construções Silva & Filhos, SA",
                Contribuinte = Contribuinte,
                Morada = "Rua da Almada",
                Morada1 = "3º Andar",
                Localidade = "Porto",
                CodPostal = "4050-035",
                CodPostalLocalidade = "PORTO",
                Pais = "PT",
                Distrito = "Porto",
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
                Carro = "001"
            },

            new MFPrimaveraClientItem
            {
                Cliente = "010064",
                Nome = "Estruturas Metálicas do Norte",
                Contribuinte = Contribuinte,
                Morada = "Rua Alto das Torres",
                Morada1 = null,
                Localidade = "Vila Nova de Gaia",
                CodPostal = "4430-010",
                CodPostalLocalidade = "VILA NOVA DE GAIA",
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
                Carro = "001"
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
