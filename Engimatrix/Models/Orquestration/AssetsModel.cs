// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.ModelObjs;
using engimatrix.ModelObjs.Orquestration;
using engimatrix.Utils;
using static engimatrix.Views.Orquestration.AssetsRequest;

namespace engimatrix.Models.Orquestration
{
    public static class AssetsModel
    {
        public static List<AssetsItem> GetAssets(string language, string user_operation)
        {
            List<AssetsItem> result = [];

            Dictionary<string, string> param = [];

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT id, description, type, user, password, text FROM assets_script", param, user_operation, false, "Get All Assets");
            AssetsItem rec = null;

            foreach (Dictionary<string, string> item in responsive.out_data)
            {
                int id = Convert.ToInt32(item["0"]);
                string description = item["1"];
                string type = item["2"];
                string user = item["3"];
                string password = item["4"];
                string text = item["5"];

                if (ConfigManager.isProduction)
                {
                    type = Cryptography.Decrypt(type, ConfigManager.generalKey);
                    user = Cryptography.Decrypt(user, ConfigManager.generalKey);
                    password = Cryptography.Decrypt(password, ConfigManager.generalKey);
                    text = Cryptography.Decrypt(text, ConfigManager.generalKey);
                }

                rec = new AssetsItem(id, description, type, user, password, text);
                result.Add(rec.ToItem());
            }

            return result;
        }

        public static AssetsItem GetAssetByName(string assetName, string user_operation)
        {
            Dictionary<string, string> param = new()
            {
                { "assetName", assetName }
            };

            // Query to fetch the asset by name
            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT ID, description, type, user, password, text FROM assets_script WHERE description = @assetName", param, user_operation, false, "Get Text Asset By Name");

            if (responsive.out_data.Count == 0)
            {
                throw new FileNotFoundException("Asset not found");
            }

            Dictionary<string, string> item = responsive.out_data[0];

            int id = Convert.ToInt32(item["0"]);
            string description = item["1"];
            string type = item["2"];
            string user = item["3"];
            string password = item["4"];
            string text = item["5"];

            if (ConfigManager.isProduction)
            {
                type = Cryptography.Decrypt(type, ConfigManager.generalKey);
                user = Cryptography.Decrypt(user, ConfigManager.generalKey);
                password = Cryptography.Decrypt(password, ConfigManager.generalKey);
                text = Cryptography.Decrypt(text, ConfigManager.generalKey);
            }

            return new AssetsItem(id, description, type, user, password, text);
        }

        public static bool remove(string id, string user_operation)
        {
            Dictionary<string, string> param = new()
            {
                { "@id", id }
            };

            SqlExecuter.ExecFunction("DELETE FROM assets_script WHERE id = @id", param, user_operation, true, "Remove Asset");

            Log.Debug("Asset " + id + " removed successfully.");

            return true;
        }

        public static bool edit(Edit input, string user_operation)
        {
            if (ConfigManager.isProduction)
            {
                input.text = Cryptography.Encrypt(input.text, ConfigManager.generalKey);
                input.user = Cryptography.Encrypt(input.user, ConfigManager.generalKey);
                input.password = Cryptography.Encrypt(input.password, ConfigManager.generalKey);
            }

            Dictionary<string, string> param = new()
            {
                { "@id", input.id },
                { "@text", input.text },
                { "@user", input.user },
                { "@password", input.password }
            };

            SqlExecuter.ExecFunction("UPDATE assets_script SET text = @text, user = @user, password = @password WHERE id = @id", param, user_operation, true, "Edit Asset");

            Log.Debug("Asset " + input.id + " edited successfully.");

            return true;
        }

        public static bool Add(Add input, string user_operation)
        {
            if (ConfigManager.isProduction)
            {
                input.type = Cryptography.Encrypt(input.type, ConfigManager.generalKey);
                input.text = Cryptography.Encrypt(input.text, ConfigManager.generalKey);
                input.user = Cryptography.Encrypt(input.user, ConfigManager.generalKey);
                input.password = Cryptography.Encrypt(input.password, ConfigManager.generalKey);
            }

            Dictionary<string, string> param = new()
            {
                { "@description", input.description },
                { "@type", input.type },
                { "@text", input.text },
                { "@user", input.user },
                { "@password", input.password }
            };

            SqlExecuter.ExecFunction("INSERT INTO assets_script (description, type, text, user, password) VALUES (@description, @type, @text, @user, @password)", param, user_operation, true, "Add Asset");

            Log.Debug("Asset " + input.description + " added successfully.");

            return true;
        }
    }

}


