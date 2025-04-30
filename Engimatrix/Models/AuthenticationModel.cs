// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text;
using engimatrix.Config;
using engimatrix.Emails;
using engimatrix.Exceptions;
using engimatrix.ModelObjs;
using engimatrix.RepositoryRecords;
using engimatrix.Utils;
using engimatrix.Views;
using Engimatrix.ModelObjs;

namespace engimatrix.Models
{
    public class AuthenticationModel
    {
        public static bool RegisterUser(SignUpRequest authReq, string language)
        {
            string regToken = authReq.name.GetHashCode().ToString() + authReq.email.GetHashCode() + (DateTimeOffset.UtcNow).ToUnixTimeMilliseconds();
            Log.Debug("[RegisterUser] " + authReq.name + " " + authReq.email + " Token:" + regToken);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@email", authReq.email);
            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user_pending WHERE user_email=@email", dic, authReq.email, false, "RegisterUser");

            string pendingUserExists = responsive.out_data[0]["0"];

            if (pendingUserExists != "1")
            {
                throw new UserNotFoundException();
            }

            // Validate if user is already registred

            dic = new Dictionary<string, string>();
            dic.Add("@email", authReq.email);
            SqlExecuterItem responsive1 = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email=@email", dic, authReq.email, false, "RegisterUser");

            string UserExists = responsive1.out_data[0]["0"];

            if (UserExists != "1")
            {
                throw new UserNotFoundException();
            }

            // Add to pending users table and sends registration confirmation email

            dic = new Dictionary<string, string>();
            dic.Add("@user_name", authReq.name);
            dic.Add("@user_email", authReq.email);
            dic.Add("@user_password", Cryptography.Encrypt(authReq.password, authReq.email));
            dic.Add("@registry_time", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("@num_of_contacts", "1");
            dic.Add("@last_contact", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("@language", language);
            dic.Add("@registry_id", regToken);
            SqlExecuterItem responsive2 = SqlExecuter.ExecFunction(
                "INSERT INTO user_pending (registry_id, user_name, user_email, user_password, registry_time, num_of_contacts, last_contact, language)  VALUES " +
            "(@registry_id, @user_name, @user_email, @user_password, @registry_time, @num_of_contacts, @last_contact, @language)", dic, authReq.email, true, "RegisterUser");
            if (ConfigManager.isProduction)
            {
                authReq.name = Cryptography.Encrypt(authReq.name, authReq.email);
            }
            try
            {
                EmailService.SendSignUpEmail(regToken, authReq.name, authReq.email, language);
            }
            catch (Exception e)
            {
                Log.Error("Error - " + e.Message);
                return false;
            }

            return true;
        }

        internal static void ForgotPass(ForgotPassRequest input, string language)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@email", input.email);

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email=@email", dic, input.email, false, "ForgotPass");

            string userExists = responsive.out_data[0]["0"];

            if (userExists == "1")
            {
                DefineAndSendUserRecoverPasswordToken(input.email);
                return;
            }
        }

        internal static void ResetPass(ResetPassRequest input, bool performTokenValidation, string executer_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (performTokenValidation)
            {
                dic = new Dictionary<string, string>();
                dic.Add("@email", input.email);
                dic.Add("@token", input.token);

                SqlExecuterItem responsive6 = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM reset_pass_token WHERE email=@email and token=@token", dic, executer_user, false, "ResetPass");

                string entriesFound = responsive6.out_data[0]["0"];

                if (entriesFound != "1")
                {
                    throw new UserNotFoundException();
                }
                dic = new Dictionary<string, string>();
                dic.Add("@email", input.email);

                SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT * FROM reset_pass_token WHERE email=@email", dic, executer_user, true, "ResetPass");

                string expirationTime = responsive.out_data[0]["2"];
                DateTime dateTime = DateTime.Parse(expirationTime);

                if (dateTime < DateTime.UtcNow)
                {
                    throw new InvalidArgsResetPassException();
                }
            }

            dic = new Dictionary<string, string>();
            dic.Add("@email", input.email);

            SqlExecuterItem responsive1 = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email=@email", dic, executer_user, false, "ResetPass");

            string usersFound = responsive1.out_data[0]["0"];

            if (usersFound != "1")
            {
                throw new UserNotFoundException();
            }

            List<string> passHistoryArr = null;
            string userPass = Encoding.UTF8.GetString(Convert.FromBase64String(input.password));
            string userName = string.Empty;

            dic = new Dictionary<string, string>();
            dic.Add("@email", input.email);

            SqlExecuterItem responsive2 = SqlExecuter.ExecFunction("SELECT * FROM user WHERE user_email=@email", dic, executer_user, true, "ResetPass");

            userName = responsive2.out_data[0]["2"];
            string passHistory = responsive2.out_data[0]["10"];

            if (ConfigManager.isProduction)
            {
                passHistory = Cryptography.Decrypt(passHistory, input.email);
                userName = Cryptography.Decrypt(userName, input.email);
            }

            passHistoryArr = new List<string>(passHistory.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
            foreach (string pass in passHistoryArr)
            {
                if (pass.Equals(userPass))
                {
                    throw new EqualsRecentHistoryPassException();
                }
            }

            string newPassHistory = string.Empty;
            if (passHistoryArr != null)
            {
                if (passHistoryArr.Count >= 10)
                {
                    passHistoryArr.RemoveAt(0);
                }

                if (passHistoryArr.Count == 0)
                {
                    newPassHistory = userPass;
                }
                else
                {
                    newPassHistory = String.Join("|", passHistoryArr) + "|" + userPass;
                }
            }

            dic = new Dictionary<string, string>();
            dic.Add("@email", input.email);
            dic.Add("@user_password", Cryptography.Encrypt(userPass, input.email));
            dic.Add("@last_pass_change", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("@pass_history", Cryptography.Encrypt(newPassHistory, input.email));

            SqlExecuter.ExecFunction("UPDATE user SET user_password=@user_password, last_pass_change=@last_pass_change, pass_history=@pass_history WHERE user_email=@email", dic, executer_user, true, "ResetPass");

            // delete used token

            dic = new Dictionary<string, string>();
            dic.Add("@email", input.email);
            dic.Add("@token", input.token);

            SqlExecuter.ExecFunction("DELETE FROM reset_pass_token WHERE email=@email and token=@token", dic, executer_user, true, "ResetPass");

            EmailService.ResetPasswordConfirmationEmail(userName, input.email, "en");
        }

        public static UserSimple LoginUser(SignInRequest authReq, string lang)
        {
            UserDBRecord userRec = null;
            bool userPasswordExpired = false;

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@email", authReq.email);
            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email = @email", dic, authReq.email, false, "LoginUser");

            string usersFound = responsive.out_data[0]["0"];

            if (usersFound != "1")
            {
                throw new UserNotFoundException();
            }

            dic = new Dictionary<string, string>();
            dic.Add("@email", authReq.email);
            responsive = SqlExecuter.ExecFunction("SELECT * FROM user WHERE user_email=@email", dic, authReq.email, true, "LoginUser");

            string userId = responsive.out_data[0]["0"];
            string userEmail = responsive.out_data[0]["1"];
            string userName = responsive.out_data[0]["2"];
            string password = responsive.out_data[0]["3"];
            string userRoleId = responsive.out_data[0]["4"];
            string activeSince = responsive.out_data[0]["5"];
            string lastLogin = responsive.out_data[0]["6"];
            string language = responsive.out_data[0]["7"];
            string passExpires = responsive.out_data[0]["8"];
            string lastPassChange = responsive.out_data[0]["9"];
            string passHistory = responsive.out_data[0]["10"];

            if (ConfigManager.isProduction)
            {
                userName = Cryptography.Decrypt(userName, userEmail);
                passHistory = Cryptography.Decrypt(passHistory, userEmail);
                password = Cryptography.Decrypt(password, userEmail);
            }

            userRec = new UserDBRecord(userId, userEmail, userName, password, userRoleId, activeSince, lastLogin, language, passExpires, lastPassChange, passHistory);

            // In prod, the clean password is encrypted, and we receive the base64 on the frontend
            if (ConfigManager.isProduction)
            {
                // So we need to convert the frontend password from base64 and compare with the clean decrypted DB password
                string userPass = Encoding.UTF8.GetString(Convert.FromBase64String(authReq.password));
                if (!userRec.UserPassword.Equals(userPass))
                {
                    throw new InvalidLoginException();
                }
            }
            // Both userRec.UserPassword (DB) and authReq.password (frontend) are in Base64, so we just compare them
            else if (!userRec.UserPassword.Equals(authReq.password))
            {
                throw new InvalidLoginException();
            }

            if (passExpires == "1" && !string.IsNullOrEmpty(lastPassChange))
            {
                int passwordExpirationDays = ConfigManager.PasswordExpirationPolicyDays();

                DateTime lastPassChangeDate = DateTime.Parse(lastPassChange);

                if ((DateTime.UtcNow - lastPassChangeDate).TotalDays > passwordExpirationDays)
                {
                    userPasswordExpired = true;
                    ForgotPass(new ForgotPassRequest { email = authReq.email }, language);
                    throw new PasswordExpiredException();
                }
            }

            //if (lastLogin == "null" || userRoleId == "3")
            //{
            //    dic = new Dictionary<string, string>();
            //    dic.Add("@email", authReq.email);
            //    responsive = SqlExecuter.ExecFunction("SELECT state FROM client WHERE email =@email", dic, authReq.email, false, "GetClientState");

            //    string state = responsive.out_data[0]["0"];
            //    if (state== "0")
            //        throw new InvalidLoginException();
            //}

            dic = new Dictionary<string, string>();
            dic.Add("@last_login", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("@email", authReq.email);
            SqlExecuterItem responsive2 = SqlExecuter.ExecFunction("UPDATE user SET last_login=@last_login WHERE user_email=@email", dic, authReq.email, true, "LoginUser");

            List<DepartmentItem> departments = DepartmentModel.GetUserDepartments(userEmail, userEmail);

            return new UserSimple(userRec.UserName, Cryptography.GenerateJwtToken(userRec), UserModel.GetUserRole(userRec.UserRoleId), userPasswordExpired, departments);
        }

        public static UserSimple LoginUserWithMicrosoft(SignInRequest authReq, string lang)
        {
            UserDBRecord userRec = null;
            bool userPasswordExpired = false;

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@email", authReq.email);
            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email = @email", dic, authReq.email, false, "LoginUser");

            string usersFound = responsive.out_data[0]["0"];

            if (usersFound != "1")
            {
                throw new UserNotFoundException();
            }

            dic = new Dictionary<string, string>();
            dic.Add("@email", authReq.email);
            responsive = SqlExecuter.ExecFunction("SELECT * FROM user WHERE user_email=@email", dic, authReq.email, true, "LoginUser");

            string userId = responsive.out_data[0]["0"];
            string userEmail = responsive.out_data[0]["1"];
            string userName = responsive.out_data[0]["2"];
            string password = responsive.out_data[0]["3"];
            string userRoleId = responsive.out_data[0]["4"];
            string activeSince = responsive.out_data[0]["5"];
            string lastLogin = responsive.out_data[0]["6"];
            string language = responsive.out_data[0]["7"];
            string passExpires = responsive.out_data[0]["8"];
            string lastPassChange = responsive.out_data[0]["9"];
            string passHistory = responsive.out_data[0]["10"];

            if (ConfigManager.isProduction)
            {
                userName = Cryptography.Decrypt(userName, userEmail);
                passHistory = Cryptography.Decrypt(passHistory, userEmail);
            }

            userRec = new UserDBRecord(userId, userEmail, userName, password, userRoleId, activeSince, lastLogin, language, passExpires, lastPassChange, passHistory);

            dic = new Dictionary<string, string>();
            dic.Add("@last_login", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("@email", authReq.email);
            SqlExecuterItem responsive2 = SqlExecuter.ExecFunction("UPDATE user SET last_login=@last_login WHERE user_email=@email", dic, authReq.email, true, "LoginUser");

            return new UserSimple(userRec.UserName, Cryptography.GenerateJwtToken(userRec), UserModel.GetUserRole(userRec.UserRoleId), userPasswordExpired);
        }

        public static UserSimple RefreshAcessToken(string userEmailIn, string executer_user)
        {
            UserDBRecord userRec = null;
            bool userPasswordExpired = false;

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@email", userEmailIn);

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email=@email", dic, executer_user, false, "RefreshAcessToken");

            string usersFound = responsive.out_data[0]["0"];

            if (usersFound != "1")
            {
                throw new UserNotFoundException();
            }

            dic = new Dictionary<string, string>();
            dic.Add("@email", userEmailIn);
            SqlExecuterItem responsive1 = SqlExecuter.ExecFunction("SELECT * FROM user WHERE user_email=@email", dic, executer_user, true, "RefreshAcessToken");

            string userId = responsive1.out_data[0]["0"];
            string userEmail = responsive1.out_data[0]["1"];
            string userName = responsive1.out_data[0]["2"];
            string password = responsive1.out_data[0]["3"];
            string userRoleId = responsive1.out_data[0]["4"];
            string activeSince = responsive1.out_data[0]["5"];
            string lastLogin = responsive1.out_data[0]["6"];
            string language = responsive1.out_data[0]["7"];
            string passExpires = responsive1.out_data[0]["8"];
            string lastPassChange = responsive1.out_data[0]["9"];
            string passHistory = responsive1.out_data[0]["10"];

            if (ConfigManager.isProduction)
            {
                userName = Cryptography.Decrypt(userName, userEmail);
                passHistory = Cryptography.Decrypt(passHistory, userEmail);
            }

            userRec = new UserDBRecord(userId, userEmail, userName, password, userRoleId, activeSince, lastLogin, language, passExpires, lastPassChange, passHistory);

            dic = new Dictionary<string, string>();
            dic.Add("@last_login", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("@email", userEmailIn);
            SqlExecuterItem responsive2 = SqlExecuter.ExecFunction("UPDATE user SET last_login=@last_login WHERE user_email=@email", dic, executer_user, true, "RefreshAcessToken");

            List<DepartmentItem> departments = DepartmentModel.GetUserDepartments(userEmail, userEmail);

            return new UserSimple(userRec.UserName, Cryptography.GenerateJwtToken(userRec), UserModel.GetUserRole(userRec.UserRoleId), userPasswordExpired, departments);
        }

        public static bool AddAuth2FCode(string auth2f_code, string email, string language)
        {
            try
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("@token", auth2f_code);
                param.Add("@user_email", email);

                SqlExecuterItem response = SqlExecuter.ExecFunction("INSERT INTO `auth2f` (user_email, token) VALUES (@user_email, @token)", param, email, false, "AddAuth2FCode");

                // Sends an email to the user_email containing the auth2f_code
                EmailService.SendAuth2FToClient(email, auth2f_code, language);

                return true;
            }
            catch (Exception e)
            {
                Log.Debug(e.Message);
                return false;
            }
        }

        public static bool ValidateAuth2FCode(string auth2f_code, string email)
        {
            try
            {
                int delete_minutes = ConfigManager.DeleteOldAuthf2Codes();

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("@token", auth2f_code);
                param.Add("@user_email", email);
                param.Add("@delete_minutes", System.Convert.ToString(delete_minutes));

                Dictionary<string, string> dic = new Dictionary<string, string>();
                SqlExecuterItem responsive = SqlExecuter.ExecFunction("DELETE FROM auth2f WHERE created_at < DATE_SUB(NOW(), INTERVAL @delete_minutes MINUTE)", param, email, true, "ValidateAuth2FCode");
                SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT * FROM auth2f WHERE user_email=@user_email ORDER BY created_at DESC LIMIT 1", param, email, true, "ValidateAuth2FCode");

                ValidateAuth2fCodeItem rec = null;

                foreach (Dictionary<string, string> item in response.out_data)
                {
                    string user_email = item["0"];
                    string token = item["1"];
                    string created_at = (Convert.ToDateTime(item["2"])).ToString("yyyy-MM-dd HH:mm:ss");

                    //rec = new ValidateAuth2fCodeItem(user_email, token, createdAt);

                    DateTime createdAtTime = DateTime.Parse(created_at);
                    int min = ConfigManager.Auth2FCodeExpTime();

                    if (token == auth2f_code && (DateTime.UtcNow - createdAtTime).TotalMinutes < min)
                    {
                        //"min" minutes passed from start
                        return true;
                    }

                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static string generateNewAuth2fCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6);
        }

        private static void DefineAndSendUserRecoverPasswordToken(string userEmail)
        {
            DateTime expirationDate = DateTime.UtcNow.AddHours(ConfigManager.ResetTokenExpirationTime());
            string userName = string.Empty;
            string token = UserModel.generateNewToken();

            string recordsFound = "0";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@email", userEmail);
            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM reset_pass_token WHERE email=@email", dic, userEmail, false, "DefineAndSendUserRecoverPasswordToken");

            recordsFound = responsive.out_data[0]["0"];

            dic = new Dictionary<string, string>();
            dic.Add("@email", userEmail);
            SqlExecuterItem responsive1 = SqlExecuter.ExecFunction("SELECT * FROM user WHERE user_email=@email", dic, userEmail, true, "DefineAndSendUserRecoverPasswordToken");

            userName = responsive1.out_data[0]["2"];
            if (ConfigManager.isProduction)
            {
                userName = Cryptography.Decrypt(userName, userEmail);
            }
            if (recordsFound == "1")
            {
                dic = new Dictionary<string, string>();
                dic.Add("@expiration_time", expirationDate.ToString("yyyy-MM-dd HH:mm:ss"));
                dic.Add("@email", userEmail);
                dic.Add("@token", token);

                SqlExecuter.ExecFunction("UPDATE reset_pass_token SET token=@token, expiration_time=@expiration_time WHERE email=@email", dic, userEmail, true, "DefineAndSendUserRecoverPasswordToken");
            }
            else if (recordsFound == "0")
            {
                dic = new Dictionary<string, string>();
                dic.Add("@expiration_time", expirationDate.ToString("yyyy-MM-dd HH:mm:ss"));
                dic.Add("@email", userEmail);
                dic.Add("@token", token);

                SqlExecuter.ExecFunction("INSERT INTO reset_pass_token (email, token, expiration_time) VALUES (@email, @token, @expiration_time)", dic, userEmail, true, "DefineAndSendUserRecoverPasswordToken");
            }
            else
            {
                throw new DBWriteException("Unable to update reset_pass_token table - error - " + userEmail);
            }

            EmailService.SendResetPasswordEmail(userName, userEmail, token, expirationDate, "en");
        }
    }
}
