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
    public class UserModel
    {
        public static string GetUserRole(string roleId)
        {
            string userRole = "USER";

            Dictionary<string, string> dic = new()
            {
                { "@id", roleId }
            };
            SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user_role WHERE user_role_id=@id", dic, null, false, "GetUserRole");

            string rolesFound1 = response.out_data[0]["0"];
            int rolesFound = Int32.Parse(rolesFound1);

            if (rolesFound != 1)
            {
                return userRole;
            }

            dic = new Dictionary<string, string>
            {
                { "@id", roleId }
            };
            response = SqlExecuter.ExecFunction("SELECT * FROM user_role WHERE user_role_id=@id", dic, null, false, "GetUserRole");

            userRole = response.out_data[0]["1"];

            return userRole;
        }

        public static string GetUserEmailById(int userId)
        {
            Dictionary<string, string> dic = new()
            {
                { "@user_id", userId.ToString() }
            };
            SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT user_email FROM user WHERE user_id=@user_id", dic, userId.ToString(), false, "GetUserEmailById");

            return response.out_data[0]["0"];
        }

        public static string GetUserNameByEmail(string userEmail)
        {
            Dictionary<string, string> dic = new()
            {
                { "@userEmail", userEmail }
            };
            SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT user_name FROM user WHERE user_email=@userEmail", dic, userEmail, false, "GetUserNameByEmail");

            if (response.out_data[0].Count <= 0)
            {
                return String.Empty;
            }

            string userName = response.out_data[0]["0"];

            if (ConfigManager.isProduction)
            {
                userName = Cryptography.Decrypt(userName, userEmail);
            }

            return userName;
        }

        public static string getUserIdByEmail(string userEmail)
        {
            Dictionary<string, string> dic = new()
            {
                { "@userEmail", userEmail }
            };
            SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT user_id FROM user WHERE user_email=@userEmail", dic, userEmail, false, "GetUserNameByEmail");

            if (response.out_data[0].Count <= 0)
            {
                return String.Empty;
            }

            return response.out_data[0]["0"];
        }

        public static bool IsUserAdmin(string userEmail)
        {
            Dictionary<string, string> dic = new()
            {
                { "@user_email", userEmail }
            };
            SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email=@user_email", dic, userEmail, false, "IsUserAdmin");

            string usersFound = response.out_data[0]["0"];

            if (usersFound != "1")
            {
                return false;
            }

            dic = new Dictionary<string, string>
            {
                { "@user_email", userEmail }
            };
            response = SqlExecuter.ExecFunction("SELECT * FROM user WHERE user_email=@user_email", dic, userEmail, true, "IsUserAdmin");

            string userRoleId1 = response.out_data[0]["4"];
            int userRoleId = Int32.Parse(userRoleId1);

            if (userRoleId <= 2)
            {
                return true;
            }
            return false;
        }

        public static bool IsUserAdminOrSupervisor(string userEmail)
        {
            Dictionary<string, string> dic = new()
            {
                { "@user_email", userEmail }
            };

            SqlExecuterItem response = SqlExecuter.ExecuteFunction("SELECT user_role_id FROM user WHERE user_email=@user_email", dic, userEmail, true, "IsUserAdminOrSupervisor");

            // No user found
            if (response.out_data.Count == 0)
            {
                return false;
            }

            int userRoleId = Int32.Parse(response.out_data[0]["user_role_id"]);

            // anything that is not the user, will be either an admin or supervisor
            if (userRoleId != (int)UserRoleConstants.Role.USER)
            {
                return true;
            }

            return false;
        }

        public static string GetUserRoleIdByUser(string user_email)
        {
            Dictionary<string, string> dic = new()
            {
                { "@user_email", user_email }
            };
            SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT user_role_id FROM user WHERE user_email=@user_email", dic, user_email, false, "GetUserRoleIdByUser");

            return response.out_data[0]["0"];
        }

        internal static bool AddUser(AddUserRequest input, string language, string executer_user)
        {
            Dictionary<string, string> dic = new()
            {
                { "@user_email", input.email }
            };

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email=@user_email", dic, executer_user, false, "AddUser");
            string usersFound1 = responsive.out_data[0]["0"];
            int usersFound = Int32.Parse(usersFound1);
            if (usersFound > 0)
            {
                throw new UserExistsException();
            }

            InsertUserRecord(input.email, input.name, input.password, input.user_role_id, language, executer_user);

            // Generate his signature
            SignatureModel.SaveSignature(SignatureModel.GetDefaultFormattedSignature(input.email), input.email, executer_user);

            // Associate his departments
            DepartmentModel.SaveUserDepartments(input.email, input.departments, executer_user);

            return true;
        }

        internal static bool RemoveUser(RemoveUserRequest input, string executer_user)
        {
            Dictionary<string, string> dic = new()
            {
                { "@user_email", input.email }
            };

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email=@user_email", dic, executer_user, false, "RemoveUser");

            string usersFound1 = responsive.out_data[0]["0"];
            int usersFound = Int32.Parse(usersFound1);

            if (usersFound != 1)
            {
                throw new UserExistsException();
            }

            if (IsUserAdmin(input.email))
            {
                throw new DeleteAdminException();
            }

            // Delete all user associated departments
            DepartmentModel.DeleteUserDepartments(input.email, executer_user);

            // Delete user associated signature
            SignatureModel.DeleteSignature(input.email, executer_user);

            dic = new Dictionary<string, string>
            {
                { "@email", input.email }
            };
            SqlExecuter.ExecFunction("DELETE FROM user WHERE user_email=@email", dic, input.email, true, "RemoveUser");

            return true;
        }

        public static bool UpdateUser(UpdateUserRequest input, string loggedEmailUser, string executer_user)
        {
            Dictionary<string, string> dic = new()
            {
                { "@user_email", input.email }
            };

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email=@user_email", dic, input.email, false, "UpdateUser");

            string usersFound1 = responsive.out_data[0]["0"];
            int usersFound = Int32.Parse(usersFound1);

            if (usersFound != 1)
            {
                throw new UserExistsException();
            }

            // admin can only update his confis and not from other admins
            if (IsUserAdmin(input.email) && loggedEmailUser != input.email)
            {
                throw new UpdateAdminException();
            }

            string currentUserPass = string.Empty;
            string plainPassNewPass = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(input.updated_password));

            dic = new Dictionary<string, string>
            {
                { "@email", input.email }
            };
            SqlExecuterItem responsive1 = SqlExecuter.ExecFunction("SELECT * FROM user WHERE user_email=@email", dic, input.email, true, "UpdateUser");

            currentUserPass = responsive1.out_data[0]["3"];
            if (ConfigManager.isProduction)
                currentUserPass = Cryptography.Decrypt(currentUserPass, input.email);

            dic = new Dictionary<string, string>
            {
                { "@user_email", input.email }
            };
            if (ConfigManager.isProduction)
            {
                input.updated_name = Cryptography.Encrypt(input.updated_name, input.email);
            }
            dic.Add("@user_name", input.updated_name);

            SqlExecuter.ExecFunction("UPDATE user SET user_name=@user_name WHERE user_email=@user_email", dic, input.email, true, "UpdateUser");

            // detect user pass changes
            if (plainPassNewPass != currentUserPass)
            {
                ResetPassRequest inputResetPass = new();
                inputResetPass.email = input.email;
                inputResetPass.password = input.updated_password;
                // AuthenticationModel.ResetPass(inputResetPass, false, executer_user);
            }

            if (input.updated_departments.Count > 0)
            {
                DepartmentModel.UpdateUserDepartments(input.email, input.updated_departments, executer_user);
            }

            return true;
        }

        public static void InsertUserRecord(string userEmail, string userName, string userPass, int userRoleId, string language, string executer_user)
        {
            int passExpires = 1;
            // role 2 is admin
            if (userRoleId == 2)
            {
                passExpires = 0;
            }

            Dictionary<string, string> dic = new()
            {
                { "@user_email", userEmail }
            };
            string plainPass = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(userPass));
            if (ConfigManager.isProduction)
            {
                userName = Cryptography.Encrypt(userName, userEmail);
                userPass = Cryptography.Encrypt(plainPass, userEmail);
            }

            dic.Add("@user_name", userName);
            dic.Add("@user_password", userPass);
            dic.Add("@user_role_id", userRoleId.ToString());
            dic.Add("@active_since", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("@last_login", null);
            dic.Add("@language", language);
            dic.Add("@pass_expires", passExpires.ToString());
            dic.Add("@last_pass_change", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("@pass_history", string.Empty);

            SqlExecuter.ExecFunction("INSERT INTO user (user_email, user_name, user_password, user_role_id, active_since, last_login, language, pass_expires, last_pass_change, pass_history)  VALUES " +
            "(@user_email, @user_name, @user_password, @user_role_id, @active_since, @last_login, @language, @pass_expires, @last_pass_change, @pass_history)", dic, executer_user, true, "InsertUserRecord");
        }

        internal static bool SendUserCredentials(SendCredentialsRequest input)
        {
            UserDBRecord userRec = null;
            string language = "en";

            Dictionary<string, string> dic = new()
            {
                { "@email", input.email }
            };
            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email=@email", dic, input.email, false, "SendUserCredentials");

            string usersFound1 = responsive.out_data[0]["0"];
            int usersFound = Int32.Parse(usersFound1);

            if (usersFound != 1)
            {
                throw new UserExistsException();
            }
            dic = new Dictionary<string, string>
            {
                { "@email", input.email }
            };
            responsive = SqlExecuter.ExecFunction("SELECT * FROM user WHERE user_email=@email", dic, input.email, true, "SendUserCredentials");

            string userId = responsive.out_data[0]["0"];
            string userEmail = responsive.out_data[0]["1"];
            string userName = responsive.out_data[0]["2"];
            string password = responsive.out_data[0]["3"];
            string userRoleId = responsive.out_data[0]["4"];
            string activeSince = responsive.out_data[0]["5"];
            string lastLogin = responsive.out_data[0]["6"];
            language = responsive.out_data[0]["7"];
            string passExpires = responsive.out_data[0]["8"];
            string lastPassChange = responsive.out_data[0]["9"];
            string passHistory = responsive.out_data[0]["10"];
            if (ConfigManager.isProduction)
            {
                userName = Cryptography.Decrypt(userName, userEmail);
                passHistory = Cryptography.Decrypt(passHistory, userEmail);
            }

            userRec = new UserDBRecord(userId, userEmail, userName, password, userRoleId, activeSince, lastLogin, language, passExpires, lastPassChange, passHistory);

            EmailService.SendCredentialsEmail(userRec.UserName, userRec.UserEmail, userRec.UserPassword, language);
            return true;
        }

        public static List<UserItem> GetUsers(string userFilterEmail, string executer_user)
        {
            List<UserItem> result = [];
            Dictionary<string, string> parameters = [];
            string query;

            if (!string.IsNullOrEmpty(userFilterEmail))
            {
                parameters.Add("@user_email", "%" + userFilterEmail + "%");
                query = "SELECT * FROM user WHERE user_email LIKE @user_email";
            }
            else
            {
                query = "SELECT * FROM user";
            }

            SqlExecuterItem response = SqlExecuter.ExecFunction(query, parameters, executer_user, false, "GetUsers");

            if (response.out_data.Count == 0)
            {
                return result;
            }

            foreach (Dictionary<string, string> item in response.out_data)
            {
                string userId = item["0"];
                string userEmail = item["1"];
                string userName = item["2"];
                string password = item["3"];
                string userRoleId = item["4"];
                string activeSince = item["5"];
                string lastLogin = item["6"];
                string language = item["7"];
                string passExpires = item["8"];
                string lastPassChange = item["9"];
                string passHistory = item["10"];

                if (ConfigManager.isProduction)
                {
                    userName = Cryptography.Decrypt(userName, userEmail);
                    passHistory = Cryptography.Decrypt(passHistory, userEmail);
                    password = Cryptography.Decrypt(password, userEmail);
                }

                List<DepartmentItem> departments = DepartmentModel.GetUserDepartments(userEmail, executer_user);
                string role = GetUserRole(userRoleId);
                UserItem userRec = new(userEmail, userName, string.Empty, activeSince, lastLogin, role, departments);
                result.Add(userRec);
            }

            return result;
        }

        internal static bool UpdatePass(UpdatePasswordRequest input, string execute_user)
        {
            Dictionary<string, string> dic = new()
            {
                { "@user_email", input.email }
            };

            SqlExecuterItem responsive = SqlExecuter.ExecFunction("SELECT COUNT(*) FROM user WHERE user_email=@user_email", dic, execute_user, false, "UpdatePass");

            string usersFound1 = responsive.out_data[0]["0"];
            int usersFound = Int32.Parse(usersFound1);

            if (usersFound != 1)
            {
                throw new UserExistsException();
            }

            string currentUserPass = string.Empty;
            string OldPass = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(input.OldPass));
            string plainPassNewPass = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(input.NewPass));

            dic = new Dictionary<string, string>
            {
                { "@email", input.email }
            };

            SqlExecuter.ExecFunction("SELECT * FROM user WHERE user_email=@email", dic, execute_user, true, "UpdatePass");

            currentUserPass = Cryptography.Decrypt(responsive.out_data[0]["10"], input.email);

            if (OldPass != currentUserPass)
            {
                throw new PasswordNotEqualToOriginalException("Validation of password not successful, Old password is not equal to the one in the DataBase - email" + input.email);
            }

            if (plainPassNewPass == currentUserPass)
            {
                throw new UpdatePasswordException("New password equals current password - email" + input.email);
            }

            // detect user pass changes
            if (plainPassNewPass != currentUserPass)
            {
                ResetPassRequest inputResetPass = new();
                inputResetPass.email = input.email;
                AuthenticationModel.ResetPass(inputResetPass, false, execute_user);
            }

            return true;
        }

        public static string GetUserByToken(string token)
        {
            int userId = Cryptography.GetUserJwtToken(token);

            if (userId == 0)
            {
                throw new UserNotFoundException();
            }

            return UserModel.GetUserEmailById(userId);
        }

        public static string CreatePassword()
        {
            int lenghtThreshold = 20;
            string validCharsLowerLetters = "abcdefghijklmnopqrstuvwxyz";
            string validCharsUpperLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string validCharsNumbers = "1234567890";
            string validCharsSpecialChars = "!@#$%^&*()+=-|?/<>,.";

            StringBuilder generatedPassword = new();
            Random rnd = new();
            for (int i = 0; i < (lenghtThreshold / 4); i++)
            {
                int indexLowerLetters = rnd.Next(validCharsLowerLetters.Length);
                int indexUpperLetters = rnd.Next(validCharsUpperLetters.Length);
                int indexCharsNumbers = rnd.Next(validCharsNumbers.Length);
                int indexCharsSpecialChars = rnd.Next(validCharsSpecialChars.Length);

                generatedPassword.Append(validCharsLowerLetters[indexLowerLetters].ToString() + validCharsUpperLetters[indexUpperLetters].ToString() +
                    validCharsNumbers[indexCharsNumbers].ToString() + validCharsSpecialChars[indexCharsSpecialChars].ToString());
            }

            return generatedPassword.ToString();
        }

        public static string generateNewToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
