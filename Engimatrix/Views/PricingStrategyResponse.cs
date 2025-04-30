// // Copyright (c) 2024 Engibots. All rights reserved.

using System.Text.Json.Serialization;
using engimatrix.Config;
using engimatrix.ModelObjs;
using engimatrix.Models;
using engimatrix.ResponseMessages;

namespace engimatrix.Views
{
    public class PricingStrategyResponse
    {
        public List<PricingStrategyItem> pricing_strategies { get; set; }
        public string result { get; set; }
        public int result_code { get; set; }

        public PricingStrategyResponse(List<PricingStrategyItem> pricing_strategies, int result_code, string language)
        {
            this.pricing_strategies = pricing_strategies;
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }

        public PricingStrategyResponse(int result_code, string language)
        {
            this.result_code = result_code;
            this.result = ResponseMessage.GetResponseMessage(result_code, language);
        }
    }

}
