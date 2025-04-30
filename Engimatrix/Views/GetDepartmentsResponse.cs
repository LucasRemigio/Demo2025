// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ResponseMessages;
using Engimatrix.ModelObjs;

namespace engimatrix.Views
{
    public class GetDepartmentsResponse
    {
        public List<DepartmentItem> departments { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public GetDepartmentsResponse(List<DepartmentItem> departments, int result_code, string language)
        {
            this.departments = departments;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public GetDepartmentsResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
