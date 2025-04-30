// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs;
using engimatrix.ModelObjs.CTT;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class CttMunicipalityListResponse : BaseResponse
    {
        public List<CttMunicipalityItem>? ctt_municipalities { get; set; }

        public CttMunicipalityListResponse(List<CttMunicipalityItem> ctt_municipalities, int result_code, string language) : base(result_code, language)
        {
            this.ctt_municipalities = ctt_municipalities;
        }

        public CttMunicipalityListResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }

    public class CttMunicipalityDtoListResponse : BaseResponse
    {
        public List<CttMunicipalityDto>? ctt_municipalities { get; set; }

        public CttMunicipalityDtoListResponse(List<CttMunicipalityDto> ctt_municipalities, int result_code, string language) : base(result_code, language)
        {
            this.ctt_municipalities = ctt_municipalities;
        }

        public CttMunicipalityDtoListResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }
}
