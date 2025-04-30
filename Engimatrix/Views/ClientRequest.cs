// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.Utils;

namespace engimatrix.Views
{
    public class ClientRequest
    {
        public class AddAddress
        {
            public string json_content { get; set; }

            public bool Validate()
            {
                if (!Util.IsValidInputString(json_content))
                {
                    return false;
                }

                return true;
            }
        }

        public class EditAddresses
        {
            public Address[] addresses { get; set; }

            public bool Validate()
            {
                foreach (Address item in addresses)
                {
                    bool valid = item.Validate();
                    if (!valid)
                        return false;
                }

                return true;
            }
        }

        public class Address
        {
            public string entity_id { get; set; }
            public string address_id { get; set; }
            public string address { get; set; }
            public string locality { get; set; }
            public string zip_code { get; set; }
            public string zip_locality { get; set; }
            public string phone { get; set; }
            public string mobile_phone { get; set; }
            public string email { get; set; }
            public string name { get; set; }
            public string token { get; set; }

            public bool Validate()
            {
                if (!Util.IsValidInputString(entity_id) || !Util.IsValidInputString(address_id) || !Util.IsValidInputString(address) || !Util.IsValidInputString(locality) || !Util.IsValidInputString(zip_code) || !Util.IsValidInputString(zip_locality) || !Util.IsValidInputEmail(email) || !Util.IsValidInputString(name) || !Util.IsValidInputString(token))
                {
                    return false;
                }

                return true;
            }
        }

        public class AddExclusionClient
        {
            public string client_id { get; set; }
            public string client_email { get; set; }
            public string client_vat { get; set; }

            public bool Validate()
            {
                if (!Util.IsValidInputString(client_id) && !Util.IsValidInputString(client_email) && !Util.IsValidInputString(client_vat))
                {
                    return false;
                }

                return true;
            }
        }
    }
}
