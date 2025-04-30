// // Copyright (c) 2024 Engibots. All rights reserved.


namespace engimatrix.ModelObjs.CTT;
public class CttMunicipalityItem
{
    public string cc { get; set; }
    public string dd { get; set; }
    public string name { get; set; }

    public CttMunicipalityItem(string cc, string dd, string name)
    {
        this.cc = cc;
        this.dd = dd;
        this.name = name;
    }
}
