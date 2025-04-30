// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using System.Globalization;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Primavera;
using engimatrix.Utils;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.CallRecords;
using Microsoft.IdentityModel.Tokens;

namespace engimatrix.Models;

public static class ClientModel
{
    private static bool IsSyncingPrimaveraClients;
    public static List<ClientItem> GetClients(string execute_user)
    {
        string query = $"SELECT id, code, token, segment_id, created_at, created_by, updated_at, updated_by FROM mf_client";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "getClients");

        List<ClientItem> clients = [];

        if (!response.operationResult)
        {
            throw new Exception("Error getting clients from mf_client");
        }

        if (response.out_data.Count <= 0)
        {
            return clients;
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            DateTime createdAt = !string.IsNullOrEmpty(item["created_at"]) ? DateTime.Parse(item["created_at"]) : default;
            DateTime updatedAt = !string.IsNullOrEmpty(item["updated_at"]) ? DateTime.Parse(item["updated_at"]) : default;
            ClientItem client = new ClientItemBuilder()
                .SetId(Int32.Parse(item["id"]))
                .SetCode(item["code"])
                .SetToken(item["token"])
                .SetSegmentId(Int32.Parse(item["segment_id"]))
                .SetCreatedAt(createdAt)
                .SetCreatedBy(item["created_by"])
                .SetUpdatedAt(updatedAt)
                .SetUpdatedBy(item["updated_by"])
                .Build();

            clients.Add(client);
        }

        return clients;
    }

    public async static Task<List<ClientDTO>> SearchClients(string? search_query, string execute_user)
    {
        // The search query must split the words and then search for either the code or the name
        List<MFPrimaveraClientItem> primaveraClients = [.. (await PrimaveraClientModel.GetPrimaveraClients()).Values];

        // given the search query is empty, we must get the top 20 clients with most orders
        if (string.IsNullOrEmpty(search_query))
        {
            List<string> clientCodes = GetTop20ClientsWithMostOrders(execute_user);
            return await GetClientsByCode(clientCodes, execute_user);
        }

        List<string> words = [.. search_query.Split(' ')];

        // given the search query is not empty, we must search for the client code or the name
        List<string> primaveraCodes = [.. primaveraClients
            .Where(client => words.Any(word => client.Cliente?.Contains(word, StringComparison.OrdinalIgnoreCase) == true ||
                            client.Nome?.Contains(word, StringComparison.OrdinalIgnoreCase) == true))
            .Select(client => client.Cliente ?? string.Empty)];

        //limit the codes to 20
        primaveraCodes = [.. primaveraCodes.Take(20)];

        return await GetClientsByCode(primaveraCodes, execute_user);
    }

    public async static Task<List<ClientDTO>> GetClientsByCode(List<string> clientCodes, string execute_user)
    {
        if (clientCodes.Count == 0)
        {
            return [];
        }

        List<ClientRatingDTO> ratings = ClientRatingModel.GetClientRatingsByCodeDTO(clientCodes, execute_user);
        Dictionary<string, List<ClientRatingDTO>> ratingsByCode = ratings.GroupBy(rating => rating.client_code).ToDictionary(group => group.Key, group => group.ToList());

        string query = @$"SELECT c.id, c.code, s.name AS segment_name, s.id AS segment_id
            FROM mf_client c
            JOIN mf_segment s ON c.segment_id = s.id
            WHERE c.code IN ('{string.Join("','", clientCodes)}')";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "getClientsByCode");

        if (!response.operationResult)
        {
            throw new Exception("Error getting clients from mf_client");
        }

        List<ClientDTO> clients = [];

        foreach (Dictionary<string, string> item in response.out_data)
        {
            MFPrimaveraClientItem? primaveraClient = await PrimaveraClientModel.GetPrimaveraClient(item["code"]);
            if (primaveraClient == null)
            {
                Log.Error($"No primavera client found for client code: {item["code"]}");
                continue;
            }

            if (!ratingsByCode.TryGetValue(item["code"], out List<ClientRatingDTO>? clientRatings))
            {
                clientRatings = [];
            }

            SegmentItem segment = new(Int32.Parse(item["segment_id"]), item["segment_name"]);

            ClientDTO client = new ClientDTOBuilder()
                .SetId(Int32.Parse(item["id"]))
                .SetCode(item["code"])
                .SetSegment(segment)
                .SetPrimaveraClient(primaveraClient)
                .SetRatings(clientRatings)
                .Build();

            clients.Add(client);
        }

        return clients;
    }

    public static List<string> GetTop20ClientsWithMostOrders(string execute_user)
    {
        string query =
            @$"SELECT client_code, COUNT(*) AS amount 
            FROM `order` 
            WHERE client_code IS NOT NULL AND client_code <> '' 
            GROUP BY client_code 
            ORDER BY amount DESC
            LIMIT 20";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "getTop20ClientsWithMostOrders");

        if (!response.operationResult)
        {
            throw new Exception("Error getting clients from mf_client");
        }

        List<string> clients = [];

        foreach (Dictionary<string, string> item in response.out_data)
        {
            clients.Add(item["client_code"]);
        }

        return clients;
    }

    public static ClientItem GetClientById(int id, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "id", id.ToString() }
        };

        string query = $"SELECT id, code, segment_id FROM mf_client WHERE id = @id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getClient");

        if (!response.operationResult)
        {
            throw new Exception("Error getting clients from mf_client");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for clients");
        }

        Dictionary<string, string> item = response.out_data[0];
        ClientItem client = new ClientItemBuilder()
            .SetId(Int32.Parse(item["id"]))
            .SetCode(item["code"])
            .SetSegmentId(Int32.Parse(item["segment_id"]))
            .Build();

        return client;
    }

    public static ClientItem? GetClientByCode(string code, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "code", code }
        };

        string query = $"SELECT id, code, segment_id FROM mf_client WHERE code = @code";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getClient");

        if (!response.operationResult)
        {
            throw new Exception("Error getting clients from mf_client");
        }

        if (response.out_data.Count <= 0)
        {
            Log.Debug($"GetClientByCode: Client with code {code} not found");
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];
        ClientItem client = new ClientItemBuilder()
            .SetId(Int32.Parse(item["id"]))
            .SetCode(item["code"])
            .SetSegmentId(Int32.Parse(item["segment_id"]))
            .Build();

        return client;
    }

    public static ClientDTO? GetClientByCodeDTO(string code, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "code", code }
        };

        string query = $"SELECT c.id AS client_id, c.code AS client_code, " +
            "c.segment_id AS segment_id, s.name AS segment_name " +
            "FROM mf_client c " +
            "JOIN mf_segment s ON c.segment_id = s.id " +
            "WHERE code = @code";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetClientByCodeDTO");

        if (!response.operationResult)
        {
            throw new Exception("Error getting clients from mf_client");
        }

        if (response.out_data.Count <= 0)
        {
            Log.Debug($"GetClientByCodeDTO: Client with code {code} not found");
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];

        SegmentItem segmentItem = new(Int32.Parse(item["segment_id"]), item["segment_name"]);

        ClientDTO client = new ClientDTOBuilder()
            .SetId(Int32.Parse(item["client_id"]))
            .SetCode(item["client_code"])
            .SetSegment(segmentItem)
            .Build();

        return client;
    }

    public static ClientDTO? GetClientByCodeDTOWithPrimavera(string code, string execute_user)
    {
        if (string.IsNullOrEmpty(code))
        {
            return null;
        }

        ClientDTO? client = GetClientByCodeDTO(code, execute_user);
        if (client == null)
        {
            return null;
        }

        MFPrimaveraClientItem? primaveraClient = PrimaveraClientModel.GetPrimaveraClient(code).Result;
        if (primaveraClient == null)
        {
            return client;
        }

        client.primavera_client = primaveraClient;

        return client;
    }

    public async static Task<ClientDTO> GetClientWithRatingsByCodeDTO(string code, string execute_user)
    {
        string query = $"SELECT c.id AS client_id, c.code AS client_code, s.id AS segment_id, s.name AS segment_name, " +
            "c.updated_at AS updated_at, c.updated_by AS updated_by, " +
            "c.created_at AS created_at, c.created_by AS created_by " +
            "FROM mf_client c " +
            "JOIN mf_segment s ON c.segment_id = s.id " +
            "WHERE code = @code";

        Dictionary<string, string> dic = new()
        {
            { "code", code }
        };

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getClientWithRatings");

        if (!response.operationResult)
        {
            throw new Exception("Error getting clients from mf_client");
        }

        if (response.out_data.Count <= 0)
        {
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];
        SegmentItem segment = new(Int32.Parse(item["segment_id"]), item["segment_name"]);
        string clientCode = item["client_code"];
        int clientId = Int32.Parse(item["client_id"]);

        List<ClientRatingDTO> ratings = ClientRatingModel.GetClientRatingsByCodeDTO(clientCode, execute_user);

        ClientDTOBuilder clientDTOBuilder = new ClientDTOBuilder().SetId(clientId).SetCode(clientCode).SetSegment(segment).SetRatings(ratings);

        MFPrimaveraClientItem? primaveraClient = await PrimaveraClientModel.GetPrimaveraClient(clientCode);
        if (primaveraClient != null)
        {
            clientDTOBuilder.SetPrimaveraClient(primaveraClient);
        }

        decimal weightedRating = ClientRatingModel.CalculateWeightedClientRatingByDTO(ratings);
        clientDTOBuilder.SetWeightedRating(weightedRating);

        ClientDTO clientDTO = clientDTOBuilder.Build();

        return clientDTO;
    }

    public async static Task<ClientNoAuthDTO?> GetClientNoAuthByTokenDto(string token, string execute_user)
    {
        string query = $"SELECT c.code AS client_code, c.token AS client_token, s.id AS segment_id, s.name AS segment_name " +
            "FROM mf_client c " +
            "JOIN mf_segment s ON c.segment_id = s.id " +
            "WHERE token = @token";

        Dictionary<string, string> dic = new()
        {
            { "token", token }
        };

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getClientNoAuthByTokenDto");

        if (!response.operationResult)
        {
            throw new Exception("Error getting client by token dto from mf_client");
        }

        if (response.out_data.Count <= 0)
        {
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];
        SegmentItem segment = new(Int32.Parse(item["segment_id"]), item["segment_name"]);
        string clientCode = item["client_code"];

        ClientNoAuthDTOBuilder client = new ClientNoAuthDTOBuilder().SetToken(item["client_token"]).SetCode(clientCode).SetSegment(segment);

        MFPrimaveraClientItem? primaveraClient = await PrimaveraClientModel.GetPrimaveraClient(clientCode);
        if (primaveraClient != null)
        {
            MFPrimaveraClientItemNoAuth primaveraClientNoAuth = new()
            {
                Cliente = primaveraClient.Cliente,
                Nome = primaveraClient.Nome,
                Email = primaveraClient.Email
            };

            client.SetPrimaveraClient(primaveraClientNoAuth);
        }

        return client.Build();
    }


    public async static Task<ClientDTO> GetClientWithRatingsByCodeDTOByOrderToken(string code, string orderToken, string execute_user)
    {
        string query = $"SELECT c.id AS client_id, c.code AS client_code, s.id AS segment_id, s.name AS segment_name, " +
            "c.updated_at AS updated_at, c.updated_by AS updated_by, " +
            "c.created_at AS created_at, c.created_by AS created_by " +
            "FROM mf_client c " +
            "JOIN mf_segment s ON c.segment_id = s.id " +
            "WHERE code = @code";

        Dictionary<string, string> dic = new()
        {
            { "code", code }
        };

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "getClientWithRatings");

        if (!response.operationResult)
        {
            throw new Exception("Error getting clients from mf_client");
        }

        if (response.out_data.Count <= 0)
        {
            return null;
        }

        Dictionary<string, string> item = response.out_data[0];
        SegmentItem segment = new(Int32.Parse(item["segment_id"]), item["segment_name"]);
        string clientCode = item["client_code"];
        int clientId = Int32.Parse(item["client_id"]);

        List<ClientRatingDTO> ratings = ClientRatingModel.GetClientRatingsDTOByCodeByOrderToken(clientCode, orderToken, execute_user);

        ClientDTOBuilder clientDTOBuilder = new ClientDTOBuilder().SetId(clientId).SetCode(clientCode).SetSegment(segment).SetRatings(ratings);

        MFPrimaveraClientItem? primaveraClient = await PrimaveraClientModel.GetPrimaveraClient(clientCode);
        if (primaveraClient != null)
        {
            clientDTOBuilder.SetPrimaveraClient(primaveraClient);
        }

        decimal weightedRating = ClientRatingModel.CalculateWeightedClientRatingByDTO(ratings);
        clientDTOBuilder.SetWeightedRating(weightedRating);

        ClientDTO clientDTO = clientDTOBuilder.Build();

        return clientDTO;
    }

    public async static Task<(List<PrimaveraOrderItem> orders, List<MFPrimaveraInvoiceItem> invoices)> GetCreditClientStatisticsForRating(string clientCode)
    {
        // get the pending orders
        List<PrimaveraOrderItem> orders = await PrimaveraOrderModel.GetPendingClientOrdersPrimavera(clientCode);

        // get the pending invoices
        List<MFPrimaveraInvoiceItem> invoices = await PrimaveraInvoiceModel.GetPendingPrimaveraInvoicesByClientCode(clientCode);

        return new(orders, invoices);
    }

    public async static Task<List<ClientDTO>> GetClientsWithRatingsDTO(string execute_user)
    {
        string query = $"SELECT c.id AS client_id, c.code AS client_code, " +
                "s.id AS segment_id, s.name AS segment_name, " +
                "c.updated_at AS updated_at, c.updated_by AS updated_by, " +
                "c.created_at AS created_at, c.created_by AS created_by " +
            "FROM mf_client c " +
            "JOIN mf_segment s ON c.segment_id = s.id " +
            "ORDER BY c.code";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "getClientWithRatings");

        if (!response.operationResult)
        {
            throw new Exception("Error getting clients from mf_client");
        }

        if (response.out_data.Count <= 0)
        {
            return [];
        }

        List<ClientDTO> clients = [];

        List<ClientRatingDTO> ratings = ClientRatingModel.GetClientRatingsDTO(execute_user);

        // Hash the lists by client_code
        Dictionary<string, List<ClientRatingDTO>> ratingsByClient = [];
        foreach (ClientRatingDTO rating in ratings)
        {
            if (!ratingsByClient.TryGetValue(rating.client_code, out var clientRatings))
            {
                clientRatings = [];
                ratingsByClient[rating.client_code] = clientRatings;
            }

            clientRatings.Add(rating);
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            SegmentItem segment = new(Int32.Parse(item["segment_id"]), item["segment_name"]);

            int clientId = Int32.Parse(item["client_id"]);
            string clientCode = item["client_code"];
            DateTime createdAt = !string.IsNullOrEmpty(item["created_at"]) ? DateTime.Parse(item["created_at"]) : default;
            DateTime? updatedAt = !string.IsNullOrEmpty(item["updated_at"]) ? DateTime.Parse(item["updated_at"]) : null;
            string createdBy = item["created_by"];
            string updatedBy = item["updated_by"];

            // get ratings for this client
            List<ClientRatingDTO> clientRatings = ratingsByClient.ContainsKey(clientCode) ? ratingsByClient[clientCode] : [];

            ClientDTOBuilder clientDTOBuilder = new ClientDTOBuilder()
                .SetId(clientId)
                .SetCode(clientCode)
                .SetSegment(segment)
                .SetRatings(clientRatings)
                .SetCreatedBy(createdBy)
                .SetCreatedAt(createdAt)
                .SetUpdatedBy(updatedBy)
                .SetUpdatedAt(updatedAt);

            MFPrimaveraClientItem? primaveraClient = await PrimaveraClientModel.GetPrimaveraClient(clientCode);
            if (primaveraClient != null)
            {
                clientDTOBuilder.SetPrimaveraClient(primaveraClient);
            }

            decimal weightedRating = ClientRatingModel.CalculateWeightedClientRatingByDTO(clientRatings);
            clientDTOBuilder.SetWeightedRating(weightedRating);

            ClientDTO clientDTO = clientDTOBuilder.Build();

            clients.Add(clientDTO);
        }

        return clients;
    }

    public async static Task<SyncPrimaveraStats> SyncPrimaveraClientsAndCreatePending(string executeUser)
    {
        if (IsSyncingPrimaveraClients)
        {
            throw new AlreadyLoadingException("Already syncing clients, skipping");
        }

        IsSyncingPrimaveraClients = true;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        List<ClientItem> clients = GetClients(executeUser);

        // Hash the clients response for O(1) lookup time
        Dictionary<string, ClientItem> clientsDic = [];
        foreach (ClientItem client in clients)
        {
            clientsDic[client.code] = client;
        }

        // Get primvara clients
        Dictionary<string, MFPrimaveraClientItem> primaveraClients = await PrimaveraClientModel.GetPrimaveraClients();

        // Iterate through each Primavera client
        int numberOfSyncedClients = 0;
        foreach (KeyValuePair<string, MFPrimaveraClientItem> primaveraClientKeyPair in primaveraClients)
        {
            string primaveraClientCode = primaveraClientKeyPair.Key;
            MFPrimaveraClientItem primaveraClient = primaveraClientKeyPair.Value;

            // Check if we already have the client
            if (!clientsDic.TryGetValue(primaveraClientCode, out ClientItem? matchingClient))
            {
                // Client doesn't exist in the database, create it
                int segmentId = GetMatchingSegmentFromPrimavera(primaveraClient.TipoTerceiro);
                ClientItem clientCreate = new ClientItemBuilder().SetCode(primaveraClientCode).SetSegmentId(segmentId).Build();
                CreateNewClient(clientCreate, executeUser);
                numberOfSyncedClients++;
                continue;
            }

            // Check if the client segment is different
            int matchingSegment = GetMatchingSegmentFromPrimavera(primaveraClient.TipoTerceiro);
            if (matchingClient.segment_id == matchingSegment)
            {
                // nothing to update 
                if (!string.IsNullOrEmpty(matchingClient.updated_by) && !string.IsNullOrEmpty(matchingClient.token))
                {
                    continue;
                }

                //even if the segment is the same, we want to update it so that the frontend shows the client has been
                // correctly identified by the correct segment, if not updated before
            }

            if (string.IsNullOrEmpty(matchingClient.token))
            {
                // Generate a token for the client
                string token = Guid.NewGuid().ToString();
                matchingClient.token = token;
            }

            ClientItem clientToUpdate = new ClientItemBuilder()
                .SetId(matchingClient.id)
                .SetCode(primaveraClientCode)
                .SetToken(matchingClient.token)
                .SetSegmentId(matchingSegment)
                .SetUpdatedAt(DateTime.Now)
                .SetUpdatedBy(executeUser)
                .Build();

            UpdateClient(clientToUpdate, executeUser);
            // Patch the segment
            numberOfSyncedClients++;
        }

        IsSyncingPrimaveraClients = false;
        stopwatch.Stop();
        Log.Debug($"Syncing clients finished with {numberOfSyncedClients} clients synced in {stopwatch.ElapsedMilliseconds}ms");
        SyncPrimaveraStats stats = new SyncPrimaveraStats(stopwatch.ElapsedMilliseconds, numberOfSyncedClients);
        return stats;
    }

    public static int GetMatchingSegmentFromPrimavera(string? primaveraSegmentId)
    {
        if (string.IsNullOrEmpty(primaveraSegmentId))
        {
            return SegmentConstants.SegmentId.OUTROS;
        }

        if (!int.TryParse(primaveraSegmentId, out int segmentId))
        {
            return SegmentConstants.SegmentId.OUTROS;
        }

        return segmentId switch
        {
            (int)PrimaveraEnums.TipoTerceiroSegmento.REVENDEDOR => SegmentConstants.SegmentId.REVENDA,
            (int)PrimaveraEnums.TipoTerceiroSegmento.INDUSTRIA_METALOMECANICA => SegmentConstants.SegmentId.INDUSTRIA,
            (int)PrimaveraEnums.TipoTerceiroSegmento.CONSTRUTORES => SegmentConstants.SegmentId.CONSTRUCAO,
            (int)PrimaveraEnums.TipoTerceiroSegmento.SERRALHARIA => SegmentConstants.SegmentId.SERRALHARIA,
            _ => SegmentConstants.SegmentId.OUTROS,
        };
    }

    public async static Task AddPrimaveraClient(string clientCode, string executeUser)
    {
        MFPrimaveraClientItem? primaveraClient = await PrimaveraClientModel.GetPrimaveraClient(clientCode) ?? throw new NotFoundException($"Primavera client {clientCode} not found");
        ClientItem? client = GetClientByCode(clientCode, "System");

        if (client != null)
        {
            Log.Info($"Client {clientCode} already exists in the database.");
            return;
        }

        int segmentId = GetMatchingSegmentFromPrimavera(primaveraClient.TipoTerceiro);
        ClientItem createClient = new ClientItemBuilder().SetCode(clientCode).SetSegmentId(segmentId).Build();
        CreateNewClient(createClient, executeUser);

        Log.Info($"Client with code {clientCode} and name {primaveraClient.Nome} and email {primaveraClient.Email} added to the database.");
    }

    public static void CreateNewClient(ClientItem client, string executeUser)
    {
        if (string.IsNullOrEmpty(client.code) || client.segment_id <= 0)
        {
            throw new InputNotValidException("Client code and segment_id are required");
        }

        string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        Dictionary<string, string> dic = new()
        {
            { "@ClientCode", client.code},
            { "@SegmentId", client.segment_id.ToString()},
            { "@CreatedAt", currentDate },
            { "@CreatedBy", executeUser},
            { "@UpdatedAt", currentDate },
            { "@UpdatedBy", executeUser}
        };

        string insertQuery = @$"INSERT INTO mf_client 
            (code, segment_id, created_at, created_by, updated_at, updated_by) 
            VALUES 
            (@ClientCode, @SegmentId, @CreatedAt, @CreatedBy, @UpdatedAt, @UpdatedBy)";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(insertQuery, dic, executeUser, false, "InsertNewClient");

        if (!response.operationResult)
        {
            throw new Exception($"Error creating new client with code {client.code}");
        }

        Log.Debug($"Client with code {client.code} created successfully.");

        CreateDefaultRatings(client.code, executeUser);
    }

    private static void CreateDefaultRatings(string clientCode, string executeUser)
    {
        // This is so that, if we create more ratings, the next statement updates automatically
        List<RatingTypeItem> ratingTypes = RatingTypeModel.GetClientRatingTypes(executeUser);

        // TODO: Hardcoded default rating
        char defaultRating = 'D';
        Dictionary<string, string> dic = new()
        {
            { "@ClientCode", clientCode },
            { "@DefaultRating", defaultRating.ToString() },
            { "@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "@CreatedBy", executeUser }
        };

        string insertQuery = $"INSERT INTO mf_client_rating (client_code, rating_type_id, rating, created_at, created_by) VALUES ";

        List<string> valueList = [];
        foreach (RatingTypeItem rating in ratingTypes)
        {
            valueList.Add($"(@ClientCode, {rating.id}, @DefaultRating, @CreatedAt, @CreatedBy)");
        }

        // Join the values with commas and append to the insert query
        insertQuery += string.Join(", ", valueList);

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(insertQuery, dic, executeUser, false, "InsertDefaultRatings");

        if (!response.operationResult)
        {
            throw new Exception($"Error creating default ratings for client with code {clientCode}");
        }

        Log.Debug($"Default ratings for client with code {clientCode} created successfully.");
    }

    public static void PatchSegment(string clientCode, int segmentId, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "@SegmentId", segmentId.ToString() },
            { "@ClientCode", clientCode },
            { "@UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "@UpdatedBy", execute_user }
        };

        string query = $"UPDATE mf_client " +
            "SET segment_id = @SegmentId, updated_at = @UpdatedAt, updated_by = @UpdatedBy " +
            "WHERE code = @ClientCode";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, true, "patchSegment");

        if (!response.operationResult)
        {
            throw new Exception($"Error patching segment for clientCode {clientCode} and segmentId {segmentId}");
        }
    }

    public static void UpdateClient(ClientItem client, string executeUser)
    {
        Dictionary<string, string> dic = new()
        {
            { "@Token", client.token },
            { "@Id", client.id.ToString() },
            { "@Code", client.code },
            { "@SegmentId", client.segment_id.ToString() },
            { "@UpdatedAt", client.updated_at.ToString("yyyy-MM-dd HH:mm:ss") },
            { "@UpdatedBy", client.updated_by }
        };

        string query = $"UPDATE mf_client " +
            "SET token = @Token, updated_at = @UpdatedAt, updated_by = @UpdatedBy " +
            "WHERE id = @Id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, executeUser, true, "updateClient");

        if (!response.operationResult)
        {
            throw new DatabaseException($"Error updating client with id {client.id}");
        }
    }

    public async static void FixClientCodes(string executeUser)
    {
        string query = $"SELECT c.id AS client_id, c.code AS client_code " +
            "FROM mf_client c " +
            "ORDER BY c.code";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], executeUser, false, "getClientWithRatings");

        if (!response.operationResult)
        {
            throw new Exception("Error getting clients from mf_client");
        }

        if (response.out_data.Count <= 0)
        {
            throw new InputNotValidException("Error getting record for clients");
        }

        foreach (Dictionary<string, string> item in response.out_data)
        {
            int clientId = Int32.Parse(item["client_id"]);
            string clientCode = item["client_code"];

            MFPrimaveraClientItem? primaveraClient = await PrimaveraClientModel.GetPrimaveraClient(clientCode);
            if (primaveraClient == null)
            {
                // fix the current client code with the primavara one
                Log.Error("No primavera client found");
                continue;
            }

            // Construct the SQL query to update the client code
            string updateQuery = $"UPDATE mf_client " +
                                 $"SET code = '{clientCode}' " +
                                 $"WHERE id = {clientId}";

            // Execute the update query
            SqlExecuterItem updateResponse = SqlExecuter.ExecuteFunction(updateQuery, [], executeUser, false, "updateClientCode");

            if (!updateResponse.operationResult)
            {
                Log.Error($"Error updating client code for client ID: {clientId}");
            }
            else
            {
                Log.Info($"Successfully updated client code for client ID: {clientId}");
            }

        }
    }
}
