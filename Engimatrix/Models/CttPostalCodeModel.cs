// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Diagnostics;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.CTT;
using engimatrix.Utils;

namespace engimatrix.Models;

public static class CttPostalCodeModel
{
    public static List<CttPostalCodeDto> GetAllDto(string executeUser, string? cc, string? dd, string? cp4, string? cp3)
    {
        Dictionary<string, string> dic = [];

        string query = "SELECT id AS id, pc.dd AS dd, pc.cc AS cc, llll, localidade, art_cod, art_tipo, pri_prep, art_titulo, seg_prep, art_desig, art_local, troco, porta, cliente, cp4, cp3, cpalf, " +
            "mun.cc AS mun_cc, dis.dd AS mun_dd, dis.name AS dis_name, mun.name AS mun_name " +
            "FROM ctt_postal_code pc " +
            "JOIN ctt_municipality mun ON mun.cc = pc.cc AND mun.dd = pc.dd " +
            "JOIN ctt_district dis ON dis.dd = pc.dd " +
            "WHERE 1 = 1 ";

        if (!string.IsNullOrEmpty(cc))
        {
            dic.Add("cc", cc);
            query += "AND pc.cc = @cc ";
        }
        if (!string.IsNullOrEmpty(dd))
        {
            dic.Add("dd", dd);
            query += "AND pc.dd = @dd ";
        }
        if (!string.IsNullOrEmpty(cp4))
        {
            dic.Add("cp4", cp4);
            query += "AND pc.cp4 = @cp4 ";
        }
        if (!string.IsNullOrEmpty(cp3))
        {
            dic.Add("cp3", cp3);
            query += "AND pc.cp3 = @cp3 ";
        }

        SqlExecuterItem result = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "GetAllCttPostalCodeDto");

        if (!result.operationResult)
        {
            throw new DatabaseException("Error fetching the ctt postal codes dtos from the database");
        }

        if (result.out_data.Count == 0)
        {
            throw new ResourceEmptyException("No ctt postal codes dtos found in the database");
        }

        List<CttPostalCodeDto> postalCodes = [];
        foreach (Dictionary<string, string> item in result.out_data)
        {
            CttDistrictItem district = new(item["dd"], item["dis_name"]);
            CttMunicipalityDto municipality = new(item["cc"], district, item["mun_name"]);
            CttPostalCodeDto postalCode = new CttPostalCodeDtoBuilder()
                .SetId(int.Parse(item["id"]))
                .SetMunicipality(municipality)
                .SetLlll(item["llll"])
                .SetLocalidade(item["localidade"])
                .SetArtCod(item["art_cod"])
                .SetArtTipo(item["art_tipo"])
                .SetPriPrep(item["pri_prep"])
                .SetArtTitulo(item["art_titulo"])
                .SetSegPrep(item["seg_prep"])
                .SetArtDesig(item["art_desig"])
                .SetArtLocal(item["art_local"])
                .SetTroco(item["troco"])
                .SetPorta(item["porta"])
                .SetCliente(item["cliente"])
                .SetCp4(item["cp4"])
                .SetCp3(item["cp3"])
                .SetCpalf(item["cpalf"])
                .Build();

            postalCodes.Add(postalCode);
        }

        return postalCodes;
    }

    public static List<CttPostalCodeItem> GetAll(string executeUser, string? cc, string? dd, string? cp4, string? cp3)
    {
        Dictionary<string, string> dic = [];

        string query = "SELECT id AS id, dd AS dd, cc AS cc, llll, localidade, art_cod, art_tipo, " +
            "pri_prep, art_titulo, seg_prep, art_desig, art_local, troco, porta, cliente, cp4, cp3, cpalf " +
            "FROM ctt_postal_code pc " +
            "WHERE 1 = 1 ";

        if (!string.IsNullOrEmpty(cc))
        {
            dic.Add("cc", cc);
            query += "AND cc = @cc ";
        }
        if (!string.IsNullOrEmpty(dd))
        {
            dic.Add("dd", dd);
            query += "AND dd = @dd ";
        }
        if (!string.IsNullOrEmpty(cp4))
        {
            dic.Add("cp4", cp4);
            query += "AND cp4 = @cp4 ";
        }
        if (!string.IsNullOrEmpty(cp3))
        {
            dic.Add("cp3", cp3);
            query += "AND cp3 = @cp3 ";
        }

        SqlExecuterItem result = SqlExecuter.ExecuteFunction(query, dic, executeUser, false, "GetAllCttPostalCodeDto");

        if (!result.operationResult)
        {
            throw new DatabaseException("Error fetching the ctt postal codes dtos from the database");
        }

        if (result.out_data.Count == 0)
        {
            return [];
        }

        List<CttPostalCodeItem> postalCodes = [];
        foreach (Dictionary<string, string> item in result.out_data)
        {
            CttPostalCodeItem postalCode = new CttPostalCodeItemBuilder()
                .SetId(int.Parse(item["id"]))
                .SetDd(item["dd"])
                .SetCc(item["cc"])
                .SetLlll(item["llll"])
                .SetLocalidade(item["localidade"])
                .SetArtCod(item["art_cod"])
                .SetArtTipo(item["art_tipo"])
                .SetPriPrep(item["pri_prep"])
                .SetArtTitulo(item["art_titulo"])
                .SetSegPrep(item["seg_prep"])
                .SetArtDesig(item["art_desig"])
                .SetArtLocal(item["art_local"])
                .SetTroco(item["troco"])
                .SetPorta(item["porta"])
                .SetCliente(item["cliente"])
                .SetCp4(item["cp4"])
                .SetCp3(item["cp3"])
                .SetCpalf(item["cpalf"])
                .Build();

            postalCodes.Add(postalCode);
        }

        return postalCodes;
    }
}
