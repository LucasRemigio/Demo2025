// // Copyright (c) 2024 Engibots. All rights reserved.

using Engimatrix.ModelObjs;

namespace engimatrix.ModelObjs
{
    public class SignatureItem
    {
        public string user_id { get; set; }
        public string signature { get; set; }

        public SignatureItem(string user_id, string signature)
        {
            this.user_id = user_id;
            this.signature = signature;
        }

        public SignatureItem()
        { }

        public SignatureItem ToItem()
        {
            return new SignatureItem(this.user_id, this.signature);
        }
    }
}
