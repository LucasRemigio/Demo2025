// // Copyright (c) 2024 Engibots. All rights reserved.

using Microsoft.VisualBasic.FileIO;
using engimatrix.Config;
using engimatrix.ModelObjs;
using engimatrix.Models;

namespace engimatrix.Utils
{
    public static class Data
    {
        // criar duas estruturas de dados
        private static Dictionary<string, int> score = new Dictionary<string, int>();

        private static string fileScorePath = "Data/score.csv";

        private static int confidenceThreashold = 40;
        private static int confidenceIncrease = 10;
        private static int confidenceDecrease = 20;

        public static void LoadData()
        {
            LoadScore();
        }

        private static void LoadScore()
        {
            score = new Dictionary<string, int>();

            if (!File.Exists(fileScorePath))
            {
                return;
            }

            using (TextFieldParser parser = new TextFieldParser(fileScorePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadFields(); // skill headers

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields.Length > 0)
                    {
                        score.Add(fields[0], int.Parse(fields[1]));
                    }
                }
            }
        }

        private static void SaveScore()
        {
            string filePath = fileScorePath;

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine("email,score");

                foreach (KeyValuePair<string, int> data in score)
                {
                    writer.WriteLine(data.Key + "," + data.Value.ToString());
                }
            }
        }

        public static void SaveData()
        {
            SaveScore();
        }

        //public static bool IsExclusion(string email, string id, string vat)
        //{
        //    foreach (ClientExclusionItem exclusion in ClientModel.GetAlLExclusionClients(ConfigManager.defaultLanguage, "system"))
        //    {
        //        if (String.IsNullOrEmpty(exclusion.client_id))
        //            exclusion.client_id = "none";

        //        if (String.IsNullOrEmpty(exclusion.client_email))
        //            exclusion.client_email = "none";

        //        if (String.IsNullOrEmpty(exclusion.client_vat))
        //            exclusion.client_vat = "none";

        //        if (
        //            email.ToLower().Trim().Contains(exclusion.client_email.ToLower().Trim()) ||
        //            id.ToLower().Trim().Equals(exclusion.client_id.ToLower().Trim()) ||
        //            vat.ToLower().Trim().Equals(exclusion.client_vat.ToLower().Trim())
        //            )
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        public static bool HasConfidence(string email)
        {
            if (!score.ContainsKey(email))
            {
                score.Add(email, 0);
                return false;
            }

            if (score[email] >= confidenceThreashold)
            {
                return true;
            }

            return false;
        }

        public static void IncreaseConfidence(string email)
        {
            if (!score.ContainsKey(email))
            {
                score.Add(email, 0);
                SaveScore();
                return;
            }

            if (score[email] + confidenceIncrease <= 100)
            {
                score[email] = score[email] + confidenceIncrease;
            }

            SaveScore();
        }

        public static void DecreaseConfidence(string email)
        {
            if (!score.ContainsKey(email))
            {
                score.Add(email, 0);
                SaveScore();
                return;
            }

            if (score[email] - confidenceDecrease >= 0)
            {
                score[email] = score[email] - confidenceDecrease;
            }

            SaveScore();
        }
    }
}
