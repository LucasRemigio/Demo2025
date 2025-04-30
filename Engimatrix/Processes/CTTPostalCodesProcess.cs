// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text;
using engimatrix.Models;
using engimatrix.Utils;
using Exception = System.Exception;
using System.Diagnostics;


namespace engimatrix.Processes;
public static class CttPostalCodesProcess
{
    public static bool UpdatePostalCodes(string executeUser)
    {
        try
        {
            Log.Info("UpdatePostalCodes - Starting Engimatrix Postal Codes Update");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Stopwatch stopwatch = new();
            stopwatch.Start();

            //curent directory, on ctt folder
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = Path.Combine(userProfile, "Downloads", "PostalCodes");
            Log.Debug(filePath);

            string districtsPath = Path.Combine(filePath, "distritos.txt");
            string concelhosPath = Path.Combine(filePath, "concelhos.txt");
            string codesPath = Path.Combine(filePath, "todos_cp.txt");

            string[] requiredFiles = [districtsPath, concelhosPath, codesPath];
            foreach (string fileName in requiredFiles)
            {
                if (!File.Exists(fileName))
                {
                    Log.Error($"Update Postal Codes - File not found: {fileName}");
                    return false;
                }
            }

            DeleteRegistries();
            TimeIt(stopwatch, "Delete Registries");

            //Districts
            List<string[]> districts = LoadDistricts(districtsPath);
            InsertDistricts(districts);
            TimeIt(stopwatch, "Update Districts");

            //municipalitys
            List<string[]> municipalities = LoadMunicipalities(concelhosPath);
            InsertMunicipalities(municipalities);
            TimeIt(stopwatch, "Update municipalitys");

            //Postal Codes
            List<string[]> postalCodes = LoadPostalCodes(codesPath);
            InsertPostalCodes(postalCodes);
            TimeIt(stopwatch, "Update Postal Codes");

            string query = "INSERT INTO ctt_postal_code_update VALUES ()";
            SqlExecuter.ExecFunction(query, [], "system", true, "Save Postal Code Refresh Event");

            Log.Info("Update Postal Codes - Completed successfully");
        }
        catch (Exception e)
        {
            Log.Error("Update Postal Codes - Something went wrong - " + e);
            return false;
        }
        return true;
    }

    private static void TimeIt(Stopwatch stopwatch, string type)
    {
        stopwatch.Stop();
        Log.Debug($"CTT - {type} in {stopwatch.ElapsedMilliseconds} ms");
        stopwatch.Restart();
    }

    private static void DeleteRegistries()
    {
        //Delete registries
        string query = "TRUNCATE TABLE ctt_postal_code;";
        SqlExecuter.ExecFunction(query, [], "system", true, "Deleting Previous Postal Codes");

        query = "DELETE FROM ctt_municipality WHERE CC > -1;";
        SqlExecuter.ExecFunction(query, [], "system", true, "Deleting Previous municipalitys");

        query = "DELETE FROM ctt_district WHERE DD > -1;";
        SqlExecuter.ExecFunction(query, [], "system", true, "Deleting Previous Districts");
    }

    private static List<string[]> LoadPostalCodes(string filePath)
    {
        // Expecting 17 fields per line for postal codes.
        return LoadFile(filePath, 17, "Update Postal Codes");
    }

    private static List<string[]> LoadDistricts(string filePath)
    {
        // Expecting 2 fields per line for districts.
        return LoadFile(filePath, 2, "Update Districts");
    }

    private static List<string[]> LoadMunicipalities(string filePath)
    {
        // Expecting 3 fields per line for municipalitys.
        return LoadFile(filePath, 3, "Update municipalitys");
    }

    private static List<string[]> LoadFile(string filePath, int expectedFieldCount, string logPrefix)
    {
        List<string[]> records = [];

        try
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.GetEncoding(1252));

            foreach (string line in lines)
            {
                string[] fields = line.Split(';');

                if (fields.Length != expectedFieldCount)
                {
                    Log.Error($"{logPrefix} - Invalid line format: {line}");
                    continue;
                }

                records.Add(fields);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"{logPrefix} - Something went wrong - {ex}");
        }

        return records;
    }

    private static void InsertDistricts(List<string[]> districts)
    {
        StringBuilder queryBuilder = new();
        queryBuilder.Append($"INSERT INTO ctt_district (DD, NAME) VALUES ");

        foreach (string[] fields in districts)
        {
            if (string.IsNullOrEmpty(fields[0]) || string.IsNullOrEmpty(fields[1]))
            {
                Log.Error($"Insert Districts - Invalid fields: {fields[0]} - {fields[1]}");
                continue;
            }

            queryBuilder.Append($" ('{fields[0]}', '{fields[1]}')");

            if (fields != districts.Last())
            {
                queryBuilder.Append(',');
            }
        }

        string query = queryBuilder.ToString();
        SqlExecuter.ExecFunction(query, [], "system", true, "Refresh All Districts");
    }

    private static void InsertMunicipalities(List<string[]> municipalities)
    {
        StringBuilder queryBuilder = new();
        queryBuilder.Append("INSERT INTO ctt_municipality (DD, CC, NAME) VALUES ");

        foreach (string[] fields in municipalities)
        {
            // if any of the field is empty, skip the record
            if (string.IsNullOrEmpty(fields[0]) || string.IsNullOrEmpty(fields[1]) || string.IsNullOrEmpty(fields[2]))
            {
                Log.Error($"Insert municipalitys - Invalid fields: {fields[0]} - {fields[1]} - {fields[2]}");
                continue;
            }

            queryBuilder.Append($" ('{fields[0]}', '{fields[1]}', '{fields[2]}')");

            // add a final comma if not the last
            if (fields != municipalities.Last())
            {
                queryBuilder.Append(',');
            }
        }

        string query = queryBuilder.ToString();
        SqlExecuter.ExecFunction(query, [], "system", true, "Refresh All municipalitys");
    }

    private static void InsertPostalCodes(List<string[]> postalCodes)
    {
        const int batchSize = 50000;
        int totalCount = postalCodes.Count;
        Log.Debug(totalCount.ToString());

        List<string> queries = [];

        // Process by batch using an index-based loop to avoid LINQ overhead.
        for (int index = 0; index < totalCount; index += batchSize)
        {
            int currentBatchSize = Math.Min(batchSize, totalCount - index);

            // Estimate a capacity (adjust as needed) to reduce reallocations.
            StringBuilder insertQueryBuilder = new(currentBatchSize * 1000);

            // List of fields; consider declaring this as a static readonly member if it doesn't change.
            string[] insertFields =
            [
                "dd", "cc", "llll", "localidade", "art_cod", "art_tipo",
                "pri_prep", "art_titulo", "seg_prep", "art_desig", "art_local", "troco",
                "porta", "cliente", "cp4", "cp3", "cpalf"
            ];

            insertQueryBuilder.Append("INSERT INTO ctt_postal_code (");
            insertQueryBuilder.Append(string.Join(", ", insertFields));
            insertQueryBuilder.Append(") VALUES ");

            // Loop over the current batch using indices
            for (int i = index; i < index + currentBatchSize; i++)
            {
                string[] fields = postalCodes[i];
                insertQueryBuilder.Append('(');

                // Use index-based loop for fields
                for (int j = 0; j < fields.Length; j++)
                {
                    // Get the field, treat null as empty string
                    string field = fields[j] ?? string.Empty;

                    // Replace single quotes with doubled quotes for SQL escaping if necessary
                    if (field.Contains('\''))
                    {
                        field = field.Replace("'", "''");
                    }

                    // Append the field wrapped in single quotes
                    insertQueryBuilder.Append('\'').Append(field).Append('\'');

                    if (j < fields.Length - 1)
                    {
                        insertQueryBuilder.Append(", ");
                    }
                }

                insertQueryBuilder.Append(')');

                // Append a comma unless it's the last row in the current batch
                if (i < index + currentBatchSize - 1)
                {
                    insertQueryBuilder.Append(", ");
                }
            }

            insertQueryBuilder.Append(';');

            string query = insertQueryBuilder.ToString();
            queries.Add(query);
        }

        Parallel.ForEach(queries, query =>
        {
            SqlExecuter.ExecFunction(query, [], "system", false, "Refresh All Postal Codes");
        });
    }
}
