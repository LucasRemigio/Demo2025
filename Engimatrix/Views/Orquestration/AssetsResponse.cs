// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs.Orquestration;
using engimatrix.ResponseMessages;

namespace engimatrix.Views.Orquestration
{
    public class AssetsResponse
    {
        public class GetAssets
        {
            public List<AssetsItem> assets { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetAssets(List<AssetsItem> assets, int result_code, string language)
            {
                this.assets = assets;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetAssets(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }
        public class GetAsset
        {
            public AssetsItem asset { get; set; }
            public string result { get; set; }
            public int result_code { get; set; }

            public GetAsset(AssetsItem asset, int result_code, string language)
            {
                this.asset = asset;
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }

            public GetAsset(int result_code, string language)
            {
                this.result_code = result_code;
                this.result = ResponseMessage.GetResponseMessage(result_code, language);
            }
        }
    }
}
