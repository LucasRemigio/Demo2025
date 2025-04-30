// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using Engimatrix.ModelObjs;
using engimatrix.Models;
using Microsoft.Graph.Models;
using engimatrix.Utils;
using engimatrix.Config;
using MimeKit;
using Engimatrix.Utils;
using Smartsheet.Api.Models;

namespace engimatrix.Models
{
    public static class DepartmentModel
    {
        public static List<DepartmentItem> GetDepartments(string executer_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT * FROM masterferro_engimatrix.department ORDER BY id;", dic, executer_user, true, "getDepartmentRolesAndIds");

            List<DepartmentItem> departmentList = [];

            foreach (Dictionary<string, string> item in response.out_data)
            {
                // id = item [0] ; dep Name = item[1]
                DepartmentItem department = new DepartmentItem(item["0"], item["1"]);
                departmentList.Add(department);
            }
            return departmentList;
        }
        public static List<DepartmentItem> GetUserDepartments(string userEmail, string executer_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("@user_email", userEmail);

            SqlExecuterItem response = SqlExecuter.ExecFunction("SELECT d.id, d.name FROM user_department u JOIN department d ON u.department_id = d.id WHERE u.user_email = @user_email ORDER BY d.id;", dic, executer_user, true, "getUserDepartments");

            List<DepartmentItem> departmentList = new();

            foreach (Dictionary<string, string> item in response.out_data)
            {
                // id = item [0] ; dep Name = item[1]
                DepartmentItem department = new DepartmentItem(item["0"], item["1"]);
                departmentList.Add(department);
            }
            return departmentList;
        }

        public static void SaveUserDepartments(string userEmail, List<DepartmentItem> departments, string executer_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("user_email", userEmail);
            foreach (DepartmentItem dept in departments)
            {
                dic["deptId"] = dept.id;
                SqlExecuter.ExecFunction("INSERT INTO user_department (`department_id`, `user_email`) VALUES (@deptId, @user_email)", dic, executer_user, false, "AssociateUserDepartment");
            }
        }
        public static void DeleteUserDepartments(string userEmail, string executer_user)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("userEmail", userEmail);
            SqlExecuter.ExecFunction("DELETE FROM user_department WHERE user_email = @userEmail", dic, executer_user, false, "DeleteUserDepartments");
        }

        public static void UpdateUserDepartments(string userEmail, List<DepartmentItem> departments, string executer_user)
        {
            DeleteUserDepartments(userEmail, executer_user);

            SaveUserDepartments(userEmail, departments, executer_user);
        }

    }
}
