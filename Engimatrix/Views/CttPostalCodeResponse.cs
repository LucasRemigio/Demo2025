// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs.CTT;

namespace engimatrix.Views
{
    public class CttPostalCodeDtoListResponse : BaseResponse
    {
        public List<CttPostalCodeDto>? ctt_postal_codes { get; set; }

        public CttPostalCodeDtoListResponse(List<CttPostalCodeDto> ctt_postal_codes, int result_code, string language) : base(result_code, language)
        {
            this.ctt_postal_codes = ctt_postal_codes;
        }

        public CttPostalCodeDtoListResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }
}
