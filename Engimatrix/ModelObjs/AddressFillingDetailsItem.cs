// // Copyright (c) 2024 Engibots. All rights reserved.

using engimatrix.ModelObjs.CTT;

namespace engimatrix.ModelObjs;
public class AddressFillingDetails
{
    public string municipality_cc { get; set; }
    public string district_dd { get; set; }
    public string cp4 { get; set; }
    public string cp3 { get; set; }
    public CttPostalCodeItem postal_code { get; set; }
    public int transport_id { get; set; }
    public string city { get; set; }
    public string address { get; set; }

    public AddressFillingDetails()
    { }

    public AddressFillingDetails(string municipality_cc, string district_dd, string cp4, string cp3, CttPostalCodeItem postal_code)
    {
        this.municipality_cc = municipality_cc;
        this.district_dd = district_dd;
        this.cp4 = cp4;
        this.cp3 = cp3;
        this.postal_code = postal_code;
    }
}
