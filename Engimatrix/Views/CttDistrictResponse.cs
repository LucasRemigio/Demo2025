// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ModelObjs.CTT;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class CttDistrictListResponse : BaseResponse
    {
        public List<CttDistrictItem>? ctt_districts { get; set; }

        public CttDistrictListResponse(List<CttDistrictItem> ctt_districts, int result_code, string language) : base(result_code, language)
        {
            this.ctt_districts = ctt_districts;
        }

        public CttDistrictListResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }

}
