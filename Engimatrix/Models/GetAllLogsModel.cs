// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.RepositoryRecords;

namespace engimatrix.Models
{
    public class GetAllLogsModel
    {
        public static List<GetAllLogsItem> GetAllLogs(string userFilterEmail)
        {
            List<GetAllLogsItem> result = new List<GetAllLogsItem>();
            Dictionary<string, string> dic = new Dictionary<string, string>();

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT * FROM logs", dic, null, true, "GetAllLogs");

            GetAllLogsDBRRecord GetAllLogsRec = null;

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                string userId = item["0"];
                string userEmail = item["1"];
                string userName = item["2"];
                string userRoleId = item["4"];
                string activeSince = item["5"];

                GetAllLogsRec = new GetAllLogsDBRRecord(userId, userEmail, userName, userRoleId);
                result.Add(GetAllLogsRec.ToGetAllLogsItem());
            }

            return result;
        }
    }
}
