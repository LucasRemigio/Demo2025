// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Config;
using engimatrix.ModelObjs;
using engimatrix.Utils;

namespace engimatrix.Models
{
    public static class SignatureModel
    {
        public static SignatureItem GetSignature(string execute_user)
        {
            string userId = UserModel.getUserIdByEmail(execute_user);

            Dictionary<string, string> dic = new()
            {
                { "user_id", userId }
            };

            string query = "SELECT signature FROM signature WHERE user_id = @user_id";
            SqlExecuterItem response = SqlExecuter.ExecFunction(query, dic, execute_user, false, "GetRepliesByToken");

            // User has never saved any signature. Lets save the default one
            if (response.out_data.Count <= 0)
            {
                string defaultSignature = GetDefaultFormattedSignature(execute_user);
                SaveSignature(defaultSignature, execute_user, execute_user);
                return new SignatureItem(userId, defaultSignature);
            }

            string signature = response.out_data[0]["0"];
            if (ConfigManager.isProduction)
            {
                signature = Cryptography.Decrypt(signature, userId);
            }

            return new SignatureItem(userId, signature);
        }

        public static void SaveSignature(string signature, string userEmail, string execute_user)
        {
            string userId = UserModel.getUserIdByEmail(userEmail);

            Dictionary<string, string> dic = [];
            if (ConfigManager.isProduction)
            {
                signature = Cryptography.Encrypt(signature, userId);
            }
            dic.Add("user_id", userId);
            dic.Add("signature", signature);

            SqlExecuter.ExecFunction("INSERT INTO signature (`user_id`, `signature`) VALUES (@user_id, @signature)", dic, execute_user, false, "InsertUserSignatureRecord");
        }

        public static void PatchSignature(string signature, string execute_user)
        {
            string userId = UserModel.getUserIdByEmail(execute_user);

            Dictionary<string, string> dic = [];
            if (ConfigManager.isProduction)
            {
                signature = Cryptography.Encrypt(signature, userId);
            }
            dic.Add("user_id", userId);
            dic.Add("signature", signature);

            SqlExecuter.ExecFunction("UPDATE signature SET signature = @signature WHERE user_id = @user_id", dic, execute_user, false, "InsertUserSignatureRecord");
        }

        public static void DeleteSignature(string userEmail, string executer_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string userId = UserModel.getUserIdByEmail(userEmail);
            dic.Add("userId", userId);

            SqlExecuter.ExecFunction("DELETE FROM signature WHERE user_id = @userId", dic, executer_user, false, "DeleteUserDepartments");
        }

        public static string GetDefaultFormattedSignature(string? execute_user)
        {
            string senderName = "Equipa da Masterferro";
            if (!string.IsNullOrEmpty(execute_user))
            {
                senderName = UserModel.GetUserNameByEmail(execute_user);
            }

            string greeting = $@" 
            <p>
                <strong>{senderName}</strong><br>
                <a href=""http://engibots.com/"">engibots.com</a><br><br>

                <strong>E:</strong>
                <a href=""mailto:ricardo.silva@engibots.com"">ricardo.silva@engibots.com</a><br>

                <strong>T:</strong> (+351) 213 303 720<br>
                <strong>M:</strong> (+351) 919 817 211<br><br>

                <strong>A:</strong><br>
                Av.D.João II, Lote 1.07.2.1, Piso 0<br>
                Parque das Nações<br>
                1990 - 096 Lisboa<br><br>

                <a href=""https://www.linkedin.com/company/engibots/"">LinkedIn · engibots</a>

                <div>
                    <img src='https://engibots.github.io/ImageHost/Engibots/signature.png' alt='Logo' style='width: 100%; height: auto; display: block;' />
                </div>
            </p> ";

            return greeting;
        }

        public static string GetHappyChristmasGif(string execute_user)
        {
            string greeting = @"
                <div style='max-width: 200px;'>
                    <img src='https://engibots.github.io/ImageHost/MasterferroHappyChristmas-resize.gif' alt='Logo' style='width: 100%; height: auto; display: block;' />
                </div> ";

            return greeting;
        }
    }
}
