using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class UserDepartmentResponse
    {
        
        public class GetDepartmentRolesIdsResponse
        {
            public string result { get; set; }
            public int result_code { get; set; }

            public Dictionary<string, string> departmentsAndIds { get; set; }

            public GetDepartmentRolesIdsResponse(Dictionary<string, string> departmentsAndIdsDic, int result_code, string language)
            {
                departmentsAndIds = departmentsAndIdsDic;

                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetDepartmentRolesIdsResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

        public class GetUserDepartmentRolesIdResponse
        {
            public string result { get; set; }
            public int result_code { get; set; }

            public List<string> userDepartmentRolesId { get; set; }

            public GetUserDepartmentRolesIdResponse(List<string> departmentIds, int result_code, string language)
            {
                userDepartmentRolesId = departmentIds;

                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetUserDepartmentRolesIdResponse(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }

    }
}
