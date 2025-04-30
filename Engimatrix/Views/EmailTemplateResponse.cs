// // Copyright (c) 2024 Engibots. All rights reserved.


using engimatrix.Views;

namespace Engimatrix.Views;
public class EmailTemplateResponse
{
    public class GetEmailTemplateResponse : BaseResponse
    {
        public string? template { get; set; }
        public string? signature { get; set; }

        public GetEmailTemplateResponse(string template, string signature, int result_code, string language) : base(result_code, language)
        {
            this.template = template;
            this.signature = signature;
        }

        public GetEmailTemplateResponse(int result_code, string language) : base(result_code, language)
        {
        }
    }
}

