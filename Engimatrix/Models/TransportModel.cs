// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using engimatrix.Config;
using engimatrix.Connector;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.Utils;
using Engimatrix.ModelObjs;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.CallRecords;
using Microsoft.IdentityModel.Tokens;

namespace engimatrix.Models;

public static class TransportModel
{
    public static List<TransportItem> GetTranports(string execute_user)
    {
        string query = "SELECT * FROM transport";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, [], execute_user, false, "GetAllTransports");

        if (!response.operationResult)
        {
            throw new Exception("Error getting transports");
        }

        if (response.out_data.Count == 0)
        {
            return [];
        }

        List<TransportItem> transports = [];
        foreach (Dictionary<string, string> item in response.out_data)
        {
            TransportItem transport = new TransportItemBuilder()
                .SetId(Int32.Parse(item["id"]))
                .SetName(item["name"])
                .SetSlug(item["slug"])
                .SetDescription(item["description"])
                .Build();

            transports.Add(transport);
        }

        return transports;
    }

    public static TransportItem? GetTransportById(int id, string execute_user)
    {
        Dictionary<string, string> dic = new()
        {
            { "@Id", id.ToString() }
        };

        string query = "SELECT * FROM transport WHERE id = @id";

        SqlExecuterItem response = SqlExecuter.ExecuteFunction(query, dic, execute_user, false, "GetTransportById");

        if (!response.operationResult)
        {
            throw new Exception("Error getting transport");
        }

        if (response.out_data.Count == 0)
        {
            return null;
        }

        Dictionary<string, string> item = response.out_data.First();

        TransportItem transport = new TransportItemBuilder()
            .SetId(Int32.Parse(item["id"]))
            .SetName(item["name"])
            .SetSlug(item["slug"])
            .SetDescription(item["description"])
            .Build();

        return transport;
    }
}
