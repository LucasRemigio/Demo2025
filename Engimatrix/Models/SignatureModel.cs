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

            string masterFerroLocation = "https://maps.app.goo.gl/Mpkjg8S5qrVTd3Gs6";

            string greeting = $@"
                <p>{senderName}</p>
                <div style='max-width: 200px;'>
                    <img src='https://engibots.github.io/ImageHost/masterferro_logo.png' alt='Logo' style='width: 100%; height: auto; display: block;' />
                </div>
                <p>| Master Ferro, Lda |</p>
                <p>| <a href='mailto:{execute_user}'>{execute_user}</a> | <a href='http://www.masterferro.pt'>www.masterferro.pt</a> |</p>
                <p>| Chamada para a rede fixa / m&oacute;vel nacional |</p>
                <p>| <a href='{masterFerroLocation}'>Av. Barros e Soares, 531 | 4715-213 Braga | Portugal</a> |</p>
                <div>
                    <b>Aviso de Confidencialidade</b>
                    <p>Este e-mail e quaisquer ficheiros inform&aacute;ticos com ele transmitidos s&atilde;o confidenciais e destinados ao conhecimento e uso exclusivo do respetivo destinat&aacute;rio, n&atilde;o podendo o conte&uacute;do dos mesmos ser alterado. Caso tenha recebido este e-mail indevidamente, queira informar de imediato o remetente e proceder &agrave; elimina&ccedil;&atilde;o da mensagem. O correio eletr&oacute;nico n&atilde;o garante a confidencialidade dos conte&uacute;dos das mensagens, nem a rece&ccedil;&atilde;o adequada dos mesmos. Caso o destinat&aacute;rio deste e-mail tenha qualquer obje&ccedil;&atilde;o &agrave; utiliza&ccedil;&atilde;o deste meio dever&aacute; contactar de imediato o remetente.</p>
                </div>
                <br><br><br>";

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
