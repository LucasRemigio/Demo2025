using engimatrix.ResponseMessages;
using System.Collections.Generic;
using engimatrix.ModelObjs.Orquestration;

namespace engimatrix.Views.Orquestration
{
    public class GetJobsResponse
    {
        public List<JobsItem> jobs_items { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public GetJobsResponse(List<JobsItem> jobsItems, int result_code, string language)
        {
            this.jobs_items = jobsItems;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public GetJobsResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }
}
